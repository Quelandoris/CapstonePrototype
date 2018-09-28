using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBrain : MonoBehaviour {

    //General Variables
    AIMove myAIMove = new AIMove();

    //AI Passive Walk
    public List<Transform> Targets = new List<Transform>();
    List<int> NavTargetPrioties = new List<int>();
    
    int curTarget; //Tracks which Target the AI should be going towards

    // Use this for initialization
    void Start () {
        myAIMove = GetComponent<AIMove>();
        //Convert Targets into NavTargetPriorities, and give them the default priority = 50
        for (int i = 0; i < Targets.Count; i++)
        {
            NavTargetPrioties[i] = 50;
        }
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //AIPassiveWalk is used when the AI has no other task it is persuing; it will patrol between different points depending on how long it has been since it last went to that point, with some RNG
    //It does this until an Event call changes its state.
    void AIPassiveWalk()
    {
        //Check if AIMove has a target. Usually this is only run at the start of the game
        if (myAIMove.NavTarget == null)
        {
            //Choose the first target randomly Round since it needs to be a whole number
            curTarget = Random.RandomRange(0, Targets.Count);
            myAIMove.NavTarget = Targets[curTarget];
        }
        //Check when AI arrives at point.
        if (){
            //reset current target's priority
            NavTargetPrioties[curTarget] = 0;
            //Check which NavTarget has highest priority
            int val = 0;
            for(int i=0;i<NavTargetPrioties.Count;i++){
                if (NavTargetPrioties[i] > NavTargetPrioties[val]) val = i;
            }
            curTarget = val;
            myAIMove.NavTarget = Targets[curTarget];
        }
    }
}
