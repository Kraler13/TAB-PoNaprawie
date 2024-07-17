using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniteHealth : MonoBehaviour
{
    public SquadLogic squadLogic;
    public float CurrentUniteHP;

    public void TakeDamage(float damage)
    {
        CurrentUniteHP -= damage;
        if (CurrentUniteHP < 0)
        {
            int index = squadLogic.Unites.IndexOf(gameObject);
            squadLogic.Unites.RemoveAt(index);
            squadLogic.CurentUnitesCount--;
            Destroy(gameObject);
        }
    }
}
