using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArm : MonoBehaviour {

    public GameObject oilProjPrefab;
	void Start () {
		
	}
	
	
	public void Shoot()
    {
        
        Instantiate(oilProjPrefab,transform.position,oilProjPrefab.transform.rotation);
    }
}
