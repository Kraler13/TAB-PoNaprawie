using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildingActionButtons : MonoBehaviour
{
    [SerializeField] private ActionButtonsScriptableObj ActionButtonsScriptableObj;
    [SerializeField] private PlacementSystem placementSystem;
    [SerializeField] private string MainBuilding;
    [SerializeField] private string Forester;
    [SerializeField] private string RangeExtender;
    [SerializeField] private string B2;

    public void OnBuildingCliced(RaycastHit hit)
    {
        if (hit.transform.gameObject.name == MainBuilding) 
        {
            MainBuildingActions();
        }
    }
    private void MainBuildingActions()
    {
        ClearButtons();
        ActionButtonsScriptableObj.buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "B";
        
        ActionButtonsScriptableObj.buttons[0].onClick.AddListener(Build);
    }
    private void Build()
    {
        ClearButtons();
        ActionButtonsScriptableObj.buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "House";
        ActionButtonsScriptableObj.buttons[0].onClick.AddListener(BuildHouse);
        ActionButtonsScriptableObj.buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Forester";
        ActionButtonsScriptableObj.buttons[1].onClick.AddListener(BuildForester);
        ActionButtonsScriptableObj.buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Stoner";
        ActionButtonsScriptableObj.buttons[2].onClick.AddListener(BuildStoner);
        ActionButtonsScriptableObj.buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = "RangeExtender";
        ActionButtonsScriptableObj.buttons[3].onClick.AddListener(BuildRangeExtender);
        //ActionButtonsScriptableObj.buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = "Forester";
        //ActionButtonsScriptableObj.buttons[3].onClick.AddListener(BuildForester);
    }
    private void BuildHouse()
    {
        placementSystem.StartPlacement(1);
    }
    private void BuildForester()
    {
        placementSystem.StartPlacement(4);
    }
    private void BuildStoner()
    {
        placementSystem.StartPlacement(5);
    }
    private void BuildRangeExtender()
    {
        placementSystem.StartPlacement(3);
    }
    private void ClearButtons()
    {
        foreach (var button in ActionButtonsScriptableObj.buttons)
        {
            button.onClick.RemoveAllListeners();
            button.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
    }
}
