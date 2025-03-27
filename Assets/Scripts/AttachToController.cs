using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AttachToController : MonoBehaviour
{
    public GameObject objectToAttach;
    public GameObject controller;
    public float rotationSpeed = 10f; // prędkość obrotu w stopniach na sekundę

    public GameObject newPrefab;
    public GameObject currentObject;

    void Start()
    {
        // if (objectToAttach != null && controller != null)
        // {
        //     //MovePivotToBottom(objectToAttach);
        //     objectToAttach.transform.SetParent(controller.transform);
        //     objectToAttach.transform.localPosition = Vector3.zero;
        //     objectToAttach.transform.localRotation = Quaternion.identity;
        // }
        GetObjectSize.GetSize(currentObject);
        StartCoroutine(SwapAfterTimeFunction());
    }

    void Update()
    {
        // if (objectToAttach != null)
        // {
        //     // Obracaj obiekt wokół własnej osi Y w lewo (czyli przeciwnie do ruchu wskazówek zegara)
        //     objectToAttach.transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime, Space.Self);
        // }
        
        currentObject.transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime, Space.Self);

        
    }

    IEnumerator SwapAfterTimeFunction()
    {
        yield return new WaitForSeconds(10);
        SwapAfterTime();
    }
    
    void SwapAfterTime()
    {
        ReplaceObject(currentObject, newPrefab);
        objectToAttach = newPrefab;
        GetObjectSize.GetSize(currentObject);
    }

    public void ReplaceObject(GameObject oldObj, GameObject newPrefab)
{
    if (oldObj == null || newPrefab == null) return;

    // 1. Pobierz rozmiar starego obiektu
    Renderer oldRenderer = oldObj.GetComponentInChildren<Renderer>();
    if (oldRenderer == null) return;
    Vector3 oldSize = oldRenderer.bounds.size;

    // 2. Zapamiętaj pozycję/rotację i parenta
    Transform parent = oldObj.transform.parent;
    Vector3 position = oldObj.transform.position;
    Quaternion rotation = oldObj.transform.rotation;

    // 3. Usuń stary obiekt
    Destroy(oldObj);

    // 4. Stwórz nowy obiekt tymczasowo
    GameObject tempInstance = Instantiate(newPrefab, position, rotation);

    // 5. Zbierz wszystkie meshe
    MeshFilter[] meshFilters = tempInstance.GetComponentsInChildren<MeshFilter>();
    if (meshFilters.Length == 0) return;

    CombineInstance[] combine = new CombineInstance[meshFilters.Length];

    for (int i = 0; i < meshFilters.Length; i++)
    {
        if (meshFilters[i].sharedMesh == null) continue;

        combine[i].mesh = meshFilters[i].sharedMesh;
        combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
    }

    Mesh combinedMesh = new Mesh();
    combinedMesh.CombineMeshes(combine, true, true);

    // 6. Utwórz finalny GameObject z połączonym meshem
    GameObject newObj = new GameObject("CombinedObject");
    newObj.transform.SetPositionAndRotation(position, rotation);
    if (parent != null)
        newObj.transform.SetParent(parent);

    MeshFilter mf = newObj.AddComponent<MeshFilter>();
    mf.sharedMesh = combinedMesh;

    MeshRenderer mr = newObj.AddComponent<MeshRenderer>();
    mr.sharedMaterial = meshFilters[0].GetComponent<Renderer>().sharedMaterial;

    // 7. Oblicz rozmiar nowego obiektu
    Vector3 newSize = combinedMesh.bounds.size;

    // 8. Oblicz wspólny współczynnik skalowania – proporcjonalnie (żeby się zmieścił)
    float scaleX = oldSize.x / newSize.x;
    float scaleY = oldSize.y / newSize.y;
    float scaleZ = oldSize.z / newSize.z;
    float uniformScale = Mathf.Min(scaleX, scaleY, scaleZ);

    newObj.transform.localScale = Vector3.one * uniformScale;

    // 9. Wycentruj nowy obiekt względem starego
    Vector3 adjustedCenter = mr.bounds.center;
    Vector3 offset = position - adjustedCenter;
    newObj.transform.position += offset;

    // 10. Posprzątaj tymczasowy prefab
    Destroy(tempInstance);

    // 11. Przypisz referencję
    currentObject = newObj;
}
    
    public bool HasMultipleMeshes(GameObject obj)
    {
        // Pobierz wszystkie MeshFiltery w obiekcie i jego dzieciach
        MeshFilter[] meshFilters = obj.GetComponentsInChildren<MeshFilter>();

        // Jeśli więcej niż jeden — mamy kilka meshów
        return meshFilters.Length > 1;
    }
    
    void MovePivotToBottom(GameObject obj)
    {
        // Pobierz renderer, żeby dostać bounds
        var renderer = obj.GetComponentInChildren<Renderer>();
        if (renderer == null) return;

        Bounds bounds = renderer.bounds;

        // Punkt bottom center (w przestrzeni świata)
        Vector3 bottomCenter = new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);

        // Utwórz nowego parenta w miejscu bottom pivotu
        GameObject pivotHolder = new GameObject(obj.name + "_BottomPivot");
        pivotHolder.transform.position = bottomCenter;
        pivotHolder.transform.rotation = obj.transform.rotation;

        // Oblicz lokalną pozycję względem parenta, z uwzględnieniem skali
        Vector3 worldToLocalOffset = obj.transform.position - bottomCenter;
        Vector3 localOffset = Quaternion.Inverse(obj.transform.rotation) * worldToLocalOffset;
        localOffset = new Vector3(
            localOffset.x / obj.transform.localScale.x,
            localOffset.y / obj.transform.localScale.y,
            localOffset.z / obj.transform.localScale.z
        );

        // Ustaw parenta
        obj.transform.SetParent(pivotHolder.transform);
        obj.transform.localPosition = localOffset;

        // (opcjonalnie) przypisz nowego parenta jako obiekt do dalszego użycia
        objectToAttach = pivotHolder;
    }
    
}
