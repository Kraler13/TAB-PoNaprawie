using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResorsGathering : MonoBehaviour
{
    [SerializeField] private ResorsSriptableObj resorsSriptableObj;
    [SerializeField] private PlacementSystem placementSystem;
    public BoxCollider boxCollider;
    public Rigidbody rb;
    public bool stoneBuilding;
    public bool forestBuilding;

    private void Start()
    {
        placementSystem = GameObject.FindGameObjectWithTag("PlacmentSystem").GetComponent<PlacementSystem>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ForestBuildingRange")
        {
            placementSystem.isColliding = true;
        }
        if (other.tag == "Forest" && forestBuilding)
        {
            resorsSriptableObj.ForestCountTilesToAdd++;
            resorsSriptableObj.boxCollidersToDestroy.Add(other.GetComponent<BoxCollider>());
        }

        if (other.tag == "Stone" && stoneBuilding)
        {
            resorsSriptableObj.StoneCountTilesToAdd++;
            resorsSriptableObj.boxCollidersToDestroy.Add(other.GetComponent<BoxCollider>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Forest" && forestBuilding)
        {
            resorsSriptableObj.ForestCountTilesToAdd--;
            resorsSriptableObj.boxCollidersToDestroy.Remove(other.GetComponent<BoxCollider>());
        }

        if (other.tag == "Stone" && stoneBuilding)
        {
            resorsSriptableObj.StoneCountTilesToAdd--;
            resorsSriptableObj.boxCollidersToDestroy.Remove(other.GetComponent<BoxCollider>());
        }
    }  
}
