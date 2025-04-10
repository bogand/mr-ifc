using System;
using MixedReality.Toolkit.SpatialManipulation;
using UnityEngine;

public class Model : MonoBehaviour
{
    public event EventHandler<GameObject> OnObjectCreated;
    public event EventHandler<GameObject> OnObjectDeleted;
    private static readonly string _pathToBoundsVisualsPrefab = "Assets/Prefabs/Spatial Manipulation/BoundingBoxWithHandles.prefab";
    private GameObject boundsVisualPrefab;

    void Awake()
    {
        boundsVisualPrefab = (GameObject)Resources.Load(_pathToBoundsVisualsPrefab);
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
        if (!this.gameObject.GetComponent<ObjectManipulator>())
        {
            this.gameObject.AddComponent<ObjectManipulator>();
        }

        if (!this.gameObject.GetComponent<MeshCollider>())
        {
            this.gameObject.AddComponent<MeshCollider>();
        }

        if (!this.gameObject.GetComponent<BoundsControl>())
        {
            var boundsControl = this.gameObject.AddComponent<BoundsControl>();
            boundsControl.BoundsVisualsPrefab = boundsVisualPrefab;
            boundsControl.ConstraintsManager = this.GetComponent<ConstraintManager>();
        }
    }
}
