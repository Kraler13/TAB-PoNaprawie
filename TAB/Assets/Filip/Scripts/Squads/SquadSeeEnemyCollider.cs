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
            SquadLogic.AddEnemy(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            SquadLogic.RemoveEnemy(other.gameObject);
        }
    }
}
