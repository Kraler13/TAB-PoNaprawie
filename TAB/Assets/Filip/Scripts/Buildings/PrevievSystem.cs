using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PrevievSystem : MonoBehaviour
{
    [SerializeField] private GameObject selection;
    [SerializeField] private float previewYOffset = 0.06f;
    [SerializeField] private Material previewMaterialsPrefab;
    public GameObject PreviewObject;
    public ResorsGathering ResorsGathering;
    private Material previewMaterialsInstance;
    private void Start()
    {
        previewMaterialsInstance = new Material(previewMaterialsPrefab);
    }

    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        PreviewObject = Instantiate(prefab);
        PreperePreview(PreviewObject);
    }


    private void PreperePreview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialsInstance;
            }
            renderer.materials = materials;
        }
        if (previewObject.GetComponentInChildren<ResorsGathering>() != null)
        {
            ResorsGathering = previewObject.GetComponentInChildren<ResorsGathering>();
        }
    }

    public void StopShowingPreview()
    {
        Destroy(PreviewObject);
    }

    public void UpdatePosition(Vector3 position, bool validity)
    {
        MovePreview(position);
        ApplyFeedback(validity);
    }

    private void ApplyFeedback(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        c.a = 0.5f;
        previewMaterialsInstance.color = c;
    }


    private void MovePreview(Vector3 position)
    {
        PreviewObject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);
    }
}