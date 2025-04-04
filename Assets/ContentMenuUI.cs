using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ContentMenuUI : MonoBehaviour
{
    [SerializeField] private List<ModelMenuField> menuFields;

    private void Start()
    {
        menuFields = GetComponentsInChildren<ModelMenuField>().ToList();
    }

    public void AddMenuField(ModelMenuField menuField)
    {
        menuFields.Add(menuField);
    }

    public void RemoveMenuField(ModelMenuField menuField)
    {
        menuFields.Remove(menuField);
    }
}
