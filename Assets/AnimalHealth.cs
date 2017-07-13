using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalHealth : MonoBehaviour {

    public float health;
    public GameObject pickupable;
    public ItemData itemYield;

    public void dealDamage(float damage)
    {
        health -= damage;
        checkAlive();
        StartCoroutine(bleed(20));
        GetComponent<DeerAI>().setState(DeerState.Scared);
    }

    private IEnumerator bleed(float bleedDamage)
    {
        while (checkAlive())
        {
            yield return new WaitForSeconds(1);
            health -= bleedDamage;
        }
    }

    private bool checkAlive()
    {
        if (health <= 0)
        {
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<DeerAI>().enabled = false;
            GetComponent<DeerAI>().isDead = true;
            this.enabled = false;
            GameObject g = Instantiate(pickupable, transform);
            g.GetComponent<pickupScript>().itemYield = itemYield;
            tag = "Pickupable";
            return false;
        }
        else
        {
            return true;
        }
    }
}
