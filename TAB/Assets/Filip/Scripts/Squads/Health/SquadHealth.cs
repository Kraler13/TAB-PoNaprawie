using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadHealth : MonoBehaviour
{
    public float maxUniteHP;
    public float CurrentSquadHP;
    public float maxSquadHealth;
    private SquadLogic squadLogic;
    private int currentUniteCount;
    private bool squadIsAttacked;
    private bool k = false;
    void Start()
    {
        squadLogic = GetComponent<SquadLogic>();
        CurrentSquadHP = squadLogic.CurentUnitesCount * maxUniteHP;
        maxSquadHealth = CurrentSquadHP;
    }

    void LateUpdate()
    {
        if (currentUniteCount != squadLogic.CurentUnitesCount)
        {
            currentUniteCount = squadLogic.CurentUnitesCount ;
            maxSquadHealth = currentUniteCount * maxUniteHP;         
        }

        if (Input.GetKeyDown(KeyCode.Space) && !k)
        {
            TakeDemage(101);
        }
    }

    public void TakeDemage(float damage)
    {
        k = true;       
        int r = Random.Range(0, squadLogic.Unites.Count);
        squadLogic.Unites[r].GetComponent<UniteHealth>().TakeDamage(damage);
        CurrentSquadHP = 0;
        foreach (var unite in squadLogic.Unites)
        {
            CurrentSquadHP += unite.GetComponent<UniteHealth>().CurrentUniteHP;
            Debug.Log(CurrentSquadHP);
        }
        Debug.Log(CurrentSquadHP);

        if (CurrentSquadHP <= 10)
        {
            Debug.Log("dzi¹³a");
            Destroy(gameObject);
            return;
        }
        StartCoroutine(ForTest());
    }

    private IEnumerator ForTest()
    {
        yield return new WaitForSeconds(2f);
        k = false;
        Debug.Log("JUSZ");
    }
}
