using System.Threading.Tasks;
using UnityEngine;
using GLTFast;
using MixedReality.Toolkit.SpatialManipulation;
using Unity.XR.CoreUtils;

public class GltfLoader : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void LoadObject(string path)
    {
        var gltf = new GLTFast.GltfImport();
        var success = await gltf.Load(path);
        
        if (success) {
            // Here you can customize the post-loading behavior

            // Get the first material
            var material = gltf.GetMaterial();
            Debug.LogFormat("The first material is called {0}", material.name);

            // Instantiate the glTF's main scene
            await gltf.InstantiateMainSceneAsync( new GameObject("Instance 1").transform );
            // Instantiate the glTF's main scene
            //await gltf.InstantiateMainSceneAsync( new GameObject("Instance 2").transform );

            // Instantiate each of the glTF's scenes
            for (int sceneId = 0; sceneId < gltf.SceneCount; sceneId++) {
                await gltf.InstantiateSceneAsync(transform, sceneId);
            }
            
        } else {
            Debug.LogError("Loading glTF failed!");
        }
    }
    
    public GameObject ReturnLoadObject(string path)
    {
        var gameObj = new GameObject("root");
        gameObj.SetActive(false);
        gameObj.hideFlags = HideFlags.HideInHierarchy;
        var gltf = new GltfImport();
        gltf.LoadFile(path);
        gltf.InstantiateMainSceneAsync(gameObj.transform);
        return gameObj;
    }
    
    /*public async Task<GameObject> LoadGltfToGameObject(string path)
    {
        var gltf = new GltfImport();
        bool success = await gltf.Load(path);
        
        if (!success)
        {
            Debug.LogError("GLTF load failed");
            return null;
        }
        
        GameObject root = new GameObject("GLTF_Model");
        bool instSuccess = await gltf.InstantiateMainSceneAsync(root.transform);
        GameObject model = root.transform.GetChild(0).gameObject;
        GameObject newModel = HelperMethods.CombineMeshesFrom(model); 
        model.AddComponent<Model>();
        
        if (!instSuccess)
        {
            Debug.LogError("GLTF instantiation failed");
            return null;
        }

        return root;
    }*/
    
    /*public async Task<GameObject> LoadGltfToGameObject(string path)
{
    var gltf = new GltfImport();
    bool success = await gltf.Load(path);
    
    if (!success)
    {
        Debug.LogError("GLTF load failed");
        return null;
    }

    GameObject root = new GameObject("GLTF_Model");
    bool instSuccess = await gltf.InstantiateMainSceneAsync(root.transform);

    if (!instSuccess)
    {
        Debug.LogError("GLTF instantiation failed");
        return null;
    }

    // 1. Pobierz wszystkie MeshFiltery
    MeshFilter[] meshFilters = root.GetComponentsInChildren<MeshFilter>();
    if (meshFilters.Length == 0)
    {
        Debug.LogWarning("No meshes found in glTF model.");
        return root;
    }

    // 2. Połącz meshe
    CombineInstance[] combine = new CombineInstance[meshFilters.Length];
    for (int i = 0; i < meshFilters.Length; i++)
    {
        if (meshFilters[i].sharedMesh == null) continue;

        combine[i].mesh = meshFilters[i].sharedMesh;
        combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
    }

    Mesh combinedMesh = new Mesh();
    combinedMesh.CombineMeshes(combine, true, true);

    // 3. Stwórz nowy obiekt z połączonym meshem
    GameObject combinedObject = new GameObject("CombinedObject");
    combinedObject.transform.SetParent(root.transform);
    combinedObject.transform.localPosition = Vector3.zero;
    combinedObject.transform.localRotation = Quaternion.identity;

    MeshFilter mf = combinedObject.AddComponent<MeshFilter>();
    mf.sharedMesh = combinedMesh;

    MeshRenderer mr = combinedObject.AddComponent<MeshRenderer>();
    var firstRenderer = meshFilters[0].GetComponent<Renderer>();
    if (firstRenderer != null)
        mr.sharedMaterial = firstRenderer.sharedMaterial;

    // 4. Dodaj komponenty do zarządzania
    combinedObject.AddComponent<MeshCollider>();
    combinedObject.AddComponent<ObjectManipulator>();
    combinedObject.AddComponent<Model>();

    // 5. Usuń oryginalne dzieci (bo już połączone)
    for (int i = root.transform.childCount - 1; i >= 0; i--)
    {
        var child = root.transform.GetChild(i);
        if (child.gameObject != combinedObject)
            Destroy(child.gameObject);
    }

    return root;
}*/
    
    public void PlaceObjectInFrontOfXR(Transform xrCamera, GameObject objToPlace, float distance = 1.5f)
    {
        Vector3 forwardFlat = Vector3.ProjectOnPlane(xrCamera.forward, Vector3.up).normalized;
        Vector3 targetPosition = xrCamera.position + forwardFlat * distance;

        // Przesunięcie o wysokość modelu (aby nie wchodził w ziemię)
        var bounds = objToPlace.GetComponent<MeshRenderer>()?.bounds;
        float modelHalfHeight = bounds.HasValue ? bounds.Value.extents.y : 0f;

        objToPlace.transform.position = targetPosition + new Vector3(0, modelHalfHeight, 0);
        objToPlace.transform.LookAt(new Vector3(xrCamera.position.x, objToPlace.transform.position.y, xrCamera.position.z));
    }
    
    public async Task<GameObject> LoadGltfToGameObject(string path, Transform xrCamera, float distance = 1.5f)
{
    var gltf = new GltfImport();
    bool success = await gltf.Load(path);

    if (!success)
    {
        Debug.LogError("GLTF load failed");
        return null;
    }

    GameObject root = new GameObject("GLTF_Model");
    bool instSuccess = await gltf.InstantiateMainSceneAsync(root.transform);
    if (!instSuccess)
    {
        Debug.LogError("GLTF instantiation failed");
        return null;
    }

    // 1. Połącz wszystkie meshe
    MeshFilter[] meshFilters = root.GetComponentsInChildren<MeshFilter>();
    if (meshFilters.Length == 0) return root;

    CombineInstance[] combine = new CombineInstance[meshFilters.Length];
    for (int i = 0; i < meshFilters.Length; i++)
    {
        if (meshFilters[i].sharedMesh == null) continue;
        combine[i].mesh = meshFilters[i].sharedMesh;
        combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
    }

    Mesh combinedMesh = new Mesh();
    combinedMesh.CombineMeshes(combine, true, true);

    // 2. Stwórz nowy obiekt z połączonym meshem
    GameObject combinedObject = new GameObject("CombinedObject");
    combinedObject.transform.SetParent(root.transform);

    MeshFilter mf = combinedObject.AddComponent<MeshFilter>();
    mf.sharedMesh = combinedMesh;

    MeshRenderer mr = combinedObject.AddComponent<MeshRenderer>();
    if (meshFilters[0].TryGetComponent(out Renderer sourceRenderer))
    {
        mr.sharedMaterial = sourceRenderer.sharedMaterial;
    }

    // 3. Umieść przed graczem XR
    PlaceObjectInFrontOfXR(xrCamera, combinedObject, distance);

    // 4. Dodaj komponenty
    combinedObject.AddComponent<MeshCollider>();
    combinedObject.AddComponent<Model>();
    combinedObject.AddComponent<ObjectManipulator>();

    // 5. Usuń stare dzieci (oryginalne meshe)
    for (int i = root.transform.childCount - 1; i >= 0; i--)
    {
        Transform child = root.transform.GetChild(i);
        if (child.gameObject != combinedObject)
            Destroy(child.gameObject);
    }

    return root;
}
    
    public async Task<GameObject> LoadGltfToAttachToController(string path)
    {
        var gltf = new GltfImport();
        bool success = await gltf.Load(path);
        
        if (!success)
        {
            Debug.LogError("GLTF load failed");
            return null;
        }
        
        GameObject root = new GameObject("GLTF_Model");
        bool instSuccess = await gltf.InstantiateMainSceneAsync(root.transform);
        
        if (!instSuccess)
        {
            Debug.LogError("GLTF instantiation failed");
            return null;
        }
        
        gltf.Dispose();

        return root;
    }
    
}
