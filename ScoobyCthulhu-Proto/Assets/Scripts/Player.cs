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
    
    
    public Camera curCamera;
    float camRayLength = 100f;
    public GameObject Flashlight;
    public GameObject dog;
    int oilCount = 0;
    Inventory inv;
    public GameObject PlayerArm;
    private void Start()
    {
        FloorMask = LayerMask.GetMask("Floor");
        TargetableMask = LayerMask.GetMask("Targetable");
        
        myRB = GetComponent<Rigidbody>();
        inv = GameObject.Find("InvPanel").GetComponent<Inventory>();
        PlayerArm = GameObject.Find("ThrowArm");
    }
    private void FixedUpdate()
    {
        //movement horizontal and vertical
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turn(h);//for the player and the flashlight
        Fetch();//for when the player needs to send the dog somewhere
    }
    void Update()
    {
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * MoveSpeed;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * MoveSpeed;
    }
    private void Move(float h, float v)
    {
        // movement.Set(h, 0f, v);
        // movement = movement.normalized * MoveSpeed * Time.deltaTime;
        //movement = Camera.main.transform.TransformDirection(movement);
        movement = transform.forward * v;
       // movement.y = 0.0f;
        myRB.MovePosition(transform.position + movement/20);
        float turn = h* 5;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        myRB.MoveRotation(myRB.rotation * turnRotation);
    }
    void Turn(float h)
    {


         Ray camRay = curCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit TargetHit;

        if (Physics.Raycast(camRay, out TargetHit, camRayLength))
        {
            Flashlight.transform.LookAt(TargetHit.point);
            
        }
        if (Physics.Raycast(camRay, out TargetHit, camRayLength, FloorMask))
        {
          //  Vector3 playerToMouse = TargetHit.point - transform.position;
         //   playerToMouse.y = 0f;
         //   Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
         //   myRB.MoveRotation(newRotation);
            PlayerArm.transform.LookAt(TargetHit.point);
            PlayerArm.GetComponent<PlayerArm>().Target = TargetHit.point;
        }
        

    }
    void Fetch()
    {
        if (Input.GetButtonDown("Fire1"))//if left mouse clicked
        {
            if (inv.position == 1)//if dog
            {
                //Debug.Log("worrking");
                 dog.GetComponent<DogMovement>().GoFetch = true;
                dog.GetComponent<DogMovement>().Fetching = true;
            }
            if (inv.position == 0)//if oil
            {
                if (oilCount >=0)
                {
                   // Debug.Log("working");
                    PlayerArm.GetComponent<PlayerArm>().Shoot();
                    oilCount--;
                }
                
            }
        }
        
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "OilBottle")
        {
            oilCount = 5;
            inv.inv1 = true;
            //send to inv
            Destroy(other.gameObject);
        }
    }
}
