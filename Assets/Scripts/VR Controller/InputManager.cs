using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public void HeadPosition(InputAction.CallbackContext context)
    {
        PlayerInput.headPosition = context.ReadValue<Vector3>();
    }

    public void HeadRotation(InputAction.CallbackContext context)
    {
        PlayerInput.headRotation = context.ReadValue<Quaternion>();
    }

    public void HeadActive(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() == 0)
        {
            PlayerInput.headActive = false;
        }
        else
        {
            PlayerInput.headActive = true;
        }
    }


    public void LeftHandPosition(InputAction.CallbackContext context)
    {
        PlayerInput.leftHandPosition = context.ReadValue<Vector3>();
    }

    public void LeftHandRotation(InputAction.CallbackContext context)
    {
        PlayerInput.leftHandRotation = context.ReadValue<Quaternion>();
    }

    public void LeftHandGrip(InputAction.CallbackContext context)
    {
        PlayerInput.leftHandGrip = context.ReadValue<float>();
    }

    public void LeftHandTrigger(InputAction.CallbackContext context)
    {
        PlayerInput.leftHandTrigger = context.ReadValue<float>();
    }

    public void LeftHandJoystick(InputAction.CallbackContext context)
    {
        PlayerInput.leftHandJoystick = context.ReadValue<Vector2>();
    }

    public void LeftHandClickJoystick(InputAction.CallbackContext context)
    {
        //Pressed
        if (!context.canceled)
        {
            PlayerInput.leftHandClickJoystick = true;
        }
        else
        {
            PlayerInput.leftHandClickJoystick = false;
        }
    }

    public void LeftHandPrimary(InputAction.CallbackContext context)
    {
        //Pressed
        if (!context.canceled)
        {
            PlayerInput.leftHandPrimary = true;
        }
        else
        {
            PlayerInput.leftHandPrimary = false;
        }
    }

    public void LeftHandSecondary(InputAction.CallbackContext context)
    {
        //Pressed
        if (!context.canceled)
        {
            PlayerInput.leftHandSecondary = true;
        }
        else
        {
            PlayerInput.leftHandSecondary = false;
        }
    }

    public void leftHandActive(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() == 0)
        {
            PlayerInput.leftHandActive = false;
        }
        else
        {
            PlayerInput.leftHandActive = true;
        }
    }


    public void RightHandPosition(InputAction.CallbackContext context)
    {
        PlayerInput.rightHandPosition = context.ReadValue<Vector3>();
    }

    public void RightHandRotation(InputAction.CallbackContext context)
    {
        PlayerInput.rightHandRotation = context.ReadValue<Quaternion>();
    }

    public void RightHandGrip(InputAction.CallbackContext context)
    {
        PlayerInput.rightHandGrip = context.ReadValue<float>();
    }

    public void RightHandTrigger(InputAction.CallbackContext context)
    {
        PlayerInput.rightHandTrigger = context.ReadValue<float>();
    }

    public void RightHandJoystick(InputAction.CallbackContext context)
    {
        PlayerInput.rightHandJoystick = context.ReadValue<Vector2>();
    }

    public void RightHandClickJoystick(InputAction.CallbackContext context)
    {
        //Pressed
        if (!context.canceled)
        {
            PlayerInput.rightHandClickJoystick = true;
        }
        else
        {
            PlayerInput.rightHandClickJoystick = false;
        }
    }

    public void RightHandPrimary(InputAction.CallbackContext context)
    {
        //Pressed
        if (!context.canceled)
        {
            PlayerInput.rightHandPrimary = true;
        }
        else
        {
            PlayerInput.rightHandPrimary = false;
        }
    }

    public void RightHandSecondary(InputAction.CallbackContext context)
    {
        //Pressed                                         
        if (!context.canceled)
        {
            PlayerInput.rightHandSecondary = true;
        }
        else
        {
            PlayerInput.rightHandSecondary = false;
        }
    }

    public void RightHandActive(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() == 0)
        {
            PlayerInput.rightHandActive = false;
        }
        else
        {
            PlayerInput.rightHandActive = true;
        }
    }
}