using HoloToolkit.Sharing;
using HoloToolkit.Sharing.Tests;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoloBowlingSceneManager : Singleton<HoloBowlingSceneManager>
{

    private bool _canvasVisible;
    private bool _gameStarted;

    public GameObject Canvas;
    public GameObject StartExperienceCanvas;


    public ThrowBomb ThrowBomb;


    public PlayfieldModel Playfield;
    public SyncObjectSpawner ObjectSpawner;

    public bool ExperienceStarted;

    // Use this for initialization
    void Start()
    {
        Canvas.SetActive(false);
        ThrowBomb.enabled = false;

        if (SharingStage.Instance.IsConnected)
        {
            StartExperienceCanvas.SetActive(true);
        }
        else
        {
            StartExperienceCanvas.SetActive(false);
            SharingStage.Instance.SharingManagerConnected += Instance_SharingManagerConnected;
        }
    }

    private void Instance_SharingManagerConnected(object sender, System.EventArgs e)
    {
        SharingStage.Instance.SharingManagerConnected -= Instance_SharingManagerConnected;
        StartExperienceCanvas.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_gameStarted && Playfield != null)
        {
            // Check if the TapToPlace was finished, if so, show canvas to start the game
            if (Playfield.IsBeingPlaced.Value && _canvasVisible)
            {
                Canvas.SetActive(false);
                _canvasVisible = false;
            }
            else if (!Playfield.IsBeingPlaced.Value && !_canvasVisible)
            {
                Canvas.SetActive(true);
                _canvasVisible = true;
            }
        }
    }

    public void TrySetupGame()
    {
        Playfield = ObjectSpawner.FindPlayfield();
        if (Playfield == null)
        {
            WorldAnchorManager.Instance.AnchorDebugText.text += "\nTry setup game, no playfield";
            SetupGame();
        }
        else
        {
            WorldAnchorManager.Instance.AnchorDebugText.text += "\nTry setup game, with playfield";
            ThrowBomb.enabled = true;
        }

        ExperienceStarted = true;
        Destroy(StartExperienceCanvas);
    }

    // Setup the Barrels with Tap To Place and disable other features
    public void SetupGame()
    {
        // Disable throwing bombs
        ThrowBomb.enabled = false;

        // If we reset the game, destroy the previous Barrels
        if (Playfield != null)
        {
            ObjectSpawner.DeleteSyncObject(Playfield);
        }

        Playfield = ObjectSpawner.SpawnPlayfield(Vector3.zero, Quaternion.identity);
        Playfield.MovedBy.Value = SharingStage.Instance.UserName;
        Playfield.IsBeingPlaced.Value = true;

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

        Playfield.MovedBy.Value = string.Empty;
        Playfield.IsBeingPlaced.Value = false;

        Playfield.GameStarted.Value = true;
        //TapToPlaceObject.enabled = false;

        // Enable Throwing Bombs after setup has completed
        ThrowBomb.enabled = true;


    }
}
