using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObj : MonoBehaviour {

    public float thrust = 500f;
    Transform transform;
    Rigidbody rb;
    public GameObject OilSpill;
    public Vector3 Target;
	void Awake () {
        rb = GetComponent<Rigidbody>();
        transform = GetComponent<Transform>();
        rb.AddForce(( transform.forward + (transform.up /2)) * thrust);

	}
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Target, Time.deltaTime * 50f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor")){
            Instantiate(OilSpill,transform.position,OilSpill.transform.rotation);
            Destroy(gameObject);
        }
    }
}
