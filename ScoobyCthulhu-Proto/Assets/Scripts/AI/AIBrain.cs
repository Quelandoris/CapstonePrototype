using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBrain : MonoBehaviour {

    //General Variables
    public bool debugging = false;
    public Transform NavTarget;
    NavMeshAgent agent;
    public enum modes { Passive, Alert, Chase};
    public modes mode;
    private GameObject player; //replace this with the object type of player.
    public float BaseSpeed = 1; //Speed the monster normally moves at
    public float ChaseSpeed = 1.5f; //Speed on Monster while chasing
    Renderer Rend;

    //AI Passive Walk
    Transform[] NavTargets;

    //AI Alert
    public int AlertTime; //Time AI stays in the Alert State in Seconds
    public int retargetTime; //Seconds it takes for the AI to choose a new location within SearchDistance
    public float SearchDistance; //How far the AI will stray from the source of the sound while searching
    public Vector3 SearchTarget;

    //Sight
    public float SightWidth; //This is half of it vision width
    public float SightDistance;
    public bool seen; //Can the monster currently see the player?
    public float ChaseDistance; //Distance at which monster will begin chasing

    // Use this for initialization
    void OnEnable () {
       
        mode = modes.Passive;
        agent = GetComponent<NavMeshAgent>();
        SearchTarget = Vector3.zero;
        player = GameObject.FindGameObjectWithTag("Player");
        Rend = GetComponent<Renderer>();//for color switch
        //Fill NavTargets
        int loopnum = 0;
        NavTargetScript[] temp = FindObjectsOfType<NavTargetScript>();
        NavTargets = new Transform[temp.Length];
        //foreach (NavTargetScript s in FindObjectsOfType<NavTargetScript>())
        foreach (NavTargetScript s in temp)
        {
            NavTargets[loopnum] = s.GetComponent<Transform>();
            loopnum++;
        }
                
	}
	
	void OnDisable(){
        //reset NavTargets in case some get disabled between this object getting disabled and reenabled
        Array.Clear(NavTargets, 0, NavTargets.Length);
	}

    //The Devil
    //Calls what mode AI is in. Resource heavy, so the individual modes are meant to be fairly light
    void Update()
    {
        if (debugging) Debug.DrawRay(transform.position, transform.forward * 3, Color.red);
        Sight();
        switch (mode)
        {
            case modes.Passive:
                AIPassiveWalk();
                Rend.material.color = Color.white;
                break;
            case modes.Alert:
                AIAlert();
               Rend.material.color = Color.yellow;
                break;
            case modes.Chase:
                AIChase();
               Rend.material.color = Color.red;
                break;
            default:
                Debug.Log("Mode Switch in AIBrain Update broke somehow.");
                break;
        }
    }

    //AIPassiveWalk is used when the AI has no other task it is persuing; it will patrol between different points depending on how long it has been since it last went to that point, with some RNG
    //It does this until an Event call changes its state.
    void AIPassiveWalk()
    {
        //If there is no navTarget (Usually at start of game or if the navtarget gets disabled) find a new navtarget
        if (NavTarget==null){
			changeTarget();
		}
		
    }
    //Called by NavTargetScript on collision with Monster
    public void NavTargetReached(Transform tr)
    {
        if (NavTarget == tr)
        {
            //choose new navtarget
            changeTarget();
        }
    }
    //Selects new TargetNavPoint from Array based on what has highest Priority
    void changeTarget()
    {
        Transform highestPriority = NavTargets[0];
		foreach(Transform x in NavTargets){
            //Prevent nullref
            if (x.GetComponent<NavTargetScript>())
            {
                //Compare priority, if true reassign highestPriority
                if (x.GetComponent<NavTargetScript>().priority > highestPriority.GetComponent<NavTargetScript>().priority)
                {
                    highestPriority = x;
                }
            }
		}
		//After highestPriority is found, assign it to myAIMove
		NavTarget = highestPriority;
        agent.SetDestination(NavTarget.position);
    }

    //AIAlert is used when the AI has seen or heard something; it will advance to the source of the sound, then wander the area for a set amount of time.
    void AIAlert()
    {
        float dist;
        dist = Vector3.Distance(GetComponent<Transform>().position, SearchTarget);
        if (dist < SearchDistance && !IsInvoking("ChangeSearchTarget"))
        {
            Invoke("ChangeSearchTarget", retargetTime); //Ideally this would be done with a coroutine in the full capstone but I want this to get working
        }
        else if(dist>SearchDistance)
        {
            agent.SetDestination(transform.localPosition);
        }
    }
    

    //Calculates the location for the AI to wander inside of
    static Vector3 WanderSphere(Vector3 origin, float dist, int layermask)
    {
        //Get a point inside the sphere
        Vector3 ranDir = UnityEngine.Random.insideUnitSphere * dist;
        //Make the point apply to the origin of the noise/sight
        ranDir += origin;
        //Calculate a target point
        NavMeshHit navHit;
        NavMesh.SamplePosition(ranDir, out navHit, dist, layermask);

        return navHit.position;
    }
    //Set the location calulated in WanderSphere
    void ChangeSearchTarget()
    {
        agent.SetDestination(WanderSphere(SearchTarget, SearchDistance, -1));
    }

    public void EnterAlert(Vector3 tr)
    {
        //Cancel any current invokes, in case one is running from a previous Alert or Chase
        CancelInvoke();
        NavTarget = null;
        SearchTarget = tr;
        Invoke("EndSearch", AlertTime);
        mode = modes.Alert;
        agent.speed = BaseSpeed;
    }
    //If nothing is found, return to Passive
    void EndSearch()
    {
        CancelInvoke();
        mode = modes.Passive;
        agent.speed = BaseSpeed;
        SearchTarget = Vector3.zero;
        changeTarget();
    }
    //Sight; do a raycast, check and compare the angle and distance of the player object
    void Sight()
    {
        //Make sure player exists
        if (player != null)
        {
            //check distance and Angle; if this doesn't return true, don't bother with the raycast
            if (Vector3.Distance(player.GetComponent<Transform>().position, GetComponent<Transform>().position) < SightDistance &&
                (Vector3.Angle(player.GetComponent<Transform>().position, GetComponent<Transform>().position) < SightWidth || Vector3.Angle(player.GetComponent<Transform>().position, GetComponent<Transform>().position) >= -SightWidth))
            {
                //Do the check
                RaycastHit hit;
                if (Physics.Raycast(GetComponent<Transform>().position, player.GetComponent<Transform>().position-transform.position, out hit, SightDistance))
                {
                    //if it hit something (it almost certainly should), return its tag. if its player, increase alert 
                    if (hit.transform.tag == "Player")
                    {
                        //If player is close enough, enter chase
                        if (Vector3.Distance(player.GetComponent<Transform>().position, GetComponent<Transform>().position) < ChaseDistance)
                        {
                            seen = true;
                            EnterChase(hit.transform.position);
                        }
                        //Otherwise, go alert and move towards player.
                        else if (seen == false)
                        {
                            EnterAlert(hit.point);
                        }
                    }
                }
                else
                {
                    seen = false;
                }
            }
        }
    }
    //Enter chase; go to the player while they are in sight, try to kill, then search area
    public void EnterChase(Vector3 tr)
    {
        //Cancel any current invokes, in case one is running from a previous Alert
        
        CancelInvoke();
        NavTarget = null;
        SearchTarget = tr;
        mode = modes.Chase;
        agent.speed = ChaseSpeed;
    }
    //AI for when monster is chasing. 
    void AIChase()
    {
        agent.SetDestination(SearchTarget);
        //When destination reached, go into aleart for the area
        float dist = agent.remainingDistance;
        if (dist != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance <= 0.5)
        {
            EnterAlert(SearchTarget);
            seen = false;
        }
        //Kill the player. I need to ask Connor how we want to handle this
    }
}
