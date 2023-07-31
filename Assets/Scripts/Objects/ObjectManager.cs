using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ObjectManager : MonoBehaviourPun
{
    [SerializeField] Vector3 oneHandGrabOffset = new Vector3(210, 180, 180);

    [SerializeField] bool tracksecondHandRotation;


    public bool isTwoHands;
    [HideInInspector] public bool isGrabbed;
    [HideInInspector] public HandManager firstHand;
    [HideInInspector] public HandManager secondHand;

    Rigidbody objectRb;
    float objectGrabSpeed;
    float objectSmoothSpeed;

    void Awake()
    {
        objectRb = GetComponent<Rigidbody>();
    }

    public void GrabObject(HandManager _handGrabbedWith, bool _pullObject, float _grabSpeed, float _smoothSpeed)
    {
        objectGrabSpeed = _grabSpeed;
        objectSmoothSpeed = _smoothSpeed;
        objectRb.isKinematic = true;

        if (!isTwoHands || firstHand == null)
        {
            firstHand = _handGrabbedWith;
            photonView.TransferOwnership(firstHand.photonView.Owner);
            // If first hand & distance grab.
            if (_pullObject)
            {
                StartCoroutine(PullObject());
            }
            else
            {
                isGrabbed = true;
            }
        }
        else
        {
            // Set to two hand grabbing and just swap to it in LateUpdate.
            secondHand = _handGrabbedWith;
        }
    }

    IEnumerator PullObject()
    {
        //Only occurs if it's the first hand to grab the object.
        float currentTime = 0f;
        float startingTime = Time.time;
        Vector3 startingPosition = transform.position;
        Quaternion startingRotation = transform.rotation;
        while (firstHand.isGrabbing && currentTime < 1)
        {
            float timeMultiplier = Time.time - startingTime;

            currentTime = Mathf.Clamp01(timeMultiplier * timeMultiplier * objectGrabSpeed);
            transform.position = Vector3.Lerp(startingPosition, firstHand.transform.position, currentTime);
            transform.rotation = Quaternion.Lerp(startingRotation, firstHand.transform.rotation, currentTime);
            photonView.RPC("SyncObject", RpcTarget.Others, transform.position, transform.rotation, firstHand.photonView.ViewID, -1);

            yield return new WaitForFixedUpdate();
        }
        prevPos = firstHand.transform.position;
        prevRot = firstHand.transform.eulerAngles;
        isGrabbed = true;
    }

    Vector3 prevPos;
    Vector3 prevRot;
    void LateUpdate()
    {
        if (isGrabbed)
        {
            if (secondHand != null)
            {
                TwohandGrab();
            }
            else if (firstHand != null)
            {
                OneHandGrab();
            }
        }
    }

    void OneHandGrab()
    {
        if (firstHand.isGrabbing)
        {
            transform.position = firstHand.transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, firstHand.transform.rotation * Quaternion.Euler(oneHandGrabOffset), Time.deltaTime * objectSmoothSpeed);
            prevPos = firstHand.transform.position;
            prevRot = firstHand.transform.eulerAngles;
            photonView.RPC("SyncObject", RpcTarget.Others, transform.position, transform.rotation, firstHand.photonView.ViewID, -1);
        }
        else
        {
            // Let go of object.

            objectRb.isKinematic = false;
            objectRb.angularVelocity = (firstHand.transform.eulerAngles - prevRot) * 0.0175f * Time.deltaTime / Time.fixedDeltaTime;
            objectRb.velocity = (firstHand.transform.position - prevPos) * Time.deltaTime / (Time.fixedDeltaTime * Time.deltaTime * 1.4f);
            isGrabbed = false;
            firstHand = null;
            photonView.RPC("SyncRelease", RpcTarget.Others, objectRb.velocity, objectRb.angularVelocity);
        }
    }
    void TwohandGrab()
    {
        if (firstHand.isGrabbing && secondHand.isGrabbing)
        {
            transform.position = firstHand.transform.position;

            // Second hand look towards grabbing.
            Quaternion lookRotation = Quaternion.LookRotation(secondHand.transform.position - transform.position);
            Vector3 gripRotation = new Vector3(0f, 0f, (secondHand.transform.eulerAngles.z - firstHand.transform.eulerAngles.z) * 0.5f + firstHand.transform.eulerAngles.z);
            float firstHandRot = firstHand.transform.eulerAngles.z;
            float secondHandRot = secondHand.transform.eulerAngles.z;
            if (firstHandRot > 180 && secondHandRot < 180 || firstHandRot < 180 && secondHandRot > 180)
            {
                gripRotation.z += 180;
            }
            lookRotation *= Quaternion.Euler(gripRotation);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * objectSmoothSpeed);
            photonView.RPC("SyncObject", RpcTarget.Others, transform.position, transform.rotation, firstHand.photonView.ViewID, secondHand.photonView.ViewID);
        }
        else
        {
            // Swap second to first hand or change to one hand grabbing.
            if (!firstHand.isGrabbing)
            {
                firstHand = secondHand;
                secondHand = null;
            }
            else if (!secondHand.isGrabbing)
            {
                secondHand = null;
            }
        }
    }

    [PunRPC]
    void SyncObject(Vector3 _objectPos, Quaternion _objectRot, int _firstHandId, int _secondHandId)
    {
        objectRb.isKinematic = true;
        transform.position = _objectPos;
        transform.rotation = _objectRot;

        firstHand = PhotonView.Find(_firstHandId).GetComponent<HandManager>();
        if (_secondHandId != -1)
        {
            secondHand = PhotonView.Find(_secondHandId).GetComponent<HandManager>();
        }
        else
        {
            secondHand = null;
        }

        if (photonView.Owner != firstHand.photonView.Owner)
        {
            photonView.TransferOwnership(firstHand.photonView.Owner);
        }
    }

    [PunRPC]
    void SyncRelease(Vector3 _velocity, Vector3 _angularVelocity)
    {
        objectRb.isKinematic = false;
        objectRb.velocity = _velocity;
        objectRb.angularVelocity = _angularVelocity;

        firstHand = null;
    }
}