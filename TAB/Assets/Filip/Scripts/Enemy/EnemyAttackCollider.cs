using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackCollider : MonoBehaviour
{
    public EnemySquadAttack EnemySquadAttack;
    public EnemySquadMovment EnemySquadMovment;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Squad")
        {
            //EnemySquadAttack.isMoving = false;
            EnemySquadMovment.isAttacking = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Squad")
        {
            //SquadLogic.isAttacking = false;
        }
    }
}
