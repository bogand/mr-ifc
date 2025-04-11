using GLTFast.Schema;
using UnityEditor;
using UnityEngine;

public class GetAssetPath : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject obj;
    void Start()
    {
        Debug.Log(AssetDatabase.GetAssetPath(obj));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
