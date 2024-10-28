using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUniteHealth : MonoBehaviour
{
    public EnemySquadHealth EnemySquadHealth;
    public float CurrentUniteHP;

    public void TakeDamage(float damage)
    {
        CurrentUniteHP -= damage;
        if (CurrentUniteHP < 0)
        {
            int index = EnemySquadHealth.Unites.IndexOf(gameObject);
            EnemySquadHealth.Unites.RemoveAt(index);
            Destroy(gameObject);
        }
    }
}
