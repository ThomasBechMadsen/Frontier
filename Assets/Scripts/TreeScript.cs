using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour {

    public float fullHealth;
    float health;
    public GameObject treeChunk;
    bool fallen = false;

    void Start()
    {
        health = fullHealth;
    }

    public void chop(float damage, Vector3 playerDirectionRight)
    {
        health -= damage;
        if (!fallen && health <= fullHealth/2)
        {
            fallen = true;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
        if(fallen && health <= 0)
        {
            Instantiate(treeChunk, transform.position, transform.rotation * (Quaternion.Euler(Vector3.up * Random.Range(1,360))));
            Instantiate(treeChunk, transform.position + transform.up * 3.5f, transform.rotation * (Quaternion.Euler(Vector3.up * Random.Range(1, 360))));
            Instantiate(treeChunk, transform.position + transform.up * 6.5f, transform.rotation * (Quaternion.Euler(Vector3.up * Random.Range(1, 360))))                                                                                                                    ;
            Destroy(gameObject);
        }
    }
}
