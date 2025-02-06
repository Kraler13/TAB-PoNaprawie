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
    [SerializeField] private GameObject selection;
    private int selectedObjIndex = -1;
    private List<GameObject> placedBuildings = new List<GameObject>();
    private Vector3Int lastDetectedPosition = Vector3Int.zero;
    public List<BuildInRange> buildingsWithMoreRange = new List<BuildInRange>();
    public List<ResorsGatheringRange> ResorsGatheringRange = new List<ResorsGatheringRange>();
    public List<ResorsGatheringRange> forestBuildings = new List<ResorsGatheringRange>();
    public List<ResorsGatheringRange> stoneBuildings = new List<ResorsGatheringRange>();
    public bool isBuilding = false;
    private void Start()
    {
        var extender = GameObject.FindGameObjectWithTag("Building");
        buildingsWithMoreRange.Add(extender.GetComponentInChildren<BuildInRange>());
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
        WhatToDisable();
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
        foreach (BuildInRange builing in buildingsWithMoreRange)
        {
            builing.PrevievObj = previevSystem.PreviewObject;
        }
        PrevievResoursBuilding();              
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
        if (inputForGridSystem.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePositio = inputForGridSystem.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePositio);      
        if (!CheckPlacementValidyty())
            return;
        GameObject newBuilding = Instantiate(buildingsDataScriptableObj.buildingsDatas[selectedObjIndex].Prefab);
        BuildBuildingWithMoreRang(newBuilding);
        BuildResorseBuilding(newBuilding);
        BuildHouseBuilding(newBuilding);
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
            if (building.isValid)
                isValid = true;
        }
        foreach (var building in ResorsGatheringRange)
        {
            if (building.isValid)
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
        selection.SetActive(true);
        isBuilding = false;
    }

    private void WhatToDisable()
    {
        selection.SetActive(false);
        isBuilding = true;
    }

    private void PrevievResoursBuilding()
    {
        if (previevSystem.ResorsGathering != null)
        {
            if (previevSystem.ResorsGathering.ForestBuilding)
            {
                foreach (var forestBuild in forestBuildings)
                {
                    if (previevSystem.PreviewObject != null)
                        forestBuild.PrevievObj = previevSystem.PreviewObject;
                }
            }
            if (previevSystem.ResorsGathering.StoneBuilding)
            {
                foreach (var stoneBuild in stoneBuildings)
                {
                    if (previevSystem.PreviewObject != null)
                        stoneBuild.PrevievObj = previevSystem.PreviewObject;
                }
            }
        }
    }

    private void BuildBuildingWithMoreRang(GameObject newBuilding)
    {
        if (buildingsDataScriptableObj.buildingsDatas[selectedObjIndex].BuildingWithMoreRang)
        {
            buildingsWithMoreRange.Add(newBuilding.GetComponentInChildren<BuildInRange>());
            newBuilding.GetComponentInChildren<BoxCollider>().enabled = true;
        }
    }
    private void BuildResorseBuilding(GameObject newBuilding)
    {
        if (buildingsDataScriptableObj.buildingsDatas[selectedObjIndex].ResorseBuilding)
        {
            if (newBuilding.GetComponentInChildren<ResorsGathering>().ForestBuilding)
            {
                resorsSriptableObj.ForestCountTiles += resorsSriptableObj.ForestCountTilesToAdd;
                resorsSriptableObj.ForestCountTilesToAdd = 0;
                forestBuildings.Add(newBuilding.GetComponentInChildren<ResorsGatheringRange>());
                newBuilding.GetComponentInChildren<ResorsGatheringRange>().enabled = true;
            }
            if (newBuilding.GetComponentInChildren<ResorsGathering>().StoneBuilding)
            {
                resorsSriptableObj.StoneCountTiles += resorsSriptableObj.StoneCountTilesToAdd;
                resorsSriptableObj.StoneCountTilesToAdd = 0;
                stoneBuildings.Add(newBuilding.GetComponentInChildren<ResorsGatheringRange>());
                newBuilding.GetComponentInChildren<ResorsGatheringRange>().enabled = true;
            }
            Destroy(newBuilding.GetComponentInChildren<ResorsGathering>().rb);
        }
    }
    private void BuildHouseBuilding(GameObject newBuilding)
    {
        if (buildingsDataScriptableObj.buildingsDatas[selectedObjIndex].HouseBuilding)
        {
            if (newBuilding.GetComponentInChildren<House>())
            {
                resorsSriptableObj.WorkForce += newBuilding.GetComponentInChildren<House>().HowMenyPeoupleToAdd;
            }       
            Destroy(newBuilding.GetComponentInChildren<House>());
        }
    }
}
