using System;
using MixedReality.Toolkit;
using MixedReality.Toolkit.SpatialManipulation;
using TMPro;
using UnityEngine;
using static MixedReality.Toolkit.TransformFlags;

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
    public void SetMoveFlag()
    {
        Debug.Log(model.GetComponent<ObjectManipulator>().AllowedManipulations);
        model.gameObject.GetComponent<ObjectManipulator>().AllowedManipulations = TransformFlags.Move;
        Debug.Log(model.GetComponent<ObjectManipulator>().AllowedManipulations);
    }

    public void SetRotateFlag()
    {
        Debug.Log(model.GetComponent<ObjectManipulator>().AllowedManipulations);
        model.gameObject.GetComponent<ObjectManipulator>().AllowedManipulations = TransformFlags.Rotate;
        Debug.Log(model.GetComponent<ObjectManipulator>().AllowedManipulations);
    }

    public void SetScaleFlag()
    {
        Debug.Log(model.GetComponent<ObjectManipulator>().AllowedManipulations);
        model.gameObject.GetComponent<ObjectManipulator>().AllowedManipulations = TransformFlags.Scale;
        Debug.Log(model.GetComponent<ObjectManipulator>().AllowedManipulations);
    }
    
    
    public void SetName(string name)
    {
        label.text = name;
    }

    private void OnDestroy()
    {
        
    }
}
