using HoloToolkit.Sharing;
using HoloToolkit.Sharing.SyncModel;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DefaultSyncModelAccessor))]
public class PlayfieldSyncBehavior : MonoBehaviour
{
    private DefaultSyncModelAccessor _syncModel;
    private bool gameStarted;

    protected Vector3Interpolated Position;
    protected QuaternionInterpolated Rotation;
    protected Vector3Interpolated Scale;

    private SyncTransform transformDataModel;

    private bool moving = true;

    /// <summary>
    /// Data model to which this transform synchronization is tied to.
    /// </summary>
    public SyncTransform TransformDataModel
    {
        private get { return transformDataModel; }
        set
        {
            if (transformDataModel != value)
            {
                if (transformDataModel != null)
                {
                    transformDataModel.PositionChanged -= OnPositionChanged;
                    transformDataModel.RotationChanged -= OnRotationChanged;
                    transformDataModel.ScaleChanged -= OnScaleChanged;
                }

                transformDataModel = value;

                if (transformDataModel != null)
                {
                    // Set the position, rotation and scale to what they should be
                    transform.localPosition = transformDataModel.Position.Value;
                    transform.localRotation = transformDataModel.Rotation.Value;
                    transform.localScale = transformDataModel.Scale.Value;

                    // Initialize
                    Initialize();

                    // Register to changes
                    transformDataModel.PositionChanged += OnPositionChanged;
                    transformDataModel.RotationChanged += OnRotationChanged;
                    transformDataModel.ScaleChanged += OnScaleChanged;
                }
            }
        }
    }

    private void Start()
    {
        _syncModel = GetComponent<DefaultSyncModelAccessor>();
        Initialize();
    }

    private void Initialize()
    {
        if (Position == null)
        {
            Position = new Vector3Interpolated(transform.localPosition);
        }
        if (Rotation == null)
        {
            Rotation = new QuaternionInterpolated(transform.localRotation);
        }
        if (Scale == null)
        {
            Scale = new Vector3Interpolated(transform.localScale);
        }
    }

    private void Update()
    {
        var model = _syncModel.SyncModel as PlayfieldModel;
        if (model != null)
        {
            TransformDataModel = model.Transform;

            if (string.Equals(model.MovedBy.Value, SharingStage.Instance.UserName) && !moving)
            {
                moving = true;
                GetComponent<TapToPlace>().IsBeingPlaced = true;
            }
            else if (!string.Equals(model.MovedBy.Value, SharingStage.Instance.UserName) && moving)
            {
                moving = false;
                GetComponent<TapToPlace>().IsBeingPlaced = false;
            }



            if (model.GameStarted.Value != gameStarted)
            {
                gameStarted = model.GameStarted.Value;

                // Dont allow to relocate everything again
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

        // Apply transform changes, if any
        if (Position.HasUpdate())
        {
            transform.localPosition = Position.GetUpdate(Time.deltaTime);
        }
        if (Rotation.HasUpdate())
        {
            transform.localRotation = Rotation.GetUpdate(Time.deltaTime);
        }
        if (Scale.HasUpdate())
        {
            transform.localScale = Scale.GetUpdate(Time.deltaTime);
        }
    }

    private void LateUpdate()
    {
        // Determine if the transform has changed locally, in which case we need to update the data model
        if (transform.localPosition != Position.Value ||
            Quaternion.Angle(transform.localRotation, Rotation.Value) > 0.2f ||
            transform.localScale != Scale.Value)
        {
            transformDataModel.Position.Value = transform.localPosition;
            transformDataModel.Rotation.Value = transform.localRotation;
            transformDataModel.Scale.Value = transform.localScale;

            // The object was moved locally, so reset the target positions to the current position
            Position.Reset(transform.localPosition);
            Rotation.Reset(transform.localRotation);
            Scale.Reset(transform.localScale);
        }

        var model = _syncModel.SyncModel as PlayfieldModel;
        if (model != null)
        {
            model.IsBeingPlaced.Value = GetComponent<TapToPlace>().IsBeingPlaced;
        }
    }

    private void OnDestroy()
    {
        if (transformDataModel != null)
        {
            transformDataModel.PositionChanged -= OnPositionChanged;
            transformDataModel.RotationChanged -= OnRotationChanged;
            transformDataModel.ScaleChanged -= OnScaleChanged;
        }
    }

    private void OnPositionChanged()
    {
        Position.SetTarget(transformDataModel.Position.Value);
    }

    private void OnRotationChanged()
    {
        Rotation.SetTarget(transformDataModel.Rotation.Value);
    }

    private void OnScaleChanged()
    {
        Scale.SetTarget(transformDataModel.Scale.Value);
    }
}
