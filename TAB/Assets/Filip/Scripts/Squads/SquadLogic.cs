using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//przypisujemy do ka¿dego sqadu
//bierzemy z t¹d punkty za którymi nale¿y pot¹¿aæ dla jednostki
//przypisujemy odpowiedni Button do danej jednostki
public class SquadLogic : MonoBehaviour
{
    [SerializeField] private float distanceThreshold = 1.0f;
    [SerializeField] private float RotationSpeed = 360f;
    [SerializeField] private float damage = 10f;
    public float hp = 100;
    public GameObject Unite;
    public List<GameObject> ListOfSpowningPoints;
    public List<GameObject> ListOfSpowningPointsToChange;
    public List<GameObject> Unites;
    public float SquadSpeed = 10f;
    public bool SeeEnemy = false;
    public bool isAttacking = false;
    public ResorsSriptableObj resorsSriptableObj;
    public GameObject enemy;
    public List<GameObject> ListOfEnemys;
    public bool isPatroling = false;
    public bool GoBackInPatrol = false;
    public bool isMoving = false;
    public Vector3 PatrolTargetPosition;
    public int CurentUnitesCount;
    public int StartingUnitesCount;
    private NavMeshAgent navMeshAgent;
    private bool whiteWithAttack = true;
    private bool stopPatroling = false;

    void Start()
    {
        CurentUnitesCount = StartingUnitesCount;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = SquadSpeed;
        navMeshAgent.angularSpeed = RotationSpeed;
    }
    private void Update()
    {
        if (isPatroling)
        {
            float distanceToTarget = Vector3.Distance(transform.position, PatrolTargetPosition);
            if (distanceToTarget < distanceThreshold)
            {
                GoBackInPatrol = true;
            }
            else
                GoBackInPatrol = false;
        }
        if (ListOfEnemys.Count != 0 && enemy == null)
        {
            enemy = ListOfEnemys[0];
        }
        if (enemy != null && !isMoving && !isAttacking)
        {
            MoveToDestination(enemy.transform.position);
        }
        else if (isAttacking && whiteWithAttack && !isMoving)
        {
            StartCoroutine(Attack());
        }
        else
        {
            navMeshAgent.speed = SquadSpeed;
        }
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
            {
                isMoving = false;
            }
        }
        if (isPatroling && ListOfEnemys.Count != 0 && stopPatroling)
        {
            MoveToDestination(PatrolTargetPosition);
            stopPatroling = false;
        }
    }

    public void MoveToDestination(Vector3 destination)
    {
        Vector3 direction = destination - transform.position;
        navMeshAgent.SetDestination(destination);      
    }



    private IEnumerator Attack()
    {
        whiteWithAttack = false;
        if (isAttacking && enemy != null)
        {
            navMeshAgent.speed = 0;
            yield return new WaitForSeconds(1);
            enemy.GetComponent<Health>().TakeDamage(damage * Unites.Count);
            SeeEnemy = true;
        }
        else
        {
            navMeshAgent.speed = SquadSpeed;
        }
        whiteWithAttack = true;
        stopPatroling = true;
    }
}
