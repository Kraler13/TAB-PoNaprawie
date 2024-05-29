using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildInRange : MonoBehaviour
{
    public bool isValid;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Building")
        {          
            isValid = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Building")
        {
            isValid = false;
        }
    }   
}
