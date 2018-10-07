using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    
    public Animator anim;
    
    public GameObject DogPosition;
    public GameObject InvPos1;
    public GameObject Invpos2;
    //public PotionGun gun;
    public int position = 1;
    void Start () {
        anim = GetComponent<Animator>();
	}
	
	
	void Update () {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (position == 1 )
            {
                anim.SetBool("switchLeft", true);
                position = 0;
            }
            if( position == 2)
            {
                anim.SetBool("switchRight", false);
                position = 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (position == 1)
            {
                anim.SetBool("switchRight", true);
                position = 2;
            }
            if (position == 0)
            {
                anim.SetBool("switchLeft", false);
                position = 1;
            }
            

        }
        if(position == 1)
        {
           // gun.CurrPotion = Ice;
        }
        if(position == 0)
        {
          //  gun.CurrPotion = Poison;
        }
        if(position == 2)
        {
           // gun.CurrPotion = Health;
        }
        
    }
}
