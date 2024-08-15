using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public SquadLogic SquadLogic;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            SquadLogic.isMoving = false;
            SquadLogic.isAttacking = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            SquadLogic.isAttacking = false;
        }
    }
}
