using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraVolume : MonoBehaviour {

    public Camera myCam;

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Player")
        {
            myCam.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider coll)
    {
        if (coll.tag == "Player")
        {
            myCam.gameObject.SetActive(false);
        }
    }
}
