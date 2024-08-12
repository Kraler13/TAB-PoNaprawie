using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;
using TMPro;

public class InputMenager : MonoBehaviour
{
    [SerializeField] private SquadSelection selectedSquads;
    [SerializeField] private List<Vector3> listOfPoints = new List<Vector3>();
    [SerializeField] private ActionButtonsScriptableObj actionButtonsScriptableObj;
    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask enemy;
    [SerializeField] private LayerMask building;

    public bool startPatrol = false;
    private float distanceBetweenSquads = 5;
    private LayerMask combinedLayerMaskForEnemy;
    private LayerMask combinedLayerMaskForBuilding;
    private MainBuildingActionButtons buildingActionButtons;
    private void Start()
    {
        combinedLayerMaskForEnemy = ground | enemy;
        combinedLayerMaskForBuilding = ground | building;
        buildingActionButtons = GetComponent<MainBuildingActionButtons>();
    }
    void Update()
    {
        BuildingSelection();
        SquadHandleInput();
        ClearActionButtons();
    }

    void ClearActionButtons()
    {
        if (Input.GetMouseButtonDown(0) && selectedSquads.SquadsSelected != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, combinedLayerMaskForBuilding))
            {
                if (hit.transform.gameObject.tag != "Building")
                {
                    foreach (var button in actionButtonsScriptableObj.buttons)
                    {
                        button.onClick.RemoveAllListeners();
                        button.GetComponentInChildren<TextMeshProUGUI>().text = "";
                    }
                }
            }
        }
    }
    void SquadHandleInput()
    {
        var table = selectedSquads.SquadsSelected;
        if (Input.GetMouseButtonDown(1) && table.Count != 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, combinedLayerMaskForEnemy))
            {
                if (startPatrol)
                {
                    StartPatrol(hit);
                    return;
                }
                if (hit.transform.gameObject.tag != "Enemy")
                {
                    JustMoving(hit);
                    return;
                }
                else
                {
                    AttackEnemy(hit);
                }
                
            }
        }
    }
    void StartPatrol(RaycastHit hit)
    {
        MathfHendle(hit.point);
        startPatrol = false;
        for (int i = 0; i < selectedSquads.SquadsSelected.Count; i++)
        {
            selectedSquads.SquadsSelected[i].TryGetComponent<SquadLogic>(out SquadLogic squadLogic);
            if (squadLogic != null)
            {
                squadLogic.isPatroling = true;
                squadLogic.StartPatrol(listOfPoints[i]);
            }
        }
    }

    void BuildingSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, building))
            {
                if (hit.transform.gameObject.tag == "Building")
                {
                    selectedSquads.DeselectAll();
                    buildingActionButtons.OnBuildingCliced(hit);
                }
            }
        }
    }
    void JustMoving(RaycastHit hit)
    {
        MathfHendle(hit.point);

        for (int i = 0; i < selectedSquads.SquadsSelected.Count; i++)
        {
            selectedSquads.SquadsSelected[i].TryGetComponent<SquadLogic>(out SquadLogic squadLogic);
            if (squadLogic != null)
            {
                if (squadLogic.isPatroling)
                {
                    squadLogic.isPatroling = false;                    
                }
                squadLogic.isAttacking = false;
                squadLogic.MoveToDestination(listOfPoints[i]);
                squadLogic.isMoving = true;
            }
            else
            {
                selectedSquads.SquadsSelected[i].GetComponent<SingleUniteSquad>().MoveToDestination(listOfPoints[i]);
            }
        }
    }
    void AttackEnemy(RaycastHit hit)
    {
        MathfHendle(hit.point);
        for (int i = 0; i < selectedSquads.SquadsSelected.Count; i++)
        {
            selectedSquads.SquadsSelected[i].TryGetComponent<SquadLogic>(out SquadLogic squadLogic);
            if (squadLogic != null)
            {
                squadLogic.isMoving = false;
                squadLogic.MoveToDestination(listOfPoints[i]);
                squadLogic.enemy = hit.collider.gameObject;
            }
            else
                selectedSquads.SquadsSelected[i].GetComponent<SingleUniteSquad>().MoveToDestination(listOfPoints[i]);
        }
    }
    void MathfHendle(Vector3 hitPointValue)
    {

        listOfPoints.Clear();
        Vector3 pointA = selectedSquads.SquadsSelected[0].transform.position;
        Vector3 pointB = hitPointValue;
        Vector3 vectorAB = pointB - pointA;
        Vector3 normalizedAB = vectorAB.normalized;
        Vector3 offset = distanceBetweenSquads * normalizedAB;

        listOfPoints.Add(pointB);
        listOfPoints.Add(pointB + distanceBetweenSquads * new Vector3(normalizedAB.x, 0, -normalizedAB.z));
        listOfPoints.Add(pointB - distanceBetweenSquads * new Vector3(normalizedAB.x, 0, -normalizedAB.z));
        listOfPoints.Add(pointB - offset);
        listOfPoints.Add(pointB - offset + distanceBetweenSquads * new Vector3(normalizedAB.x, 0, -normalizedAB.z));
        listOfPoints.Add(pointB - offset - distanceBetweenSquads * new Vector3(normalizedAB.x, 0, -normalizedAB.z));
        listOfPoints.Add(pointB - offset * 2);
        listOfPoints.Add(pointB - offset * 2 + distanceBetweenSquads * new Vector3(normalizedAB.x, 0, -normalizedAB.z));
        listOfPoints.Add(pointB - offset * 2 - distanceBetweenSquads * new Vector3(normalizedAB.x, 0, -normalizedAB.z));
    }
}

