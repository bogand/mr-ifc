using UnityEngine;

public static class GetObjectSize
{
    
    public static void GetSize(GameObject obj)
    {
        var size = obj.GetComponentInChildren<MeshRenderer>().bounds.size;
        Debug.Log(size);
    }
}
