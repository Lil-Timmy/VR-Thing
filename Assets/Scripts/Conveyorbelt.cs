using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyorbelt : MonoBehaviour
{
    [SerializeField] float conveyorForce;
    
    void OnTriggerStay(Collider collider)
    {
        if (collider.CompareTag("Object") && transform.parent.gameObject.layer != 0)
        {
            collider.transform.position += (transform.forward * conveyorForce);
        }
    }
}
