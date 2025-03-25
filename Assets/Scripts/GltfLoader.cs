using UnityEngine;
using GLTFast;
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
            
            gltf.Dispose();
        } else {
            Debug.LogError("Loading glTF failed!");
        }

    }
}
