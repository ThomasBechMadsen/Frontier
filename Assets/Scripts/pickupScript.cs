using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupScript : MonoBehaviour {

    public ItemData item;
    private GameObject text;

    void Start()
    {
        text = transform.GetChild(0).gameObject;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameObject.Find("Player"))
        {
            //"Press E to pickup" - show
            text.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameObject.Find("Player"))
        {
            //"Press E to pickup" - hide
            text.SetActive(false);
        }
    }

}
