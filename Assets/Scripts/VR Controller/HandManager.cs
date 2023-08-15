using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TreeEditor;
using System;

public class HandManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject ray;
    [SerializeField] LayerMask rayCheckObjectLayer;
    [SerializeField] float rayCheckSize;
    [SerializeField] float rayCheckDist;
    [SerializeField] float directCheckDist;

    [SerializeField] float minGripGrab = 0.8f;
    [SerializeField] float throwForce;
    [SerializeField] float objectSmoothSpeed;
    public float grabSpeed = 1.5f;
    public bool isRightHand;

    [SerializeField] Transform oculusController;
    [SerializeField] Transform editorController;
    [HideInInspector] public Transform currentController;

    [HideInInspector] public bool isGrabbing;
    [HideInInspector] public float gripValue;
    [HideInInspector] public float triggerValue;
    [HideInInspector] public bool primaryPressed;
    [HideInInspector] public bool secondaryPressed;

    [HideInInspector] public ObjectManager currentObject;
    [HideInInspector] public int currentObjectGrabIndex;

    [HideInInspector] public Vector3 currentVelocity;
    [HideInInspector] public Vector3 currentAngularVelocity;

    Vector3 prevPos;
    Vector3 prevRot;


    Transform foundObject;

    void Start()
    {
        if (!photonView.IsMine)
        {
            this.enabled = false;
        }

        if (Application.isEditor)
        {
            // Editor
            currentController = editorController;
            oculusController.gameObject.SetActive(false);
        }
        else if (Application.isMobilePlatform)
        {
            // APK
            currentController = oculusController;
            editorController.gameObject.SetActive(false);
        }
        else
        {
            // Windows
            currentController = editorController;
            oculusController.gameObject.SetActive(false);
        }
        ray = currentController.GetChild(0).GetChild(0).gameObject;
        ray.gameObject.SetActive(false);
    }

    void Update()
    {
        GetValues();

        CheckGrab();

        if (isGrabbing)
        {
            SetObject(currentObject.photonView.ViewID);
            photonView.RPC("SetObject", RpcTarget.Others, currentObject.photonView.ViewID);
        }
        else
        {
            SetObject(-1);
            photonView.RPC("SetObject", RpcTarget.Others, -1);
        }
    }

    void FixedUpdate()
    {
        GetVelocity();    
    }

    void CheckGrab()
    {
        if (!isGrabbing)
        {
            if (gripValue > minGripGrab)
            {
                // CURRENTLY DISABLED AS OF TESTING NEW GRAB SYSTEM
                /*if (triggerValue > minGripGrab && Physics.SphereCast(ray.transform.position, rayCheckSize, ray.transform.forward, out RaycastHit objectRay, rayCheckDist, rayCheckObjectLayer))
                {
                    if (objectRay.transform.TryGetComponent<ObjectManager>(out ObjectManager objectManager))
                    {
                        if (objectManager.firstHand == null && !objectManager.isConstrained)
                        {
                            // If not magazine or can grab magazine from current gun.
                            if (!objectManager.TryGetComponent<Magazine>(out Magazine magazine) || magazine.canGrabFromGun || magazine.currentGun == null)
                            {
                                // When grabbing with second hand, no need for trigger press. So only first hand check
                                isGrabbing = true;
                                objectManager.GrabObject(this, true, grabSpeed, objectSmoothSpeed);
                            }
                        }
                    }
                }*/       
                
                if (foundObject != null && foundObject.TryGetComponent<ObjectManager>(out ObjectManager objectManager))
                {
                    // Check if hand can grab and object can be grabbed.
                    if (!isGrabbing)
                    {
                        if (objectManager.hands[0] == null)
                        {
                            // If grab point 1 is also null, check which is closest.
                            if (objectManager.hands[1] == null && Vector3.Distance(transform.position, objectManager.grabPoints[1].transform.position) < Vector3.Distance(transform.position, objectManager.grabPoints[0].transform.position))
                            {
                                objectManager.isGrabbed = true;
                                objectManager.hands[1] = this;
                                currentObjectGrabIndex = 1;
                                currentObject = objectManager;
                                isGrabbing = true;
                            }
                            else
                            {
                                objectManager.isGrabbed = true;
                                objectManager.hands[0] = this;
                                currentObjectGrabIndex = 0;
                                currentObject = objectManager;
                                isGrabbing = true;
                            }
                        }
                        else if (objectManager.hands[1] == null)
                        {
                            // Already checked for grab point 0 so no need to distance check.
                            objectManager.isGrabbed = true;
                            objectManager.hands[1] = this;
                            currentObjectGrabIndex = 1;
                            currentObject = objectManager;
                            isGrabbing = true;
                        }
                    }
                }
            }
        }
    }

    void GetValues()
    {
        if (isRightHand)
        {
            gripValue = PlayerInput.rightHandGrip;
            triggerValue = PlayerInput.rightHandTrigger;
            primaryPressed = PlayerInput.rightHandPrimary;
            secondaryPressed = PlayerInput.rightHandSecondary;
        }
        else
        {
            gripValue = PlayerInput.leftHandGrip;
            triggerValue = PlayerInput.leftHandTrigger;
            primaryPressed = PlayerInput.leftHandPrimary;
            secondaryPressed = PlayerInput.leftHandSecondary;
        }

        if (gripValue < minGripGrab)
        {
            isGrabbing = false;
        }
        if (ray.activeSelf == true)
        {
            if (isGrabbing || gripValue < minGripGrab)
            {
                ray.SetActive(false);
            }
        }
        else
        {
            if (!isGrabbing && gripValue > minGripGrab)
            {
                ray.SetActive(true);
            }
        }
        photonView.RPC("SyncValues", RpcTarget.Others, ray.activeSelf, gripValue, triggerValue, primaryPressed, secondaryPressed);
    }

    [PunRPC]
    void SetObject(int _currentObjectId)
    {
        // Get currentObject for all other clients.
        if (PhotonView.Find(_currentObjectId) != null)
        {
            currentObject = PhotonView.Find(_currentObjectId).GetComponent<ObjectManager>();

            currentObject.hands[currentObjectGrabIndex] = this;
            currentObject.isGrabbed = true;
        }
        else
        {
            if (currentObject != null)
            {
                currentObject.hands[currentObjectGrabIndex] = null;
                currentObject.lastGrabbedHand = this;
                currentObject = null;
            }
        }
    }

    void GetVelocity()
    {
        currentVelocity = (transform.position - prevPos) / Time.fixedDeltaTime;
        prevPos = transform.position;

        currentAngularVelocity = (transform.eulerAngles - prevRot) / Time.fixedDeltaTime;
        prevRot = transform.eulerAngles;
    }

    [PunRPC]
    void SyncValues(bool _rayEnabled, float _gripValue, float _triggerValue, bool _primaryPressed, bool _secondaryPressed, Vector3 _currentVelocity, Vector3 _currentAngularVelocity)
    {
        ray.SetActive(_rayEnabled);
        gripValue = _gripValue;
        triggerValue = _triggerValue;
        primaryPressed = _primaryPressed;
        secondaryPressed = _primaryPressed;
        currentVelocity = _currentVelocity;
        currentAngularVelocity = _currentAngularVelocity;
    }

    void OnTriggerStay(Collider _collider)
    {
        // Object Layer == 8.
        if (_collider.gameObject.layer == 8 && foundObject == null)
        {
            foundObject = _collider.transform;
        }
    }

    void OnTriggerExit(Collider _collider)
    {
        foundObject = null;
    }
}