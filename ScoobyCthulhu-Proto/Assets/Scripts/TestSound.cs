using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSound : MonoBehaviour {
    //This is a test script in order to ensure the functionality of the AI's ability to hear things, lining up with how Connor plans to do sound (Hopefully)
    //Some of this can probably be reused for that exact purpose

    public float SoundRadius;
    public bool test; //if test is true, a wireframe sound bubble will be shown.
    bool gizmoShow=false; //if true, show gizmo for a few seconds
    
    //The Devil
    void Update()
    {
        if (Input.GetKeyDown("x"))
        {
            if (test) gizmoShow = true;
            Collider[] SphereHit = Physics.OverlapSphere(GetComponent<Transform>().position, SoundRadius);
            foreach(Collider c in SphereHit)
            {
                if (c.tag == "Monster")
                {
                    c.GetComponent<AIBrain>().EnterAlert(GetComponent<Transform>().position);
                }
            }
        }
    }

    //draw a sphere to display how far the sound reaches; testing, this is system-heavy if used wrong.
    void OnDrawGizmos()
    {
        if (gizmoShow)
        {
            Debug.Log("Pressed X");
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(GetComponent<Transform>().position, SoundRadius);
            Invoke("gizmoHide", 2);
        }
    }
    void gizmoHide()
    {
        gizmoShow = false;
    }
}
