using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlacementSystem : MonoBehaviour
{
    public bool isColliding = false;

    [SerializeField] private InputForGridSystem inputForGridSystem;
    [SerializeField] private Grid grid;
    [SerializeField] private BuildingsDataScriptableObj buildingsDataScriptableObj;
    [SerializeField] private ResorsSriptableObj resorsSriptableObj;
    [SerializeField] private GameObject gridVisual;
    [SerializeField] private PrevievSystem previevSystem;
    [SerializeField] private InputMenager input;
    private int selectedObjIndex = -1;
    private List<GameObject> placedBuildings = new List<GameObject>();
    private Vector3Int lastDetectedPosition = Vector3Int.zero;
    public List<BuildInRange> buildingsWithMoreRange = new List<BuildInRange>();
    public List<ResorsGathering> forestBuildings = new List<ResorsGathering>();
    public List<ResorsGathering> stoneBuildings = new List<ResorsGathering>();
    private void Start()
    {
        var extender = GameObject.FindGameObjectWithTag("Building");
        buildingsWithMoreRange.Add(extender.GetComponentInChildren<BuildInRange>());
        Debug.Log(buildingsWithMoreRange[0]);
        StopPlacement();
    }

    void LateUpdate()
    {
        if (selectedObjIndex < 0)
            return;
        Vector3 mousePositio = inputForGridSystem.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePositio);
        if (lastDetectedPosition != gridPosition)
        {
            bool placmentValidyty = CheckPlacementValidyty();
            previevSystem.UpdatePosition(grid.CellToWorld(gridPosition), placmentValidyty);
            lastDetectedPosition = gridPosition;
        }
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        selectedObjIndex = buildingsDataScriptableObj.buildingsDatas.FindIndex(data => data.ID == ID);
        if (selectedObjIndex < 0)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }
        gridVisual.SetActive(true);
        previevSystem.StartShowingPlacementPreview(
            buildingsDataScriptableObj.buildingsDatas[selectedObjIndex].Prefab,
            buildingsDataScriptableObj.buildingsDatas[selectedObjIndex].Size);
        inputForGridSystem.OnClicked += PlaceStructure;
        inputForGridSystem.OnExit += StopPlacement;
    }

    private void StopPlacement()
    { 
        WhatToEnable();
        selectedObjIndex = -1;
        gridVisual.SetActive(false);
        previevSystem.StopShowingPreview();
        inputForGridSystem.OnClicked -= PlaceStructure;
        inputForGridSystem.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
    }

    private void PlaceStructure()
    {
        if(inputForGridSystem.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePositio = inputForGridSystem.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePositio);
        bool placmentValidyty = CheckPlacementValidyty();
        if (!placmentValidyty)
            return;
        GameObject newBuilding = Instantiate(buildingsDataScriptableObj.buildingsDatas[selectedObjIndex].Prefab);
        if (buildingsDataScriptableObj.buildingsDatas[selectedObjIndex].BuildingWithMoreRang)
        {
            buildingsWithMoreRange.Add(newBuilding.GetComponentInChildren<BuildInRange>());
            newBuilding.GetComponentInChildren<BoxCollider>().enabled = true;
        }
        if (buildingsDataScriptableObj.buildingsDatas[selectedObjIndex].ResorseBuilding)
        {
            if (newBuilding.GetComponentInChildren<ResorsGathering>().forestBuilding)
            {
                resorsSriptableObj.ForestCountTiles += resorsSriptableObj.ForestCountTilesToAdd;
                resorsSriptableObj.ForestCountTilesToAdd = 0;
                forestBuildings.Add(newBuilding.GetComponentInChildren<ResorsGathering>());
            }
            if (newBuilding.GetComponentInChildren<ResorsGathering>().stoneBuilding)
            {
                resorsSriptableObj.StoneCountTiles += resorsSriptableObj.StoneCountTilesToAdd;
                resorsSriptableObj.StoneCountTilesToAdd = 0;
            }
            Destroy(newBuilding.GetComponentInChildren<ResorsGathering>().rb);
        }
        newBuilding.transform.position = grid.CellToWorld(gridPosition);
        placedBuildings.Add(newBuilding);
        newBuilding.GetComponentInChildren<NavMeshObstacle>().enabled = true;
        previevSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    private bool CheckPlacementValidyty()
    {
        bool isValid = false;
        foreach (var building in buildingsWithMoreRange)
        {
            if(building.isValid)
                isValid = true;
        }

        if (isValid && !isColliding)
        {
            return true;           
        }
        else
        {
            return false;
        }
    }

    private void WhatToEnable()
    {
        Debug.Log("1");
        input.enabled = false;
    }
}
