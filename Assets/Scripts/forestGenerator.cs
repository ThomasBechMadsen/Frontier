using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forestGenerator : MonoBehaviour {

    public GameObject tree;
    public Terrain terrain;
    public GameObject forest;
    public int amount;

	// Use this for initialization
	void Start () {
		for(int i = 0; i < amount; i++)
        {
            float x = Random.Range(-terrain.terrainData.size.x / 2, terrain.terrainData.size.x/2);
            float z = Random.Range(-terrain.terrainData.size.z / 2, terrain.terrainData.size.z / 2);
            float y = terrain.SampleHeight(new Vector3(x,0,z));
            float rz = Random.Range(0, 360);
            Instantiate(tree, new Vector3(x, y, z), Quaternion.AngleAxis(rz, Vector3.up), forest.transform);
        }
	}
}
