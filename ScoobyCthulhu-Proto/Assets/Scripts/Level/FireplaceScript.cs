using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireplaceScript : MonoBehaviour {

    public GameObject fireplaceTrap; //The trap version of the fireplace
    public GameObject Fireplace; //The unlit fireplace

    // Use this for initialization
    void OnTriggerEnter (Collider coll) {
        if (coll.tag == "Oil")
        {
            Debug.Log("Coll");
            coll.gameObject.SetActive(false);
            fireplaceTrap.SetActive(true);
            Fireplace.SetActive(false);
        }
	}
	
}
