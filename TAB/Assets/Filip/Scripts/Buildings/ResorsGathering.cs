using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResorsGathering : MonoBehaviour
{
    [SerializeField] private ResorsSriptableObj resorsSriptableObj;
    [SerializeField] private BoxCollider boxCollider;
    public bool stoneBuilding;
    public bool forestBuilding;

    private void OnTriggerEnter(Collider other)
    {
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
