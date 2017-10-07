using HoloToolkit.Sharing;
using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DefaultSyncModelAccessor))]
public class PlayfieldSyncBehavior : MonoBehaviour
{
    private DefaultSyncModelAccessor _syncModel;

    // Use this for initialization
    void Start()
    {
        _syncModel = GetComponent<DefaultSyncModelAccessor>();
    }

    // Update is called once per frame
    void Update()
    {
        var model = _syncModel.SyncModel as PlayfieldModel;
        if (model != null && model.GameStarted.Value)
        {
            GetComponent<TapToPlace>().enabled = false;

            //Add a Rigid Body to all barrels
            var colliders = this.GetComponentsInChildren<Collider>();
            foreach (var collider in colliders)
            {
                var rigidbody = collider.gameObject.AddComponent<Rigidbody>();
                rigidbody.useGravity = true;
            }
        }

    }

    void LateUpdate()
    {
        var model = _syncModel.SyncModel as PlayfieldModel;
        if (model != null)
        {
            model.IsBeingPlaced.Value = GetComponent<TapToPlace>().IsBeingPlaced;
        }
    }
}
