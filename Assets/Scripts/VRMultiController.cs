using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class VRMultiController : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform headset;
    [SerializeField] Transform rightHand;
    [SerializeField] Transform leftHand;


    [SerializeField] Rigidbody bodyRb;
    [SerializeField] CapsuleCollider bodyCapsule;

    [SerializeField] Transform orientation;

    [SerializeField] float movementSpeed;
    
    void Start()
    {
        if (!photonView.IsMine)
        {
            headset.GetComponent<Camera>().enabled = false;
            headset.GetComponent<AudioListener>().enabled = false;
            this.enabled = false;
        }
    }
    
    void Update()
    {
        Movement();

        Limbs();
    }

    void Movement()
    {
        orientation.rotation = Quaternion.Euler(0f, headset.rotation.eulerAngles.y, 0f);
        Vector2 movement = PlayerInput.leftHandJoystick * movementSpeed;
        Vector3 moveVector = (orientation.forward * movement.y + orientation.right * movement.x);
        Vector3 headPos = new Vector3(PlayerInput.headPosition.x, PlayerInput.headPosition.y, PlayerInput.headPosition.z);
        bodyRb.AddForce(moveVector * Time.deltaTime, ForceMode.Impulse);
        bodyRb.velocity *= 0.9f;

        float height = Mathf.Clamp(PlayerInput.headPosition.y, bodyCapsule.radius, 999f);
        Vector3 center = new Vector3(headset.localPosition.x, bodyCapsule.height / 2, headset.localPosition.z);

        photonView.RPC("SetHeight", RpcTarget.All, height, center);
    }

    [PunRPC]
    void SetHeight(float height, Vector3 center)
    {
        bodyCapsule.height = height;
        bodyCapsule.center = center;
    }

    void Limbs()
    {
        Vector3 headsetPos = PlayerInput.headPosition;
        Quaternion headsetRot = PlayerInput.headRotation;

        Vector3 rightHandPos = PlayerInput.rightHandPosition;
        Quaternion rightHandRot = PlayerInput.rightHandRotation;

        Vector3 leftHandPos = PlayerInput.leftHandPosition;
        Quaternion leftHandRot = PlayerInput.leftHandRotation;

        headset.localPosition = headsetPos;
        headset.localRotation = headsetRot;

        rightHand.localPosition = rightHandPos;
        rightHand.localRotation = rightHandRot;

        leftHand.localPosition = leftHandPos;
        leftHand.localRotation = leftHandRot;

        photonView.RPC("SetLimbs", RpcTarget.Others, headsetPos, headsetRot, rightHandPos, rightHandRot, leftHandPos, leftHandRot);
    }

    [PunRPC]
    void SetLimbs(Vector3 headsetPos, Quaternion headsetRot, Vector3 rightHandPos, Quaternion rightHandRot, Vector3 leftHandPos, Quaternion leftHandRot)
    {
        headset.localPosition = headsetPos;
        headset.localRotation = headsetRot;

        rightHand.localPosition = rightHandPos;
        rightHand.localRotation = rightHandRot;

        leftHand.localPosition = leftHandPos;
        leftHand.localRotation = leftHandRot;
    }
}