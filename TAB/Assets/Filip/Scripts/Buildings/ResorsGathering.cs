using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResorsGathering : MonoBehaviour
{
    [SerializeField] private ResorsSriptableObj resorsSriptableObj;
    [SerializeField] private bool stoneBuilding;
    [SerializeField] private float startingBoxColliderSize;
    [SerializeField] private float endingBoxColliderSize;
    [SerializeField] private BoxCollider boxCollider;
    public bool forestBuilding;
    private int forestTiles;

        private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Forest" && forestBuilding)
        {
            resorsSriptableObj.ForestCountTilesToAdd++;
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
    }
}
