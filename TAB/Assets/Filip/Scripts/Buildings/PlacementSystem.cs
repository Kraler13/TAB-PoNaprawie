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
    private int selectedObjIndex = -1;
    private GridData groundData;
    private GridData buildingData;
    private List<GameObject> placedBuildings = new List<GameObject>();
    private Vector3Int lastDetectedPosition = Vector3Int.zero;
    public List<BuildInRange> buildingsWithMoreRange = new List<BuildInRange>();
    private void Start()
    {
        var extender = GameObject.FindGameObjectWithTag("Building");
        buildingsWithMoreRange.Add(extender.GetComponentInChildren<BuildInRange>());
        StopPlacement();
        groundData = new GridData();
        buildingData = new GridData();
    }

    void Update()
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
                resorsSriptableObj.ForestCountTiles = resorsSriptableObj.ForestCountTilesToAdd;
                resorsSriptableObj.ForestCountTilesToAdd = 0;
            }
        }
        newBuilding.transform.position = grid.CellToWorld(gridPosition);
        placedBuildings.Add(newBuilding);
        Rigidbody rb = newBuilding.GetComponentInChildren<Rigidbody>();
        newBuilding.GetComponentInChildren<NavMeshObstacle>().enabled = true;
        Destroy(rb);
        GridData selectedData = buildingsDataScriptableObj.buildingsDatas[selectedObjIndex].ID == 0 ? groundData : buildingData;
        selectedData.AddObejctAt(gridPosition, buildingsDataScriptableObj.buildingsDatas[selectedObjIndex].Size,
            buildingsDataScriptableObj.buildingsDatas[selectedObjIndex].ID,
            placedBuildings.Count - 1);
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
}
