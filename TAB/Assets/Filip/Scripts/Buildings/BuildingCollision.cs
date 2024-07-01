using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCollision : MonoBehaviour
{
    [SerializeField] private BoxCollider thisGameObjBoxCollider;
    private PlacementSystem placementSystem;

    void Start()
    {
        placementSystem = GameObject.FindGameObjectWithTag("PlacmentSystem").GetComponent<PlacementSystem>();
    }
    private void OnTriggerEnter(Collider other)
    {
            if (other.CompareTag("Building"))
            {
                placementSystem.isColliding = true;
            }       
    }

    private void OnTriggerExit(Collider other)
    {
            if (other.CompareTag("Building"))
            {
                placementSystem.isColliding = false;
            }       
    }
}
