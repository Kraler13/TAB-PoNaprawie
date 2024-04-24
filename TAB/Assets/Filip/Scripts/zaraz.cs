using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zaraz : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Cube")
            Debug.Log("dzia³a");
    }
}
