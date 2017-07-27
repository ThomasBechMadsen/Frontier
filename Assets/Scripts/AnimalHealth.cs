using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalHealth : MonoBehaviour {

    public float health;
    public GameObject corpse;

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
        if (health > 0)
        {
            return true;
        }
        else
        {
            GameObject loot = Instantiate(corpse, transform.position, transform.rotation);
            Destroy(gameObject);
            //Time.timeScale = 0;
            return false;
        }
    }
}
