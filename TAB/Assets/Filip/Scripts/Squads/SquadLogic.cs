using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//przypisujemy do ka�dego sqadu
//bierzemy z t�d punkty za kt�rymi nale�y pot��a� dla jednostki
//przypisujemy odpowiedni Button do danej jednostki
public class SquadLogic : MonoBehaviour
{
    [SerializeField] private LayerMask ground;
    [SerializeField] private float distanceThreshold = 1.0f;
    [SerializeField] private float RotationSpeed = 360f;
    public float hp = 100;
    public GameObject Unite;
    public List<GameObject> ListOfSpowningPoints;
    public List<GameObject> ListOfSpowningPointsToChange;
    public List<GameObject> Unites;
    public float SquadSpeed = 10f;
    public bool IsAttacking = false;
    public bool InRangeAttack = false;
    public ResorsSriptableObj resorsSriptableObj;
    public GameObject enemy;
    public List<GameObject> ListOfEnemys;
    public bool isPatroling = false;
    public bool GoBackInPatrol = false;
    public Vector3 PatrolTargetPosition;
    public int CurentUnitesCount;
    public int StartingUnitesCount;
    private NavMeshAgent navMeshAgent;

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
        if (enemy != null)
        {
            MoveToDestination(enemy.transform.position);
        }
        if (IsAttacking)
            StartCoroutine(Attack());

    }

    public void MoveToDestination(Vector3 destination)
    {
        Vector3 direction = destination - transform.position;
        navMeshAgent.SetDestination(destination);
    }



    private IEnumerator Attack()
    {
        Debug.Log("atack2");
        

        IsAttacking = false;
        yield return new WaitForSeconds(1);
        if (InRangeAttack && enemy != null)
        {
            Debug.Log("atack3");

            navMeshAgent.speed = 0;
            enemy.GetComponent<Health>().TakeDamage(1);
            IsAttacking = true;
        }
        else if (!InRangeAttack)
        {
            IsAttacking = false;
        }
        else
            navMeshAgent.speed = SquadSpeed;

    }
}
