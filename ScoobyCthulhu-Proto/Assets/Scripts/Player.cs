using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float MoveSpeed = 5f;
    Vector3 movement;
    Rigidbody myRB;
    int TargetableMask;
    int FloorMask;
    
    
    float camRayLength = 100f;
    public GameObject Flashlight;
    GameObject dog;
    bool hasOil = false;
    Inventory inv;
    PlayerArm playerArm;
    private void Awake()
    {
        FloorMask = LayerMask.GetMask("Floor");
        TargetableMask = LayerMask.GetMask("Targetable");
        dog = GameObject.Find("Dog");
        myRB = GetComponent<Rigidbody>();
        inv = GameObject.Find("InvPanel").GetComponent<Inventory>();
        playerArm = GameObject.Find("ThrowArm").GetComponent<PlayerArm>();
    }
    private void FixedUpdate()
    {
        //movement horizontal and vertical
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turn();//for the player and the flashlight
        Fetch();//for when the player needs to send the dog somewhere
    }
    void Update()
    {
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

        if (Physics.Raycast(camRay, out TargetHit, camRayLength))
        {
            Flashlight.transform.LookAt(TargetHit.point);
        }
        if (Physics.Raycast(camRay, out TargetHit, camRayLength, FloorMask))
        {
            Vector3 playerToMouse = TargetHit.point - transform.position;
            playerToMouse.y = 0f;
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            myRB.MoveRotation(newRotation);
        }
    }
    void Fetch()
    {
        if (Input.GetButtonDown("Fire1"))//if left mouse clicked
        {
            if (inv.position == 1)
            {
                dog.GetComponent<DogMovement>().GoFetch = true;
                dog.GetComponent<DogMovement>().Fetching = true;
            }
            if (inv.position == 0)
            {
                if (hasOil)
                {
                    Debug.Log("working");
                    playerArm.Shoot();
                }
            }
        }
        
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "OilBottle")
        {
            hasOil = true;
            inv.inv1 = true;
            //send to inv
            Destroy(other.gameObject);
        }
    }
}
