using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCollision : MonoBehaviour
{
    private PlacementSystem placementSystem;
    void Start()
    {
        placementSystem = GameObject.FindGameObjectWithTag("PlacmentSystem").GetComponent<PlacementSystem>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Building")
            placementSystem.isColliding = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Building")
            placementSystem.isColliding = false;
    }
}
