using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraVolume : MonoBehaviour {

    public Camera myCam;
    public Player player;
    public DogMovement dog;

    //Find player, assign him for future use
    private void OnEnable()
    {
        player = FindObjectOfType<Player>();
        dog = FindObjectOfType<DogMovement>();
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Player")
        {
            myCam.gameObject.SetActive(true);
            player.curCamera = myCam;
            dog.curCamera = myCam;
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
