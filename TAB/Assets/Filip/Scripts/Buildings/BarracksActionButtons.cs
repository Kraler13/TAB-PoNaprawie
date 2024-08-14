using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BarracksActionButtons : MonoBehaviour
{
    [SerializeField] private ActionButtonsScriptableObj ActionButtonsScriptableObj;
    [SerializeField] private GameObject firstSquad;
    [SerializeField] private GameObject secondSquad;
    [SerializeField] private GameObject spownPoint;

    public void OnBuildingCliced(RaycastHit hit)
    {
        BarracksBuildingActions();
    }
    private void BarracksBuildingActions()
    {
        ClearButtons();
        ActionButtonsScriptableObj.buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "FirstSquad";
        ActionButtonsScriptableObj.buttons[0].onClick.AddListener(FirstSquad);
        //ActionButtonsScriptableObj.buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Forester";
        //ActionButtonsScriptableObj.buttons[1].onClick.AddListener(BuildForester);
        //ActionButtonsScriptableObj.buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Stoner";
        //ActionButtonsScriptableObj.buttons[2].onClick.AddListener(BuildStoner);
        //ActionButtonsScriptableObj.buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = "RangeExtender";
        //ActionButtonsScriptableObj.buttons[3].onClick.AddListener(BuildRangeExtender);
        //ActionButtonsScriptableObj.buttons[4].GetComponentInChildren<TextMeshProUGUI>().text = "Barracks";
        //ActionButtonsScriptableObj.buttons[4].onClick.AddListener(BuildBarracks);
    }
    private void FirstSquad()
    {
        GameObject squad = Instantiate(firstSquad, spownPoint.transform.position, spownPoint.transform.rotation);
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
