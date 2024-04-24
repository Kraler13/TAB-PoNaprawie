using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    public bool isColliding = false;
    [SerializeField] private GameObject mousieIndicator;
    [SerializeField] private InputForGridSystem inputForGridSystem;
    [SerializeField] private Grid grid;
    [SerializeField] private BuildingsDataScriptableObj buildingsDataScriptableObj;
    [SerializeField] private GameObject gridVisual;
    [SerializeField] private PrevievSystem previevSystem;
    private int selectedObjIndex = -1;
    private GridData groundData;
    private GridData buildingData;
    private List<GameObject> placedBuildings = new List<GameObject>();
    private Vector3Int lastDetectedPosition = Vector3Int.zero;
    private List<BuildInRange> buildingsWithMoreRange = new List<BuildInRange>();
    private void Start()
    {
        buildingsWithMoreRange.Add(GameObject.FindGameObjectWithTag("RangeExtender").GetComponent<BuildInRange>());
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
            mousieIndicator.transform.position = mousePositio;
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
        if (buildingsDataScriptableObj.buildingsDatas[selectedObjIndex].BuildingWithMoreRang)
        {
            buildingsWithMoreRange.Add(buildingsDataScriptableObj.buildingsDatas[selectedObjIndex].BuildInRange);
        }
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
        newBuilding.transform.position = grid.CellToWorld(gridPosition);
        placedBuildings.Add(newBuilding);
        Rigidbody rb = newBuilding.GetComponentInChildren<Rigidbody>();
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
            return true;
        else 
            return false;
    }
}
