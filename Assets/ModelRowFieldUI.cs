using System;
using TMPro;
using UnityEngine;

public class ModelRowFieldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private GameObject model;
    public event EventHandler<GameObject> OnObjectDeleted;

    public void init(GameObject model)
    {
        this.model = model;
        label.text = model.name;
    }

    void Start()
    {
        if (model.name != null)
        {
            label.text = model.name;
        }
        this.OnObjectDeleted += ObjectContainer.Instance.OnObjectDeleted;
    }

    public void Delete()
    {
        OnObjectDeleted?.Invoke(this, model.gameObject);
        this.OnObjectDeleted -= ObjectContainer.Instance.OnObjectDeleted;
        Destroy(gameObject);
    }
    
    public void SetName(string name)
    {
        label.text = name;
    }

    private void OnDestroy()
    {
        
    }
}
