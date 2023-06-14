using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandHelpers : MonoBehaviour
{
    [SerializeField] bool rightHand;
    [SerializeField] float throwForce;
    GameObject possibleObject;
    GameObject currentObject;
    Material objectMaterial;

    Material[] childMats;

    Vector3 startPosOffset;
    Quaternion startRotOffset;

    Vector3 prevHandPos;
    
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Object") && collider.gameObject.layer == 8)
        {
            possibleObject = collider.gameObject;
        }
    }
    void OnTriggerExit(Collider collider)
    {
        possibleObject = null;
    }

    void Update()
    {
        float gripAmount;
        if (rightHand)
        {
            gripAmount = PlayerInput.rightHandGrip;
        }
        else
        {
            gripAmount = PlayerInput.leftHandGrip;
        }
        if (gripAmount > 0.6f)
        {
            if (currentObject == null && possibleObject != null)
            {
                currentObject = possibleObject.gameObject;
                currentObject.layer = 0; // 0 = Un-used Layer For No Collisions
                currentObject.GetComponent<Rigidbody>().isKinematic = true;
                currentObject.transform.parent = transform.parent;
            }
        }
        else if (currentObject != null)
        {
            currentObject.layer = 8; // 8 == Object Layer For Collisions
            currentObject.GetComponent<Rigidbody>().isKinematic = false;
            currentObject.GetComponent<Rigidbody>().AddForce((transform.parent.position - prevHandPos) * throwForce);
            currentObject.transform.parent = null;
            currentObject = null;
        }
    }
    
    void FixedUpdate()
    {
        prevHandPos = transform.parent.position;
    }
}
