using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThrowBomb : MonoBehaviour, IInputClickHandler
{
    public GameObject Bomb;

    void Start()
    {
        InputManager.Instance.AddGlobalListener(gameObject);
    }

    private void OnDestroy()
    {
        InputManager.Instance.AddGlobalListener(gameObject);
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        var bomb = Instantiate(Bomb);
        bomb.transform.position = transform.position;

        var rigidBody = bomb.GetComponent<Rigidbody>();
        rigidBody.AddForce(this.transform.rotation * (Vector3.forward * 50), ForceMode.Impulse);
    }
}
