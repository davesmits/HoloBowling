using HoloToolkit.Unity;
using HoloToolkit.Unity.SpatialMapping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloBowling
{
    public class SceneManager : Singleton<SceneManager>
    {

        private bool _canvasVisible;
        private bool _gameStarted;

        public GameObject Canvas;
        public TapToPlace TapToPlaceObject;
        public ThrowBomb ThrowBomb;

        public GameObject BarrelsPrefab;

        // Use this for initialization
        void Start()
        {
            SetupGame();
        }

        // Update is called once per frame
        void Update()
        {
            if (!_gameStarted)
            {
                // Check if the TapToPlace was finished, if so, show canvas to start the game
                if (TapToPlaceObject.IsBeingPlaced && _canvasVisible)
                {
                    Canvas.SetActive(false);
                    _canvasVisible = false;
                }
                else if (!TapToPlaceObject.IsBeingPlaced && !_canvasVisible)
                {
                    Canvas.SetActive(true);
                    _canvasVisible = true;
                }
            }
        }

        // Setup the Barrels with Tap To Place and disable other features
        public void SetupGame()
        {
            // Disable throwing bombs
            ThrowBomb.enabled = false;

            // Set the new TapToPlace object and activate it
            if (BarrelsPrefab != null)
            {
                // If we reset the game, destroy the previous Barrels
                if (TapToPlaceObject != null)
                {
                    Destroy(TapToPlaceObject.gameObject);
                }

                var newBarrals = Instantiate(BarrelsPrefab);
                TapToPlaceObject = newBarrals.GetComponent<TapToPlace>();
            }

            TapToPlaceObject.IsBeingPlaced = true;

            // Ensure Canvas is hidden
            if (Canvas != null)
            {
                Canvas.SetActive(false);
            }

            _gameStarted = false;
        }

        // Called from the Canvas to start the game
        public void StartGame()
        {
            // Hide setup features
            _gameStarted = true;
            Canvas.SetActive(false);
            TapToPlaceObject.enabled = false;

            // Enable Throwing Bombs after setup has completed
            ThrowBomb.enabled = true;

            // Add a Rigid Body to all barrels
            var colliders = TapToPlaceObject.GetComponentsInChildren<Collider>();
            foreach (var collider in colliders)
            {
                var rigidbody = collider.gameObject.AddComponent<Rigidbody>();
                rigidbody.useGravity = true;
            }
        }
    }
}
