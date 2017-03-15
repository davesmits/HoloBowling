using HoloToolkit.Unity;
using HoloToolkit.Unity.SpatialMapping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : Singleton<SceneManager> {

    private bool _canvasVisible;
    private bool _gameStarted;

    public GameObject Canvas;
    public TapToPlace TapToPlaceObject;
    public ThrowBomb ThrowBomb;

    public GameObject Barrels;

    // Use this for initialization
    void Start () {
        CreateBarrels();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_gameStarted)
        {
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

    public void StartGame()
    {
        _gameStarted = true;
        Canvas.SetActive(false);
        TapToPlaceObject.enabled = false;
        ThrowBomb.enabled = true;


        var colliders = TapToPlaceObject.GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
        {
            var rigidbody = collider.gameObject.AddComponent<Rigidbody>();
            rigidbody.useGravity = true;
        }
    }

    public void CreateBarrels()
    {
        ThrowBomb.enabled = false;
        if (TapToPlaceObject != null)
        {
            Destroy(TapToPlaceObject.gameObject);
        }

        var newBarrals = Instantiate(Barrels);
        TapToPlaceObject = newBarrals.GetComponent<TapToPlace>();

        Canvas.SetActive(false);
        TapToPlaceObject.IsBeingPlaced = true;
        _gameStarted = false;
    }
}
