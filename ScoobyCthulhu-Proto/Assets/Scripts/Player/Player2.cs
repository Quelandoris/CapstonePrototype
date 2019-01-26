using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour {

    public float MoveSpeed = 5f;
    Vector3 movement;
    Rigidbody myRB;
    int TargetableMask;
    int FloorMask;
    public float Deadzone; //How much does the analog stick need to be pushed to register
    public float turnSpeed;

    public Camera curCamera;
    float camRayLength = 100f;
    public GameObject Flashlight;
    Inventory inv;
    private void Start()
    {
        FloorMask = LayerMask.GetMask("Floor");
        TargetableMask = LayerMask.GetMask("Targetable");

        myRB = GetComponent<Rigidbody>();
        inv = GameObject.Find("InvPanel").GetComponent<Inventory>();
    }
    private void FixedUpdate()
    {
        //movement horizontal and vertical
        //Add Deadzones
        movement = Vector3.zero;
        float h=0;
        float v=0;
        if (Mathf.Abs(Input.GetAxis("P2_LAnalog_H")) > Deadzone)
        {
            h = Input.GetAxis("P2_LAnalog_H");
            Debug.Log("Horiz" + h);
        }
        if (Mathf.Abs(Input.GetAxis("P2_LAnalog_V")) > Deadzone)
        {
            v = Input.GetAxis("P2_LAnalog_V");
            Debug.Log("Vert" + h);
        }
        Move(h, v);
    }
    private void Move(float h, float v)
    {
        // movement.Set(h, 0f, v);
        // movement = movement.normalized * MoveSpeed * Time.deltaTime;
        //movement = Camera.main.transform.TransformDirection(movement);
        if (v != 0)
        {
            movement = transform.forward * v;
            // movement.y = 0.0f;
            myRB.MovePosition(transform.position + movement / 20);
        }
        if (h != 0)
        {
            float turn = h * turnSpeed;
            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
            myRB.MoveRotation(myRB.rotation * turnRotation);
        }
    }
}
