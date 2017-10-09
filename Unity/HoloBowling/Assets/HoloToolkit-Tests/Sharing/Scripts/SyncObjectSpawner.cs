//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
//

using UnityEngine;
using HoloToolkit.Sharing.Spawning;
using HoloToolkit.Unity.InputModule;
using System;
using System.Linq;

namespace HoloToolkit.Sharing.Tests
{
    /// <summary>
    /// Class that handles spawning sync objects on keyboard presses, for the SpawningTest scene.
    /// </summary>
    public class SyncObjectSpawner : MonoBehaviour
    {
        [SerializeField]
        private PrefabSpawnManager spawnManager;

        [SerializeField]
        [Tooltip("Optional transform target, for when you want to spawn the object on a specific parent.  If this value is not set, then the spawned objects will be spawned on this game object.")]
        private Transform spawnParentTransform;

        private void Awake()
        {
            if (spawnManager == null)
            {
                Debug.LogError("You need to reference the spawn manager on SyncObjectSpawner.");
            }

            // If we don't have a spawn parent transform, then spawn the object on this transform.
            if (spawnParentTransform == null)
            {
                spawnParentTransform = transform;
            }
        }

        public PlayfieldModel SpawnPlayfield(Vector3 position, Quaternion rotation)
        {
            var spawnedObject = new PlayfieldModel();
            spawnManager.Spawn(spawnedObject, position, rotation, spawnParentTransform.gameObject, "PlayfieldModel", false);
            return spawnedObject;
        }

        public void SpawnBomb(Vector3 position, Quaternion rotation, Vector3 force)
        {
            var spawnedObject = new BombModel();
            spawnedObject.Force.Value = force;

            spawnManager.Spawn(spawnedObject, position, rotation, spawnParentTransform.gameObject, "BombModel", false);
        }

        public PlayfieldModel FindPlayfield()
        {
            var instance = SharingStage.Instance.Root.InstantiatedPrefabs;

            return instance.OfType<PlayfieldModel>().FirstOrDefault();
        }

        public void DeleteSyncObject(SyncSpawnedObject syncSpawnObject)
        {
            spawnManager.Delete(syncSpawnObject);
        }

        /// <summary>
        /// Deletes any sync object that inherits from SyncSpawnObject.
        /// </summary>
        public void DeleteSyncObject()
        {
            GameObject hitObject = GazeManager.Instance.HitObject;
            if (hitObject != null)
            {
                var syncModelAccessor = hitObject.GetComponent<DefaultSyncModelAccessor>();
                if (syncModelAccessor != null)
                {
                    var syncSpawnObject = (SyncSpawnedObject)syncModelAccessor.SyncModel;
                    DeleteSyncObject(syncSpawnObject);
                }
            }
        }
    }
}
