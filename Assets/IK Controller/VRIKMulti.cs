using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class VRIKMulti : MonoBehaviourPun
{
    [Header("IK")]
    [SerializeField] Rigidbody body;

    [SerializeField] Transform head;
    [SerializeField] Transform spine;
    [SerializeField] Transform pelvis;

    [SerializeField] Vector3 headOffset;

    [SerializeField] Transform headset;
    [SerializeField] Transform leftHand;
    [SerializeField] Transform leftHandTracker;
    [SerializeField] Transform rightHand;
    [SerializeField] Transform rightHandTracker;

    [SerializeField] IKFootSolver rightFoot;
    [SerializeField] IKFootSolver leftFoot;

    [SerializeField] float minSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] float speedMultiplier;
    [SerializeField] float minLength;
    [SerializeField] float maxLength;
    [SerializeField] float lengthMultiplier;

    float headToPelvisDist;

    void Start()
    {
        if (!photonView.IsMine)
        {
            this.enabled = false;
        }
        headToPelvisDist = head.position.y - pelvis.position.y;
    }

    void Update()
    {
        FeetIK();
        
        pelvis.position = pelvis.parent.position + headset.localPosition + Vector3.down * headToPelvisDist + headset.up * headOffset.y;
        pelvis.localRotation = Quaternion.Euler(0f, headset.localRotation.eulerAngles.y, 0f);

        head.localRotation = Quaternion.Euler(headset.localRotation.eulerAngles.x, -spine.localRotation.eulerAngles.y, headset.localRotation.eulerAngles.z);

        rightHandTracker.position = rightHand.position;
        rightHandTracker.rotation = rightHand.rotation;

        leftHandTracker.position = leftHand.position;
        leftHandTracker.rotation = leftHand.rotation;

        photonView.RPC("SetIK", RpcTarget.Others, pelvis.position, pelvis.rotation, head.rotation, rightHandTracker.position, rightHandTracker.rotation, leftHandTracker.position, leftHandTracker.rotation);
    }

    [PunRPC]
    void SetIK(Vector3 pelvisPos, Quaternion pelvisRot, Quaternion headRot, Vector3 rightPos, Quaternion rightRot, Vector3 leftPos, Quaternion leftRot)
    {
        pelvis.position = pelvisPos;
        pelvis.rotation = pelvisRot;

        head.rotation = headRot;

        rightHandTracker.position = rightPos;
        rightHandTracker.rotation = rightRot;

        leftHandTracker.position = leftPos;
        leftHandTracker.rotation = leftRot;
    }

    void FeetIK()
    {
        rightFoot.speed = Mathf.Clamp(body.velocity.sqrMagnitude * speedMultiplier, minSpeed, maxSpeed);
        rightFoot.stepLength = Mathf.Clamp(rightFoot.speed * lengthMultiplier, minLength, maxLength);
        leftFoot.speed = rightFoot.speed;
        leftFoot.stepLength = rightFoot.stepLength;

        //photonView.RPC("SetFeet", RpcTarget.Others);
    }

    [PunRPC]
    void SetFeet()
    {

    }
}