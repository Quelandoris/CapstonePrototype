using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogMovement : MonoBehaviour {

    public Transform NavTarget;
    NavMeshAgent agent;
    public enum modes { Passive, Fetch, Stay, Bark };
    public modes mode;
    private GameObject player;
    public float MoveSpeed;

    void OnEnable () {
        mode = modes.Passive;
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }
	
	// switches what mode Dog AI is in
	void Update () {
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
            case modes.Bark:
                DogBark();
                break;
            default:
                Debug.Log("Mode Switch in AIBrain Update broke somehow.");
                break;
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

    }
    void DogStay()
    {

    }
    void DogBark()
    {

    }
}
