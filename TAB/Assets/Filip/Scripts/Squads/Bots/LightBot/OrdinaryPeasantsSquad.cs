using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

//KA¯DY SQUADBUTTON MA SWOJ¥ LOGIKÊ
public class OrdinaryPeasantsSquad : MonoBehaviour
{
    [SerializeField] private ActionButtonsScriptableObj actionButtonsScriptableObj;
    private int maxUnites = 9;
    public GameObject SquadConnectedToButton;
    private SquadLogic squadLogic;
    public Image HPImage;
    private int uniteCount = 1;
    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(AllButtons);
        squadLogic = GetComponent<SquadAndUniteButtonHendeler>().SquadConnectedToButton.GetComponent<SquadLogic>();
        squadLogic.ListOfSpowningPointsToChange.Clear();
        foreach (var item in squadLogic.ListOfSpowningPoints)
        {
            squadLogic.ListOfSpowningPointsToChange.Add(item);
        }
        uniteCount = squadLogic.CurentUnitesCount;
    }

    private void AllButtons()
    {
        foreach (var button in actionButtonsScriptableObj.buttons)
        {
            button.onClick.RemoveAllListeners();
            button.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
        actionButtonsScriptableObj.buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "dodaj";
        actionButtonsScriptableObj.buttons[0].onClick.AddListener(Multiply);
        actionButtonsScriptableObj.buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Patroluj";
        actionButtonsScriptableObj.buttons[1].onClick.AddListener(Patrol);
    }
    void Multiply()
    {
        if (maxUnites > uniteCount)
        {
            squadLogic.resorsSriptableObj.ResorsOne -= 10f;
            var createdUnite = Instantiate(squadLogic.Unite, squadLogic.ListOfSpowningPointsToChange[0].transform);
            createdUnite.GetComponent<UniteLogic>().pointToFollow = squadLogic.ListOfSpowningPointsToChange[0];
            squadLogic.ListOfSpowningPointsToChange.RemoveAt(0);
            squadLogic.Unites.Add(createdUnite);
            createdUnite.GetComponent<UniteHealth>().squadLogic = squadLogic;
            createdUnite.GetComponent<UniteHealth>().CurrentUniteHP = squadLogic.gameObject.GetComponent<SquadHealth>().maxUniteHP;
            uniteCount++;
            squadLogic.CurentUnitesCount++;
        }
    }

    void Patrol()
    {
        GameObject.FindGameObjectWithTag("LvlMenager").GetComponent<InputMenager>().startPatrol = true;
    }
}
