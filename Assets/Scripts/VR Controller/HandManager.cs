using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HandManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform rayTransform;
    [SerializeField] MeshRenderer rayMeshRender;
    [SerializeField] LayerMask rayCheckObjectLayer;
    [SerializeField] float rayCheckSize;
    [SerializeField] float rayCheckDist;
    [SerializeField] float directCheckDist;

    [SerializeField] float minGripGrab = 0.8f;
    [SerializeField] float throwForce;
    [SerializeField] float objectSmoothSpeed;
    public float grabSpeed = 1.5f;
    public bool isRightHand;

    [HideInInspector] public bool isGrabbing;
    float gripValue;
    float triggerValue;
    
    Transform foundObject;

    void Start()
    {
        rayMeshRender.enabled = false;
        if (!photonView.IsMine)
        {
            this.enabled = false;
        }
    }

    void Update()
    {
        GetValues();

        CheckGrab();
    }

    void CheckGrab()
    {
        if (!isGrabbing)
        {
            if (gripValue > minGripGrab)
            {
                if (triggerValue > minGripGrab && Physics.SphereCast(rayTransform.position, rayCheckSize, rayTransform.forward, out RaycastHit objectRay, rayCheckDist, rayCheckObjectLayer))
                {
                    if (objectRay.transform.TryGetComponent<ObjectManager>(out ObjectManager objectManager))
                    {
                        if (objectManager.firstHand == null)
                        {
                            //When grabbing with second hand, no need for trigger press. So only first hand check
                            isGrabbing = true;
                            objectManager.GrabObject(this, true, grabSpeed, objectSmoothSpeed);
                        }
                    }
                }       
                else if (foundObject != null && foundObject.TryGetComponent<ObjectManager>(out ObjectManager objectManager))
                {
                    if (objectManager.firstHand == null || objectManager.secondHand == null && objectManager.firstHand.photonView.IsMine)
                    {
                        //Check for just grip when directly touching the object.
                        isGrabbing = true;
                        objectManager.GrabObject(this, false, grabSpeed, objectSmoothSpeed);
                    }
                }
            }
        }

        if (gripValue < minGripGrab)
        {
            isGrabbing = false;
        }
        if (rayMeshRender.enabled == true)
        {
            if (isGrabbing || gripValue < minGripGrab)
            {
                rayMeshRender.enabled = false;
                photonView.RPC("SyncRay", RpcTarget.Others, rayMeshRender.enabled);
            }
        }
        else
        {
            if (!isGrabbing && gripValue > minGripGrab)
            {
                rayMeshRender.enabled = true;
                photonView.RPC("SyncRay", RpcTarget.Others, rayMeshRender.enabled);
            }
        }
    }

    void GetValues()
    {
        if (isRightHand)
        {
            gripValue = PlayerInput.rightHandGrip;
            triggerValue = PlayerInput.rightHandTrigger;
        }
        else
        {
            gripValue = PlayerInput.leftHandGrip;
            triggerValue = PlayerInput.leftHandTrigger;
        }
    }

    [PunRPC]
    void SyncRay(bool _rayEnabled)
    {
        rayMeshRender.enabled = _rayEnabled;
    }

    void OnTriggerStay(Collider _collider)
    {
        //Object Layer == 8
        if (_collider.gameObject.layer == 8)
        {
            foundObject = _collider.transform;
        }
    }

    void OnTriggerExit(Collider _collider)
    {
        foundObject = null;
    }
}