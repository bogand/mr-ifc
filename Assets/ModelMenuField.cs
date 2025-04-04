using System;
using TMPro;
using UnityEngine;

public class ModelMenuField : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;

    public void Delete()
    {
        Destroy(gameObject);
    }

    private void Start()
    {
        label.text = "dupa";
    }

    public void SetName(string name)
    {
        label.text = name;
    }
}
