using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float MoveSpeed = 5f;
    Vector3 movement;
    Rigidbody myRB;
    int TargetableMask;                      
    float camRayLength = 100f;
    private void Awake()
    {
        TargetableMask = LayerMask.GetMask("Targetable");
        myRB = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        //movement horizontal and vertical
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turn();
        
    }
    void Update () {
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * MoveSpeed;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * MoveSpeed;

        
        
    }
    private void Move(float h, float v)
    {
        movement.Set(h, 0f, v);
        movement = movement.normalized * MoveSpeed * Time.deltaTime;
        myRB.MovePosition(transform.position + movement);
    }
    void Turn()
    {
        
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);   
        RaycastHit TargetHit;

        if (Physics.Raycast(camRay, out TargetHit, camRayLength, TargetableMask))
        {
           
            Vector3 playerToMouse = TargetHit.point - transform.position;

            
           playerToMouse.y = 0f;

           
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            
            myRB.MoveRotation(newRotation);
        }
    }
}
