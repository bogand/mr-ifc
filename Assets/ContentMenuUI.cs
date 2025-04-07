using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ContentMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject modelRowFieldUIPrefab;
    [SerializeField] private List<ModelRowFieldUI> menuFields;
    
    private void Start()
    {
        ObjectContainer.Instance.OnListChanged += OnListChanged;
        menuFields = GetComponentsInChildren<ModelRowFieldUI>().ToList();
    }

    public void OnListChanged(object sender,List<GameObject> list)
    {
        UpdateUI(list);
    }


    private void UpdateUI(List<GameObject> list)
    {
        List<GameObject> currentObjects = list;
        Transform menuTransform = this.transform;
        foreach (Transform modelMenuField in menuTransform)
        {
            if (modelMenuField.gameObject.GetComponent<ModelRowFieldUI>())
            {
                Destroy(modelMenuField.gameObject);
            }
        }

        foreach (var obj in currentObjects)
        {
            GameObject modelField = Instantiate(modelRowFieldUIPrefab, menuTransform);
            ModelRowFieldUI modelRowFieldUI = modelField.GetComponent<ModelRowFieldUI>();
            modelRowFieldUI.init(obj);
        }
        
    }
    
    public void AddMenuField(ModelRowFieldUI rowFieldUI)
    {
        menuFields.Add(rowFieldUI);
    }

    public void RemoveMenuField(ModelRowFieldUI rowFieldUI)
    {
        menuFields.Remove(rowFieldUI);
    }
}
