﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {
    public GameObject mainPlayer;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButton(2))
        {
            gameObject.transform.Translate(new Vector3(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y")));
        }
        if(Input.GetAxis("Mouse ScrollWheel") > 0 && Camera.main.orthographicSize > 0)
        {
            Camera.main.orthographicSize--;
            //gameObject.transform.Translate(new Vector3(0, 0, Input.GetAxis("Mouse ScrollWheel")));
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0 && Camera.main.orthographicSize < 6)
        {
            Camera.main.orthographicSize++;

        }
    }
}
