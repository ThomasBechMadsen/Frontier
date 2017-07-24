using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour {

    public float fullHealth;
    float health;
    public GameObject treeChunk;

    void Start()
    {
        health = fullHealth;
    }

    public void chop(float damage, Vector3 playerDirectionRight)
    {
        health -= damage;
        if (health <= 0)
        {

            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            health = fullHealth;
        }
    }
}
