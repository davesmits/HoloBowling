using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBomb : MonoBehaviour, IInputClickHandler {

    public GameObject Bomb;

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
        // Create a clone of the Bomb Prefab
        var bomb = Instantiate(Bomb);
        bomb.transform.position = transform.position;

        // Add a Rigid Body component to apply gravity and forces
        var rigidBody = bomb.GetComponent<Rigidbody>();
        rigidBody.AddForce(this.transform.rotation * (Vector3.forward * 30), ForceMode.VelocityChange);
    }

}
