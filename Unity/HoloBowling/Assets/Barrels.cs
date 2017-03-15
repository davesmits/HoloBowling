﻿using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Barrels : MonoBehaviour, IInputClickHandler {

    private bool _setupComplete;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (!_setupComplete)
        {
            var colliders = gameObject.GetComponentsInChildren<Collider>();
            foreach (var collider in colliders)
            {
                var rigidbody = collider.gameObject.AddComponent<Rigidbody>();
                rigidbody.useGravity = true;
            }

            _setupComplete = true;
        }
    }
}
