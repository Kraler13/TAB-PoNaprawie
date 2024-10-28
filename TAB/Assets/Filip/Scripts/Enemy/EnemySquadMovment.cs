using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySquadMovment : MonoBehaviour
{
    [SerializeField] private float wanderRadius = 10f;
    [SerializeField] private float stoppingDistance = 1f;
    [SerializeField] private float howLongMoveToTheCenter = 2f;
    [SerializeField] private int nextCenterFrameCounter = 500;
    [SerializeField] private Vector3 mapCenter;

    public bool isAttacking = false;

    private NavMeshAgent agent;
    private Vector3 startPoint;
    private float frameCounter;
    private bool isMovingToCenter = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        startPoint = transform.position;
    }
    void Update()
    {
        if (!isAttacking)
        {
            if (frameCounter == nextCenterFrameCounter)
            {
                StartCoroutine(MoveToCenter());
            }
            if (!agent.pathPending && agent.remainingDistance <= stoppingDistance && !isMovingToCenter)
            {
                MoveToRandomPoint();
            }
        }
    }
    private void FixedUpdate()
    {
        frameCounter++;
    }
    void MoveToRandomPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection.y = 0;
        Vector3 targetPosition = startPoint + randomDirection;

        NavMeshHit navHit;
        if (NavMesh.SamplePosition(targetPosition, out navHit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(navHit.position);
        }
    }

    IEnumerator MoveToCenter()
    {

        isMovingToCenter = true;
        agent.SetDestination(mapCenter);
        yield return new WaitForSeconds(howLongMoveToTheCenter);
        isMovingToCenter = false;
        MoveToRandomPoint();
        frameCounter = 0;
        Debug.Log(frameCounter);

    }


}