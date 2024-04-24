using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCollision : MonoBehaviour
{
    private PlacementSystem placementSystem;
    // Start is called before the first frame update
    void Start()
    {
        placementSystem = GameObject.FindGameObjectWithTag("PlacmentSystem").GetComponent<PlacementSystem>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("dzia³a");
        if (other.tag == "Building")
            placementSystem.isColliding = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Building")
            placementSystem.isColliding = false;
    }
}
