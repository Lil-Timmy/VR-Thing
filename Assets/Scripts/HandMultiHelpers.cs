using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HandMultiHelpers : MonoBehaviourPunCallbacks
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

    void Start()
    {
        if (!photonView.IsMine)
        {
            this.enabled = false;
        }
    }
    
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
                photonView.RPC("GrabObject", RpcTarget.All, possibleObject.GetComponent<PhotonView>().ViewID, possibleObject.transform.position, possibleObject.transform.rotation);
            }
        }
        else if (currentObject != null)
        {
            photonView.RPC("ReleaseObject", RpcTarget.All, (transform.position - prevHandPos) * throwForce * Time.deltaTime, currentObject.transform.position, currentObject.transform.rotation);
        }

        prevHandPos = transform.position;
    }

    [PunRPC]
    void GrabObject(int grabbedViewId, Vector3 objPos, Quaternion objRot)
    {
        currentObject = PhotonNetwork.GetPhotonView(grabbedViewId).gameObject;
        currentObject.layer = 0; // 0 = Un-used Layer For No Collisions
        currentObject.GetComponent<Rigidbody>().isKinematic = true;
        currentObject.transform.position = objPos;
        currentObject.transform.rotation = objRot;
        currentObject.transform.parent = transform;
    }

    [PunRPC]
    void ReleaseObject(Vector3 force, Vector3 objPos, Quaternion objRot)
    {
        currentObject.layer = 8; // 8 == Object Layer For Collisions
        currentObject.GetComponent<Rigidbody>().isKinematic = false;
        currentObject.GetComponent<Rigidbody>().velocity += force;
        currentObject.transform.position = objPos;
        currentObject.transform.rotation = objRot;
        currentObject.transform.parent = null;
        currentObject = null;
    }
}
