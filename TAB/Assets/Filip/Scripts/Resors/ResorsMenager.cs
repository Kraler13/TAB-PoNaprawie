using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResorsMenager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resorsOneTxt;
    [SerializeField] private TextMeshProUGUI resorsTwoTxt;
    [SerializeField] private ResorsSriptableObj resorsSriptableObj;
    private bool k = true;

    private void FixedUpdate()
    {
        if (k)
            StartCoroutine(UpdateResorsOnHUD());
    }

    IEnumerator UpdateResorsOnHUD()
    {
        k = false;
        resorsSriptableObj.ResorsOne += resorsSriptableObj.ForestCountTiles;
        resorsOneTxt.text = resorsSriptableObj.ResorsOne.ToString();
        resorsTwoTxt.text = resorsSriptableObj.ResorsTwo.ToString();
        yield return new WaitForSeconds(1);
        k = true;
    }
}
