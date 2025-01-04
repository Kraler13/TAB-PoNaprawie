using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySquadHealth : MonoBehaviour
{
    public event Action<GameObject> OnEnemyDestroyed;

    [SerializeField] private float health = 100f;
    [SerializeField] private List<SquadLogic> squadsAttacking;
    public List<GameObject> Unites;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            OnEnemyDestroyed?.Invoke(gameObject);
            Destroy(gameObject);
        }
    }
}
