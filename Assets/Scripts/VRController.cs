using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRController : MonoBehaviour
{
    [SerializeField] Transform headset;
    [SerializeField] Transform rightHand;
    [SerializeField] Transform leftHand;


    [SerializeField] Rigidbody bodyRb;
    [SerializeField] CapsuleCollider bodyCapsule;

    [SerializeField] Transform orientation;

    [SerializeField] float movementSpeed;
    
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

        bodyCapsule.height = Mathf.Clamp(headset.localPosition.y / 2, bodyCapsule.radius, 999f);
        bodyCapsule.center = new Vector3(headset.localPosition.x, bodyCapsule.height / 2, headset.localPosition.z);
    }

    void Limbs()
    {
        headset.localPosition = PlayerInput.headPosition;
        headset.localRotation = PlayerInput.headRotation;

        leftHand.localPosition = PlayerInput.leftHandPosition;
        leftHand.localRotation = PlayerInput.leftHandRotation;

        rightHand.localPosition = PlayerInput.rightHandPosition;
        rightHand.localRotation = PlayerInput.rightHandRotation;
    }
}
