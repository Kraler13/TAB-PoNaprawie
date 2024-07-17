using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadSeeEnemyCollider : MonoBehaviour
{
    [SerializeField] private SquadLogic SquadLogic;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            SquadLogic.SeeEnemy = true;
            SquadLogic.ListOfEnemys.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (SquadLogic.enemy.name == other.name)
            {
                SquadLogic.enemy = null;
            }
            SquadLogic.ListOfEnemys.Remove(other.gameObject);
        }
    }
}
