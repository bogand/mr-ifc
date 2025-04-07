using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectContainer : MonoBehaviour
{
    public static ObjectContainer Instance {get; private set;}
    public event EventHandler<List<GameObject>> OnListChanged;
   
    [SerializeField] private List<GameObject> loadedObjects = new List<GameObject>();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void AddObject(GameObject obj)
    {
        loadedObjects.Add(obj);
        OnListChanged?.Invoke(this, loadedObjects);
        Debug.Log($"Object added: {obj.name}");
    }

    private void RemoveObject(GameObject obj)
    {
        loadedObjects.Remove(obj);
        if (obj.transform.parent != null && obj.transform.parent.name == "GLTF_Model")
        {
            Destroy(obj.transform.parent.gameObject);
        }
        else
        {
            Destroy(obj.transform.gameObject);
        }
        OnListChanged?.Invoke(this, loadedObjects);
        Debug.Log($"Object removed: {obj.name}");
    }

    public void OnObjectDeleted(object sender,GameObject obj)
    {
        RemoveObject(obj);
    }

    public void OnObjectCreated(object sender, GameObject obj)
    {
        AddObject(obj);
    }

    public List<GameObject> GetAllObjectsList()
    {
        return loadedObjects;
    }

    public GameObject GetSingleObject(GameObject obj)
    {
        return obj;
    }
}

