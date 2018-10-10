using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArm : MonoBehaviour {
    public Vector3 Target;
    public GameObject oilProjPrefab;
	void Start () {
		
	}
	
	
	public void Shoot()
    {
        
       GameObject oilShot = Instantiate(oilProjPrefab,transform.position,transform.rotation);
        oilShot.GetComponent<ThrowObj>().Target = Target;//sends info from the player arm to the obj
    }
}
