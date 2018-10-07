using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogMovement : MonoBehaviour {

    public Transform NavTarget;
    NavMeshAgent agent;
    public enum modes { Passive, Fetch, Stay};
    public modes mode;
    private GameObject player;
    public float MoveSpeed;
    public bool GoFetch=false;
    public bool Fetching = false;
    Vector3 point;
    int FloorMask;//to see where the mouse is
    public GameObject NoisePrefab;//to spawn multiple noisePrefabs for effect might change for a particle system later
    public GameObject NoisePrefab1;
    public GameObject NoisePrefab2;

    void OnEnable () {
        mode = modes.Passive;
        FloorMask = LayerMask.GetMask("Floor");
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }
	
	// switches what mode Dog AI is in
	void Update () {
        if (GoFetch)//to communicate with player when they click left mouse
        {
            mode = modes.Fetch;
        }
        switch (mode)
        {
            case modes.Passive:
                DogPassive();
                break;
            case modes.Fetch:
                DogFetch();
                break;
            case modes.Stay:
                DogStay();
                break;
  
            default:
            break;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            mode = modes.Passive;
            DogBark();
        }
        
    }
    //DogPassive is used when the dog is in its default state following the player
    void DogPassive()
    {
        //finds distance between player and dog
        float distToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if(distToPlayer >= 2f)
        {
            agent.speed = MoveSpeed;
            agent.SetDestination(player.transform.position);
            
        }
        else
        {
            agent.speed = MoveSpeed;
            agent.SetDestination(gameObject.transform.localPosition);
        }
    }
    void DogFetch()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit TargetHit;
        
        if (Physics.Raycast(camRay, out TargetHit, 100f, FloorMask))
        {
            if (Fetching)
            {
                point = TargetHit.point;
                Fetching = false;
            }
        }

        try
        {
         //make the dog go to where the mouse is
            if (Vector3.Distance(gameObject.transform.position, point) <= 2f)//if it reaches destination
            {
                if (Input.GetAxisRaw("Fire2") == 1)
                {
                    GoFetch = false;
                    mode = modes.Passive;
                }
                
            }
            else
            {
                if (Input.GetAxisRaw("Fire2") == 1)
                {
                    GoFetch = false;
                    mode = modes.Passive;
                }
                else
                {
                    
                    agent.SetDestination(point);
                }
            }
        }
        catch
        {

        }

        

    }
    void DogStay()
    {

    }
    void DogBark()
    {
        Instantiate(NoisePrefab,gameObject.transform);
        Instantiate(NoisePrefab1, gameObject.transform);
        Instantiate(NoisePrefab2, gameObject.transform);
    }
}
