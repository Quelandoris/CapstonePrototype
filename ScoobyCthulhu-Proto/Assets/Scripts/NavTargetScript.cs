using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavTargetScript : MonoBehaviour {

	void OnEnable(){
		AIBrain.NavTargets.Add(GetComponent<Transform>(),50);
	}
	void OnDisable(){
		AIBrain.NavTargets.Remove(GetComponent<Transform>(),50);
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider coll) {
		if(coll.gameobject.tag=="Monster"){
			AIEventManager.NavReached(GetComponent<Transform>());
		}
	}
}
