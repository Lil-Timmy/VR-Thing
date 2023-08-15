using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class VRController : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform headset;
    [SerializeField] Transform rightHand;
    [SerializeField] Transform leftHand;
    [SerializeField] Rigidbody bodyRb;
    [SerializeField] CapsuleCollider bodyCapsule;
    [SerializeField] Transform bodyRotation;

    [SerializeField] Transform orientation;

    [SerializeField] float rotationSensitivity;
    [SerializeField] float joystickDeadzone;

    [SerializeField] float defaultMoveSpeed;
    [SerializeField] float airMultiplier;
    [SerializeField] float sprintMultiplier;
    [SerializeField] float accelerationSpeed;

    [SerializeField] float jumpForce;

    [SerializeField] float crouchMultiplier;
    [SerializeField] float crouchHeight;

    [SerializeField] float groundCheckDist;
    [SerializeField] LayerMask groundCheckLayer;

    bool isGrounded;
    float currentCrouchHeight;

    Vector3 prevHeadPos;
    Vector3 prevRightHandPos;
    Vector3 prevLeftHandPos;
    float ceilingHeight;
    float prevCeilingHeight;
    float currentRotation;
    float prevCrouchHeight;
    Vector3 currentMovementSpeed;
    
    void Awake()
    {
        if (!photonView.IsMine)
        {
            headset.GetComponent<Camera>().enabled = false;
            headset.GetComponent<AudioListener>().enabled = false;
            bodyRb.isKinematic = true;
            bodyCapsule.enabled = false;
            this.enabled = false;
        }
    }

    void Update()
    {
        // Sets raycast from 1x groundCheckDist Height above bodyCapsuleBottom 2x groundCheckDist Below.
        isGrounded = Physics.Raycast(new Vector3(bodyCapsule.transform.position.x + bodyCapsule.center.x, bodyCapsule.transform.position.y + groundCheckDist, bodyCapsule.transform.position.z + bodyCapsule.center.z), Vector3.down, out RaycastHit groundRay, groundCheckDist * 2, groundCheckLayer);
        
        Jump();

        Rotation();

        Limbs();

        CapsuleHeight();

        Movement();

        prevRightHandPos = PlayerInput.rightHandPosition;
        prevLeftHandPos = PlayerInput.leftHandPosition;

        photonView.RPC("SyncProperties", RpcTarget.Others, headset.position, headset.rotation, rightHand.position, rightHand.rotation, leftHand.position, leftHand.rotation);
    }

    void Jump()
    {
        // Tries to execute a jump.
        if (PlayerInput.rightHandPrimary && isGrounded)
        {
            bodyRb.velocity = new Vector3 (bodyRb.velocity.x, jumpForce, bodyRb.velocity.z);
        }
    }

    void Rotation()
    {
        // Direction of movement.
        orientation.rotation = Quaternion.Euler(0f, headset.rotation.eulerAngles.y, 0f);
        orientation.position = headset.position;
        // Turning rotation + turning hand rot calcs.
        if (PlayerInput.rightHandJoystick.x > joystickDeadzone || PlayerInput.rightHandJoystick.x < -joystickDeadzone)
        {
            float addedRot = PlayerInput.rightHandJoystick.x * rotationSensitivity * Time.deltaTime;
            currentRotation += addedRot;
            bodyRotation.localRotation = Quaternion.Euler(0f, currentRotation, 0f);

            rightHand.RotateAround(orientation.position, Vector3.up, addedRot);
            leftHand.RotateAround(orientation.position, Vector3.up, addedRot);
        }
    }

    void Movement()
    {
        if (PlayerInput.rightHandClickJoystick)
        {
            currentCrouchHeight = Mathf.Lerp(currentCrouchHeight, crouchHeight, 0.4f);
        }
        else
        {
            currentCrouchHeight = Mathf.Lerp(currentCrouchHeight, 0f, 0.4f);
        }

        // Movement calculations.
        Vector2 movement = PlayerInput.leftHandJoystick * defaultMoveSpeed;
        Vector3 moveVector = (orientation.forward * movement.y + orientation.right * movement.x);
        if (!isGrounded)
        {   
            moveVector *= airMultiplier;
        }
        // Crouch & sprint check.
        if (PlayerInput.rightHandClickJoystick)
        {
            currentMovementSpeed = Vector3.Lerp(currentMovementSpeed, moveVector * crouchMultiplier, Time.deltaTime / Time.fixedDeltaTime * accelerationSpeed);
        }
        else if (PlayerInput.leftHandClickJoystick)
        {
            currentMovementSpeed = Vector3.Lerp(currentMovementSpeed, moveVector * sprintMultiplier, Time.deltaTime / Time.fixedDeltaTime * accelerationSpeed);
        }
        else
        {
            currentMovementSpeed = Vector3.Lerp(currentMovementSpeed, moveVector, Time.deltaTime / Time.fixedDeltaTime * accelerationSpeed);
        }
        transform.Translate(currentMovementSpeed * Time.deltaTime, Space.World);

        bodyRb.velocity = new Vector3(0.9f * bodyRb.velocity.x, bodyRb.velocity.y, 0.9f * bodyRb.velocity.z);
    }

    void CapsuleHeight()
    {
        headset.localPosition = bodyRotation.forward * (PlayerInput.headPosition.z - prevHeadPos.z) + bodyRotation.right * (PlayerInput.headPosition.x - prevHeadPos.x) + new Vector3(headset.localPosition.x, PlayerInput.headPosition.y - currentCrouchHeight, headset.localPosition.z);
        headset.localRotation = Quaternion.Euler(PlayerInput.headRotation.eulerAngles.x, PlayerInput.headRotation.eulerAngles.y + currentRotation, PlayerInput.headRotation.eulerAngles.z);
        prevHeadPos = PlayerInput.headPosition;

        Physics.SphereCast(headset.position + Vector3.down * PlayerInput.headPosition.y / 2, bodyCapsule.radius * 0.8f, Vector3.up, out RaycastHit ceilingRay, PlayerInput.headPosition.y, groundCheckLayer);
        if (ceilingRay.transform != null && ceilingRay.point.y < headset.position.y + bodyCapsule.radius * 0.5f)
        {
            //Calculate height directly below blocking object above.
            float bodyHeight = headset.position.y - transform.position.y - (headset.position.y + bodyCapsule.radius - ceilingRay.point.y);
            bodyCapsule.height = Mathf.Clamp(bodyHeight - currentCrouchHeight, bodyCapsule.radius, bodyHeight);
            //Set headset height depending on ceiling, but not using capsule position since it can jitter.
            ceilingHeight = headset.position.y + bodyCapsule.radius - ceilingRay.point.y + 0.02f;
            headset.localPosition = new Vector3(headset.localPosition.x, Mathf.Clamp(PlayerInput.headPosition.y - ceilingHeight - currentCrouchHeight, 0f, 999f), headset.localPosition.z);
        }
        else
        {
            //Use regular capsule height calculations.
            bodyCapsule.height = Mathf.Clamp(PlayerInput.headPosition.y - currentCrouchHeight, bodyCapsule.radius, 999f);
            ceilingHeight = 0f;
        }
        bodyCapsule.center = new Vector3(headset.localPosition.x, bodyCapsule.height / 2, headset.localPosition.z);
    }

    void Limbs()
    {
        // Add onto current hand position with difference of movement.
        rightHand.localPosition += bodyRotation.forward * (PlayerInput.rightHandPosition.z - prevRightHandPos.z) + bodyRotation.right * (PlayerInput.rightHandPosition.x - prevRightHandPos.x) + bodyRotation.up * (PlayerInput.rightHandPosition.y - prevRightHandPos.y);
        rightHand.localRotation = Quaternion.Euler(PlayerInput.rightHandRotation.eulerAngles.x, PlayerInput.rightHandRotation.eulerAngles.y + currentRotation, PlayerInput.rightHandRotation.eulerAngles.z) * Quaternion.Euler(24f, 0f, 0f);

        leftHand.localPosition += bodyRotation.forward * (PlayerInput.leftHandPosition.z - prevLeftHandPos.z) + bodyRotation.right * (PlayerInput.leftHandPosition.x - prevLeftHandPos.x) + bodyRotation.up * (PlayerInput.leftHandPosition.y - prevLeftHandPos.y);
        leftHand.localRotation = Quaternion.Euler(PlayerInput.leftHandRotation.eulerAngles.x, PlayerInput.leftHandRotation.eulerAngles.y + currentRotation, PlayerInput.leftHandRotation.eulerAngles.z) * Quaternion.Euler(24f, 0f, 0f);

        rightHand.position += Vector3.down * (ceilingHeight - prevCeilingHeight + (currentCrouchHeight - prevCrouchHeight));
        leftHand.position += Vector3.down * (ceilingHeight - prevCeilingHeight + (currentCrouchHeight - prevCrouchHeight));
        prevCeilingHeight = ceilingHeight;
        prevCrouchHeight = currentCrouchHeight;
    }

    [PunRPC]
    void SyncProperties(Vector3 _headsetPos, Quaternion _headsetRot, Vector3 _rightHandPos, Quaternion _rightHandRot, Vector3 _leftHandPos, Quaternion _leftHandRot)
    {
        headset.position = Vector3.Lerp(headset.position, _headsetPos, Time.deltaTime / Time.fixedDeltaTime * 0.5f);
        headset.rotation = Quaternion.Lerp(headset.rotation, _headsetRot, Time.deltaTime / Time.fixedDeltaTime * 0.5f);

        rightHand.position = Vector3.Lerp(rightHand.position, _rightHandPos, Time.deltaTime / Time.fixedDeltaTime * 0.5f);
        rightHand.rotation = Quaternion.Lerp(rightHand.rotation, _rightHandRot, Time.deltaTime / Time.fixedDeltaTime * 0.5f);
        
        leftHand.position = Vector3.Lerp(leftHand.position, _leftHandPos, Time.deltaTime / Time.fixedDeltaTime * 0.5f);
        leftHand.rotation = Quaternion.Lerp(leftHand.rotation, _leftHandRot, Time.deltaTime / Time.fixedDeltaTime * 0.5f);
    }
}