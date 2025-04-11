using System;
using MixedReality.Toolkit;
using MixedReality.Toolkit.SpatialManipulation;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.XR.ARFoundation;
using Axis = UnityEngine.Animations.Axis;

public class Model : MonoBehaviour
{
    public event EventHandler<GameObject> OnObjectCreated;
    public event EventHandler<GameObject> OnObjectDeleted;
    private static readonly string _pathToBoundsVisualsPrefab = "Assets/Prefabs/Spatial Manipulation/BoundingBoxWithHandles.prefab";
    private GameObject boundsVisualPrefab;

    void Awake()
    {
        boundsVisualPrefab = AssetDatabase.LoadAssetAtPath(_pathToBoundsVisualsPrefab, typeof(GameObject)) as GameObject;
        Debug.Log(boundsVisualPrefab);
    }
    void Start()
    {
        AddComponents();
        this.OnObjectCreated += ObjectContainer.Instance.OnObjectCreated;
        this.OnObjectDeleted += ObjectContainer.Instance.OnObjectDeleted;
        OnObjectCreated?.Invoke(this, gameObject);
    }
    
    void OnDestroy()
    {
        OnObjectDeleted?.Invoke(this, gameObject);
        this.OnObjectCreated -= ObjectContainer.Instance.OnObjectCreated;
        this.OnObjectDeleted -= ObjectContainer.Instance.OnObjectDeleted;
    }

    void AddComponents()
    {
        if (!this.gameObject.GetComponent<MeshCollider>())
        {
            this.gameObject.AddComponent<MeshCollider>();
        }
        
        if (!this.gameObject.GetComponent<ObjectManipulator>())
        {
            this.gameObject.AddComponent<ObjectManipulator>();
        }
        
        if (!this.gameObject.GetComponent<ARAnchor>())
        {
            this.gameObject.AddComponent<ARAnchor>();
        }

        if (!this.gameObject.GetComponent<BoundsControl>())
        {
            var boundsControl = this.gameObject.AddComponent<BoundsControl>();
            boundsControl.BoundsVisualsPrefab = boundsVisualPrefab;
            boundsControl.ConstraintsManager = this.GetComponent<ConstraintManager>();
        }

        if (!this.gameObject.GetComponent<MinMaxScaleConstraint>())
        {
            this.gameObject.AddComponent<MinMaxScaleConstraint>();
            this.gameObject.GetComponent<MinMaxScaleConstraint>().enabled = false;
        }
        
        if (!this.gameObject.GetComponent<RotationAxisConstraint>())
        {
            this.gameObject.AddComponent<RotationAxisConstraint>();
            this.gameObject.AddComponent<RotationAxisConstraint>().ConstraintOnRotation = AxisFlags.XAxis + (int)AxisFlags.YAxis;
            this.gameObject.GetComponent<RotationAxisConstraint>().enabled = false;
        }
        
        if (!this.gameObject.GetComponent<MoveAxisConstraint>())
        {
            this.gameObject.AddComponent<MoveAxisConstraint>();
            this.gameObject.GetComponent<MoveAxisConstraint>().enabled = false;
        }
    }

    public void ApplyScaleConstraint()
    {
        bool active = this.gameObject.GetComponent<MinMaxScaleConstraint>().enabled;
        this.gameObject.GetComponent<MinMaxScaleConstraint>().enabled = !active;
    }
    
    public void ApplyRotationConstraint()
    {
        bool active = this.gameObject.GetComponent<RotationAxisConstraint>().enabled;
        this.gameObject.GetComponent<RotationAxisConstraint>().enabled = !active;
    }
    
    public void ApplyMoveConstraint()
    {
        bool active = this.gameObject.GetComponent<MoveAxisConstraint>().enabled;
        this.gameObject.GetComponent<MoveAxisConstraint>().enabled = !active;
    }
}
