using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySquadHealth : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [SerializeField] private List<SquadLogic> squadsAttacking;
    public List<GameObject> Unites;

    public float maxUniteHP;
    public float CurrentSquadHP;
    public float maxSquadHealth;


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
        int r = Random.Range(0, Unites.Count);
        Unites[r].GetComponent<UniteHealth>().TakeDamage(damage);

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
