using HoloToolkit.Sharing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DefaultSyncModelAccessor))]
[RequireComponent(typeof(Rigidbody))]
public class BombSyncBehavior : MonoBehaviour
{

    private DefaultSyncModelAccessor syncModelAccessor;

    private void Awake()
    {
        syncModelAccessor = GetComponent<DefaultSyncModelAccessor>();

        var model = (BombModel)syncModelAccessor.SyncModel;
        var rigidBody = GetComponent<Rigidbody>();
        rigidBody.AddForce(model.Force.Value, ForceMode.VelocityChange);
    }
}
