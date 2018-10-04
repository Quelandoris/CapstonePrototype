using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavTargetScript : MonoBehaviour {

    //Priority represents the Monster's order for going to each point. See AIBrain.changeTarget()
    public int priority;

	void OnEnable(){
        priority = Random.Range(40,55);
        //priority will increase by 1 every 10 seconds. This also keeps stuff out of update, since update is the devil.
        InvokeRepeating("increasePriority", 10, 10);
	}
	
	
	// Update is called once per frame
	void OnTriggerEnter (Collider coll) {
		if(coll.gameObject.tag=="Monster"){
            coll.GetComponent<AIBrain>().NavTargetReached(GetComponent<Transform>());
        }
	}

    void increasePriority()
    {
        priority++;
    }
}
