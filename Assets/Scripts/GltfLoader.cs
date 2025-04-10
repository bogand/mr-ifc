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
    
    public async Task<GameObject> LoadGltfToGameObject(string path)
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
        model.AddComponent<Model>();
        
        if (!instSuccess)
        {
            Debug.LogError("GLTF instantiation failed");
            return null;
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
