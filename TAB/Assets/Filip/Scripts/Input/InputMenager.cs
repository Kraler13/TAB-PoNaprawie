using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class InputMenager : MonoBehaviour
{
    [SerializeField] private SquadSelection selectedSquads;
    [SerializeField] private List<Vector3> listOfPoints = new List<Vector3>();
    [SerializeField] private List<GameObject> squadPatroling = new List<GameObject>();
    [SerializeField] private List<Vector3> oldPositionInPatrol = new List<Vector3>();
    [SerializeField] private List<Vector3> newPositionInPatrol = new List<Vector3>();
    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask building;

    public bool startPatrol = false;
    private float distanceBetweenSquads = 5;
    private bool patrolCheker;

    void Update()
    {
        BuildingSelection();
        foreach (var squad in squadPatroling)
        {
            var squadLogic = squad.GetComponent<SquadLogic>();
            if (squadLogic.isPatroling && squadLogic.GoBackInPatrol)
            {
                Patrol();
            }
        }
        SquadHandleInput();
    }

    void SquadHandleInput()
    {
        var table = selectedSquads.SquadsSelected;
        if (Input.GetMouseButtonDown(1) && table.Count != 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                if (startPatrol)
                {
                    StartPatrol(hit);
                    return;
                }
                if (hit.transform.gameObject.tag == "Enemy")
                {
                    AttackEnemy(hit);
                }
                else
                {
                    JustMoving(hit);
                }
            }
        }
    }
    void StartPatrol(RaycastHit hit)
    {
        patrolCheker = true;
        MathfHendle(hit.point);

        startPatrol = false;
        squadPatroling.Clear();
        oldPositionInPatrol.Clear();


        for (int i = 0; i < selectedSquads.SquadsSelected.Count; i++)
        {
            squadPatroling.Add(selectedSquads.SquadsSelected[i]);
            oldPositionInPatrol.Add(selectedSquads.SquadsSelected[i].transform.position);
            selectedSquads.SquadsSelected[i].TryGetComponent<SquadLogic>(out SquadLogic squadLogic);
            if (squadLogic != null)
            {
                squadLogic.isPatroling = true;
                squadLogic.PatrolTargetPosition = newPositionInPatrol[i];
                squadLogic.MoveToDestination(newPositionInPatrol[i]);
            }
            else
                selectedSquads.SquadsSelected[i].GetComponent<SingleUniteSquad>().MoveToDestination(listOfPoints[i]);
        }
    }
    void Patrol()
    {
        patrolCheker = !patrolCheker;

        for (int i = 0; i < squadPatroling.Count; i++)
        {
            squadPatroling[i].TryGetComponent<SquadLogic>(out SquadLogic squadLogic);
            if (squadLogic != null)
            {
                if (patrolCheker)
                {
                    squadLogic.MoveToDestination(oldPositionInPatrol[i]);
                    squadLogic.PatrolTargetPosition = oldPositionInPatrol[i];
                    Debug.Log(oldPositionInPatrol[i]);
                }
                else
                {
                    squadLogic.MoveToDestination(newPositionInPatrol[i]);
                    squadLogic.PatrolTargetPosition = newPositionInPatrol[i];
                    Debug.Log(newPositionInPatrol[i]);
                }


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
                squadLogic.isMoving = true;
                if (squadLogic.isPatroling)
                {
                    squadLogic.isPatroling = false;
                    squadLogic.isAttacking = false;
                    int ind = squadPatroling.IndexOf(squadLogic.gameObject);
                    squadPatroling.RemoveAt(ind);
                }
                squadLogic.MoveToDestination(listOfPoints[i]);
            }
            else
                selectedSquads.SquadsSelected[i].GetComponent<SingleUniteSquad>().MoveToDestination(listOfPoints[i]);
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
        if (startPatrol)
        {
            for (int i = 0; i < selectedSquads.SquadsSelected.Count; i++)
            {
                newPositionInPatrol.Add(listOfPoints[i]);
            }
        }
    }
}

