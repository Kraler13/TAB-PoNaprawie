using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResorsGatheringRange : MonoBehaviour
{
    [SerializeField] private float minDistanceFromBuilding = 9f;
    public bool isValid;
    public GameObject PrevievObj;

    private void Update()
    {
        if (PrevievObj != null)
        {
            float distance = Vector3.Distance(gameObject.transform.position, PrevievObj.transform.position);
            if (minDistanceFromBuilding < distance)
            {
                isValid = true;
                Debug.Log("true");
            }
            else
            {
                isValid = false;
                Debug.Log("false");
            }
        }

    }
}
