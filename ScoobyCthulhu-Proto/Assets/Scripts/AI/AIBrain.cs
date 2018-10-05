using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBrain : MonoBehaviour {

    //General Variables
    public Transform NavTarget;
    NavMeshAgent agent;
    public enum modes { Passive, Alert, Chase};
    public modes mode;

    //AI Passive Walk
    Transform[] NavTargets;

    //AI Alert
    public int AlertTime; //Time AI stays in the Alert State in Seconds
    public int retargetTime; //Seconds it takes for the AI to choose a new location within SearchDistance
    public float SearchDistance; //How far the AI will stray from the source of the sound while searching
    public Transform SearchTarget;

    // Use this for initialization
    void OnEnable () {
        mode = modes.Passive;
        agent = GetComponent<NavMeshAgent>();
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
        switch (mode)
        {
            case modes.Passive:
                AIPassiveWalk();
                break;
            case modes.Alert:
                AIAlert();
                break;
            case modes.Chase:

                break;
            default:

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
        dist = Vector3.Distance(GetComponent<Transform>().position, SearchTarget.position);
        if (dist < SearchDistance && !IsInvoking("ChangeSearchTarget"))
        {
            Invoke("ChangeSearchTarget", retargetTime); //Ideally this would be done with a coroutine in the full capstone but I want this to get working
        }
        else if(dist>SearchDistance)
        {
            agent.SetDestination(SearchTarget.position);
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
        agent.SetDestination(WanderSphere(SearchTarget.position, SearchDistance, -1));
    }

    public void EnterAlert(Transform tr)
    {
        //Cancel any current invokes, in case one is running from a previous Alert or Chase
        CancelInvoke();
        NavTarget = null;
        SearchTarget = tr;
        Invoke("EndSearch", AlertTime);
        mode = modes.Alert;
    }
    //If nothing is found, return to Passive
    void EndSearch()
    {
        CancelInvoke();
        mode = modes.Passive;
        SearchTarget = null;
        changeTarget();
    }
}
