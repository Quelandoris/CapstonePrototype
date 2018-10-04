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
            //Reset current nav's priority
            NavTarget.GetComponent<NavTargetScript>().priority = 0;
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
        agent.destination = NavTarget.position;
    }
}
