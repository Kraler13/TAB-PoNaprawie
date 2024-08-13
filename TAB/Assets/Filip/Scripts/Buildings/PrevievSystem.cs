using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PrevievSystem : MonoBehaviour
{
    [SerializeField] private InputMenager input;
    [SerializeField] private float previewYOffset = 0.06f;
    private GameObject previewObject;
    [SerializeField] private Material previewMaterialsPrefab;
    private Material previewMaterialsInstance;
    private void Start()
    {
        previewMaterialsInstance = new Material(previewMaterialsPrefab);
    }

    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        WhatToDisable();
        previewObject = Instantiate(prefab);
        PreperePreview(previewObject);
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
    }

    public void StopShowingPreview()
    {
        Destroy(previewObject);
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
        previewObject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);
    }

    private void WhatToDisable()
    {
        //Debug.Log("2");

        input.enabled = false;
    }
}