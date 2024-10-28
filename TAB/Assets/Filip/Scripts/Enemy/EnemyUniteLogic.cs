using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyUniteLogic : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public GameObject pointToFollow;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        navMeshAgent.SetDestination(pointToFollow.transform.position);
    }
}
