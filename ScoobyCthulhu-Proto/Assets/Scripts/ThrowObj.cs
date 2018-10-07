using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObj : MonoBehaviour {

    public float thrust = 50f;
    Transform transform;
    Rigidbody rb;
    public GameObject OilSpill;
	void Awake () {
        rb = GetComponent<Rigidbody>();
        transform = GetComponent<Transform>();
        rb.AddForce(transform.up * thrust);

	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor")){
            Instantiate(OilSpill,transform);
        }
    }
}
