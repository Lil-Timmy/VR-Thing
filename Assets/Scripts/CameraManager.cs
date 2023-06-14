using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    int rightPreviousState;
    int leftPreviousState;

    [SerializeField] Camera cam;
    Rigidbody rb;
    GameObject recordingObject;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (recordingObject == null)
        {
            GameObject tracker = GameObject.Find("Recording");
            if (tracker != null)
            {
                recordingObject = tracker;
                recordingObject.SetActive(false);
            }
        }
        else
        {
            //Lock Camera
            if (PlayerInput.rightHandSecondary != 0 && rightPreviousState == 0)
            {
                if (rb.isKinematic)
                    rb.isKinematic = false;
                else
                    rb.isKinematic = true;
            }
            rightPreviousState = PlayerInput.rightHandSecondary;        
            //Swap Camera
            if (PlayerInput.leftHandSecondary != 0 && leftPreviousState == 0)
            {
                if (cam.enabled)
                {
                    recordingObject.SetActive(false);
                    cam.enabled = false;
                }
                else
                {
                    recordingObject.SetActive(true);
                    cam.enabled = true;
                }
            }
            leftPreviousState = PlayerInput.leftHandSecondary;
        }
    }

    void FixedUpdate()
    {
        rb.velocity *= 0.9f;
        rb.angularVelocity *= 0.9f;
    }
}
