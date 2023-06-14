using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    //TEMP
    public void Shift(InputAction.CallbackContext context)
    {
        PlayerInput.shift = context.ReadValue<float>();
    }
    public void Space(InputAction.CallbackContext context)
    {
        PlayerInput.space = context.ReadValue<float>();
    }
    public void D(InputAction.CallbackContext context)
    {
        PlayerInput.d = context.ReadValue<float>();
    }
    public void A(InputAction.CallbackContext context)
    {
        PlayerInput.a = context.ReadValue<float>();
    }
    public void W(InputAction.CallbackContext context)
    {
        PlayerInput.w = context.ReadValue<float>();
    }
    public void S(InputAction.CallbackContext context)
    {
        PlayerInput.s = context.ReadValue<float>();
    }
    //TEMP


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
            PlayerInput.leftHandClickJoystick = 1;
        }
        else
        {
            PlayerInput.leftHandClickJoystick = 0;
        }
    }

    public void LeftHandPrimary(InputAction.CallbackContext context)
    {
        //Pressed
        if (!context.canceled)
        {
            PlayerInput.leftHandPrimary = 1;
        }
        else
        {
            PlayerInput.leftHandPrimary = 0;
        }
    }

    public void LeftHandSecondary(InputAction.CallbackContext context)
    {
        //Pressed
        if (!context.canceled)
        {
            PlayerInput.leftHandSecondary = 1;
        }
        else
        {
            PlayerInput.leftHandSecondary = 0;
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
            PlayerInput.rightHandClickJoystick = 1;
        }
        else
        {
            PlayerInput.rightHandClickJoystick = 0;
        }
    }

    public void RightHandPrimary(InputAction.CallbackContext context)
    {
        //Pressed
        if (!context.canceled)
        {
            PlayerInput.rightHandPrimary = 1;
        }
        else
        {
            PlayerInput.rightHandPrimary = 0;
        }
    }

    public void RightHandSecondary(InputAction.CallbackContext context)
    {
        //Pressed                                         
        if (!context.canceled)
        {
            PlayerInput.rightHandSecondary = 1;
        }
        else
        {
            PlayerInput.rightHandSecondary = 0;
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