using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResorsMenager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Wood;
    [SerializeField] private TextMeshProUGUI Stone;
    [SerializeField] private TextMeshProUGUI Food;
    [SerializeField] private TextMeshProUGUI WorkForce;
    [SerializeField] private ResorsSriptableObj resorsSriptableObj;
    private bool k = true;

    private void Start()
    {
        if (resorsSriptableObj.isNewGame)
        {
            resorsSriptableObj.ForestCountTilesToAdd = 0;
            resorsSriptableObj.ForestCountTiles = 0;
            resorsSriptableObj.StoneCountTiles = 0;
            resorsSriptableObj.StoneCountTilesToAdd = 0;
            resorsSriptableObj.FoodCountTiles = 0;
            resorsSriptableObj.FoodCountTilesToAdd = 0;
            resorsSriptableObj.WorkForce = 15;
        }
    }
    private void FixedUpdate()
    {
        if (k)
            StartCoroutine(UpdateResorsOnHUD());
    }

    IEnumerator UpdateResorsOnHUD()
    {
        k = false;
        resorsSriptableObj.Wood += resorsSriptableObj.ForestCountTiles;
        resorsSriptableObj.Stone += resorsSriptableObj.StoneCountTiles;
        resorsSriptableObj.Food += resorsSriptableObj.FoodCountTiles;
        Wood.text = resorsSriptableObj.Wood.ToString();
        Stone.text = resorsSriptableObj.Stone.ToString();
        Food.text = resorsSriptableObj.Food.ToString();
        WorkForce.text = resorsSriptableObj.WorkForce.ToString();
        yield return new WaitForSeconds(1);
        k = true;
    }
}
