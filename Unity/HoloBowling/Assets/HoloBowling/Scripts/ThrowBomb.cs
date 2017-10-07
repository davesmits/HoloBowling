using HoloToolkit.Sharing.Tests;
using HoloToolkit.Unity.InputModule;
using System;
using System.Collections;
using UnityEngine;

public class ThrowBomb : MonoBehaviour, IInputClickHandler {

    public SyncObjectSpawner Bomb;

    // Use this for initialization
    void Start()
    {
        // Add global listener for AirTap / Click events
        InputManager.Instance.AddGlobalListener(gameObject);
    }

    // Clean up
    void OnDestroy()
    {
        // Remove global listener for AirTap / Click events
        InputManager.Instance.RemoveGlobalListener(gameObject);
    }

    // Implementation for IInputClickHandler
    public void OnInputClicked(InputClickedEventData eventData)
    {
        var force = this.transform.rotation * (Vector3.forward * 30);
        Bomb.SpawnBomb(transform.position, Quaternion.identity, force);
    }

    

}
