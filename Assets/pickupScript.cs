using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupScript : MonoBehaviour {

    public Item itemYield;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameObject.Find("Player"))
        {
            //"Press E to pickup" - show
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameObject.Find("Player"))
        {
            //"Press E to pickup" - hide
        }
    }

}
