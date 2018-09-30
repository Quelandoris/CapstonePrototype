using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBrain : MonoBehaviour {

    //General Variables
    AIMove myAIMove = new AIMove();
	
    //AI Passive Walk
	public Dictionary<Transform, int> NavTargets = new Dictionary<Transform,int>();

    // Use this for initialization
    void OnEnable () {
        myAIMove = GetComponent<AIMove>();
		AIEventManager.NavReached += NavTargetReached;
        //Convert Targets into NavTargetPriorities, and give them the default priority = 50
                
	}
	
	void OnDisable(){
		AIEventManager.NavReached -= NavTargetReached;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void NavTargetReached(){
		if(myAIMove.NavTarget==tran){
			changeTarget();
		}
	}
	
    //AIPassiveWalk is used when the AI has no other task it is persuing; it will patrol between different points depending on how long it has been since it last went to that point, with some RNG
    //It does this until an Event call changes its state.
    void AIPassiveWalk()
    {
        if(myAIMove.NavTarget==null){
			changeTarget();
		}
		
    }
	
	void changeTarget(){
		Transform highestPriority;
		foreach(Transform x in NavTargets.GetKeys){
			if(NavTargets[x].GetValue>highestPriority) highestPriority=x;
		}
		
		myAIMove.NavTarget = highestPriority;
	}
}
