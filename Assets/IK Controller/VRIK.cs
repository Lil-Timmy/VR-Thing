using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIK : MonoBehaviour
{
    [Header("IK")]
    [SerializeField] Transform head;
    [SerializeField] Transform spine;
    [SerializeField] Transform pelvis;

    [SerializeField] Vector3 headOffset;

    [SerializeField] Transform headset;
    [SerializeField] Transform leftHand;
    [SerializeField] Transform leftHandTracker;
    [SerializeField] Transform rightHand;
    [SerializeField] Transform rightHandTracker;

    [SerializeField] Transform rightFoot;
    [SerializeField] Transform leftFoot;

    [SerializeField, Range(0f, 1f)] float armToBodyRotWeight;

    float headToPelvisDist;

    void Start()
    {
        headToPelvisDist = head.position.y - pelvis.position.y;
    }

    void Update()
    {
        pelvis.position = pelvis.parent.position + headset.localPosition + Vector3.down * headToPelvisDist + headset.up * headOffset.y;
        pelvis.localRotation = Quaternion.Euler(0f, headset.localRotation.eulerAngles.y, 0f);

        //Vector3 cross = Vector3.Cross(leftHandPos, rightHandPos).normalized;
        //float spineRot = pelvis.localRotation.eulerAngles.y - Vector3.Angle(Vector3.forward, Vector3.Cross(leftHand.position, rightHand.position).normalized);

        /*Vector3 spinePos = spine.position;
        spine.position = leftHand.position;
        
        Vector3 rightHandPos = new Vector3(rightHand.position.x, leftHand.position.y, rightHand.position.z);
        spine.rotation = Quaternion.LookRotation((rightHandPos - leftHand.position).normalized, Vector3.up);
        //spine.rotation = Quaternion.Euler(0f, spine.rotation.eulerAngles.y - 90f, 0f);
        spine.localRotation = Quaternion.Euler(0f, pelvis.rotation.eulerAngles.y - (spine.eulerAngles.y - 90) * -armToBodyRotWeight, 0f);

        spine.position = spinePos;*/

        head.localRotation = Quaternion.Euler(headset.localRotation.eulerAngles.x, -spine.localRotation.eulerAngles.y, headset.localRotation.eulerAngles.z);

        rightHandTracker.position = rightHand.position;
        rightHandTracker.rotation = rightHand.rotation;

        leftHandTracker.position = leftHand.position;
        leftHandTracker.rotation = leftHand.rotation;
    }
}
