using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Barrels : MonoBehaviour, IInputClickHandler {

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnInputClicked(InputClickedEventData eventData)
    {
        var rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.useGravity = true;
    }
}
