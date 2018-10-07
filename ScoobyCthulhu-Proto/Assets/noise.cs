using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noise : MonoBehaviour {

    Transform transform;
    public float GrowthRate=2f;
    public float time = 5f;
    public float WaitTime = 0f;//amount of extra time it takes for it to appear
    private void Awake()
    {
        transform = GetComponent<Transform>();
    }
    void Update () {
        if (WaitTime <= 0)
        {
            time -= Time.deltaTime;
        }
        WaitTime -= Time.deltaTime;
        
        if (time >= 0)
        {
            if (WaitTime <= 0)
            {
                transform.localScale += new Vector3(GrowthRate, GrowthRate, GrowthRate) * Time.deltaTime;
            }
        }
        else
        {
            Destroy(gameObject);
        }
	}
}
