using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalHealth : MonoBehaviour {

    public float health;
    public GameObject pickupable;
    public Item itemYield;

    public void dealDamage(float damage)
    {
        health -= damage;
        GetComponent<DeerAI>().setState(DeerState.Scared);
        if (health <= 0)
        {
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<DeerAI>().enabled = false;
            GetComponent<DeerAI>().isDead = true;
            GameObject g = Instantiate(pickupable, transform);
            g.GetComponent<pickupScript>().itemYield = itemYield;
            tag = "Pickupable";
        }
    }
}
