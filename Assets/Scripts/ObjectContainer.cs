using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectContainer : MonoBehaviour
{
    public static ObjectContainer Instance {get; private set;}
   
    [SerializeField] private List<GameObject> loadedObjects = new List<GameObject>();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject); // Opcjonalnie, jeśli chcesz, żeby singleton przetrwał zmianę sceny
    }
    
    public void AddObject(GameObject obj)
    {
        loadedObjects.Add(obj);
        Debug.Log($"Object added: {obj.name}");
    }

    public void RemoveObject(GameObject obj)
    {
        loadedObjects.Remove(obj);
        Destroy(transform.parent.gameObject);
        Debug.Log($"Object removed: {obj.name}");
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

