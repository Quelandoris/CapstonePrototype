using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {

    public GameObject winPanel;
    public GameObject Fireplace; //The unlit fireplace
    public GameObject fireplaceTrap; //The trap version of the fireplace
    public int LitTime; //Time the fireplace stays lit after the oil hits it (Seconds)

    private void OnEnable()
    {
        Invoke("Disable", LitTime);
    }

    // Use this for initialization
    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Monster")
        {
            //Kill the monster
            coll.gameObject.SetActive(false);
            //Turn on the win panel
            winPanel.SetActive(true);

        }
    }
    void Disable()
    {
        Fireplace.SetActive(true);
        fireplaceTrap.SetActive(false);
    }
}
