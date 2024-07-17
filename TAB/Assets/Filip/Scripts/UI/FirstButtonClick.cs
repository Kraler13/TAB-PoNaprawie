using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstButtonClick : MonoBehaviour
{
    [SerializeField] private SquadSelection squadSelection;
    private bool wasClicked = false;
    void FixedUpdate()
    {
        if (squadSelection.buttonList.Count != 0 && !wasClicked)
        {
            transform.GetChild(0).GetComponent<Button>().onClick.Invoke();
            wasClicked = true;
        }
        else if (squadSelection.buttonList.Count == 0)
        {
            wasClicked = false;
        }
    }
}
