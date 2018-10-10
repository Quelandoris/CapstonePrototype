using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraVolume : MonoBehaviour {

    public Camera myCam;
    public Player player;

    //Find player, assign him for future use
    private void OnEnable()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Player")
        {
            myCam.gameObject.SetActive(true);
            player.curCamera = myCam;
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
