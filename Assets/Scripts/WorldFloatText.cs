using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldFloatText : MonoBehaviour {

    public Transform camera;

    void Start()
    {
        camera = GameObject.Find("Main Camera").transform;
    }

    // Update is called once per frame
    void LateUpdate () {
        transform.position = transform.parent.position + Vector3.up;
        transform.LookAt(2 * transform.position - camera.position);
	}
}
