using System;
using UnityEngine;

public class Model : MonoBehaviour
{
    public event EventHandler<GameObject> OnObjectCreated;
    public event EventHandler<GameObject> OnObjectDeleted;
    
    void Start()
    {
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
}
