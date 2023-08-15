using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ObjectManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Quaternion oneHandRotationOffset = Quaternion.Euler(295f, 180f, 0f);
    [SerializeField] Vector3 oneHandPositionOffset = new Vector3(0f, 0.01f, 0f);
    [SerializeField] Quaternion twoHandRotationOffset = Quaternion.Euler(0f, 180f, 0f);

    public Transform[] grabPoints;
    public bool isGrabbed;
    public HandManager[] hands;
    public HandManager lastGrabbedHand;

    Rigidbody objectRb;

    void Awake()
    {
        // Object layer == 8.
        gameObject.layer = 8;

        objectRb = GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        // If there is at least one hand, update the object to move there, than any additional hands will be calculated too.
        if (isGrabbed)
        {
            Vector3 prevPos = transform.position;
            Quaternion prevRot = transform.rotation;

            ObjectGrabbed();

            transform.position = Vector3.Lerp(prevPos, transform.position, Time.deltaTime / Time.fixedDeltaTime * 0.5f);
            transform.rotation = Quaternion.Lerp(prevRot, transform.rotation, Time.deltaTime / Time.fixedDeltaTime * 0.3f);
        }

        SetPhysics();
    }

    void ObjectGrabbed()
    {
        // Check which hands to use. (both, first, or second)
        if (hands[0] != null)
        {
            // First AND Second
            if (hands[1] != null)
            {
                TwohandGrab();
            }
            // ONLY First
            else
            {
                OneHandGrab(0);
            }
        }
        // ONLY Second
        else if (hands[1] != null && hands[1].currentObject)
        {
            OneHandGrab(1);
        }
        else
        {
            isGrabbed = false;
            hands[0] = null;
            hands[1] = null;
        }
    }

    void OneHandGrab(int _handPoint)
    {
        Vector3 grabOffset = oneHandPositionOffset.z * Vector3.forward + oneHandPositionOffset.x * Vector3.right + oneHandPositionOffset.y * Vector3.up;
        transform.position = hands[_handPoint].transform.position + transform.position - grabPoints[_handPoint].position + grabOffset;
        transform.rotation = hands[_handPoint].transform.rotation * oneHandRotationOffset;
    }

    void TwohandGrab()
    {
        transform.position = hands[0].transform.position + transform.position - grabPoints[0].position;

        // Second hand look towards grabbing.
        Quaternion lookRotation = Quaternion.LookRotation(hands[1].currentController.position - transform.position, Vector3.up);
        Vector3 gripRotation = new Vector3(0f, 0f, (hands[1].currentController.eulerAngles.z - hands[0].currentController.eulerAngles.z) * 0.5f + hands[0].currentController.eulerAngles.z);
        float firstHandRot = hands[0].currentController.eulerAngles.z;
        float secondHandRot = hands[1].currentController.eulerAngles.z;
        if (firstHandRot > 180 && secondHandRot < 180 || firstHandRot < 180 && secondHandRot > 180)
        {
            gripRotation.z += 180;
        }
        lookRotation *= Quaternion.Euler(gripRotation) * twoHandRotationOffset;
        transform.rotation = lookRotation;
    }

    [PunRPC]
    void SetPhysics()
    {
        // Changes the rigidbody kinematic to on or off, and adds a force when let go.
        if (isGrabbed)
        {
            objectRb.isKinematic = true;
        }
        else
        {
            if (objectRb.isKinematic)
            {
                objectRb.isKinematic = false;

                if (lastGrabbedHand != null)
                {
                    objectRb.velocity = lastGrabbedHand.currentVelocity;
                    objectRb.angularVelocity = lastGrabbedHand.currentAngularVelocity;
                    lastGrabbedHand = null;
                }
            }
        }
    }
}