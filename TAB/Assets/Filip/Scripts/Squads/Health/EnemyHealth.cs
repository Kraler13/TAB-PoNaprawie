using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [SerializeField] private List<SquadLogic> squadsAttacking;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AttackCollider")
        {
            squadsAttacking.Add(other.gameObject.GetComponent<AttackCollider>().SquadLogic);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "AttackCollider")
        {
            squadsAttacking.Remove(other.gameObject.GetComponent<AttackCollider>().SquadLogic);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health < 0)
        {
            foreach (var squad in squadsAttacking)
            {
                if(squad != null)
                {
                    squad.ListOfEnemys.Remove(gameObject);
                }
            }
            Destroy(gameObject);
        }
    }
}
