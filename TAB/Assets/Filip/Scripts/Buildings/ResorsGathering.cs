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
    private int forestTiles;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Forest" && forestBuilding)
        {
            resorsSriptableObj.ForestCountTilesToAdd++;
            Debug.Log(forestTiles);
        }

        if (other.tag == "Stone" && stoneBuilding)
        {
            resorsSriptableObj.StoneCountTiles++;
            Debug.Log(forestTiles);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Forest" && forestBuilding)
        {
            resorsSriptableObj.ForestCountTilesToAdd--;
            Debug.Log(forestTiles);
        }

        if (other.tag == "Stone" && stoneBuilding)
        {
            resorsSriptableObj.StoneCountTiles--;
            Debug.Log(forestTiles);
        }
    }
}
