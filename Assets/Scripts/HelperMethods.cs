using UnityEngine;

public static class HelperMethods
{
    
    public static Mesh CombineMeshesFrom(GameObject source)
    {
        MeshFilter[] meshFilters = source.GetComponentsInChildren<MeshFilter>();
        if (meshFilters.Length == 0) return null;

        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        for (int i = 0; i < meshFilters.Length; i++)
        {
            if (meshFilters[i].sharedMesh == null) continue;

            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }

        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combine, true, true);
        return combinedMesh;
    }
    
    public static GameObject CreateObjectWithMesh(Mesh mesh, Vector3 position, Quaternion rotation, Transform parent, GameObject source)
    {
        GameObject newObj = new GameObject("CombinedObject");
        newObj.transform.SetPositionAndRotation(position, rotation);
        if (parent != null)
            newObj.transform.SetParent(parent);

        MeshFilter mf = newObj.AddComponent<MeshFilter>();
        mf.sharedMesh = mesh;

        MeshRenderer mr = newObj.AddComponent<MeshRenderer>();
        var sourceRenderer = source.GetComponentInChildren<Renderer>();
        if (sourceRenderer != null)
            mr.sharedMaterial = sourceRenderer.sharedMaterial;

        return newObj;
    }

    public static void ScaleAndCenterObject(GameObject obj, Vector3 targetSize, Vector3 targetPosition)
    {
        MeshRenderer mr = obj.GetComponent<MeshRenderer>();
        if (mr == null) return;

        Vector3 currentSize = mr.bounds.size;
        float scaleX = targetSize.x / currentSize.x;
        float scaleY = targetSize.y / currentSize.y;
        float scaleZ = targetSize.z / currentSize.z;
        float uniformScale = Mathf.Min(scaleX, scaleY, scaleZ);

        obj.transform.localScale = Vector3.one * uniformScale;

        Vector3 adjustedCenter = mr.bounds.center;
        Vector3 offset = targetPosition - adjustedCenter;
        obj.transform.position += offset;
    }


}
