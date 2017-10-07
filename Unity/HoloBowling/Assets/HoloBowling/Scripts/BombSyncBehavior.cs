using HoloToolkit.Sharing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DefaultSyncModelAccessor))]
[RequireComponent(typeof(Rigidbody))]
public class BombSyncBehavior : MonoBehaviour
{

    private DefaultSyncModelAccessor syncModelAccessor;
    private bool bombStarted = false;

    private void Awake()
    {
        syncModelAccessor = GetComponent<DefaultSyncModelAccessor>();

        
    }

    private void Update()
    {
        var model = (BombModel)syncModelAccessor.SyncModel;
        if (model != null && !bombStarted)
        {
            bombStarted = true;

            var rigidBody = GetComponent<Rigidbody>();
            rigidBody.AddForce(model.Force.Value, ForceMode.VelocityChange);
        }
    }
}
