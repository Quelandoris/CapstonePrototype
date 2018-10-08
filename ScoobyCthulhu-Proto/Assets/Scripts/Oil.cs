using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oil : MonoBehaviour {

    Transform transform;
    public float GrowthRate = 2f;
    public float time = .5f;
    void Awake () {
        transform = GetComponent<Transform>();
    }
	
	
	void Update () {
        time -= Time.deltaTime;
        if (time >= 0)
        {
           
                transform.localScale += new Vector3(GrowthRate, 0f, GrowthRate) * Time.deltaTime;
            
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
