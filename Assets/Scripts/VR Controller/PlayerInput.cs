using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class PlayerInput
{
    /* All Input References */

    //Position, Simple Vector3
    public static Vector3 headPosition;
    //Rotation, Simple Quaternion.
    public static Quaternion headRotation;

    //Bool, True When Tracked
    public static bool headActive;


    //Position, Simple Vector3.
    public static Vector3 leftHandPosition;
    //Rotation, Simple Quaternion.
    public static Quaternion leftHandRotation;

    //Amount, 0 To 1 Value Based On Percentage Pressed Down.
    public static float leftHandGrip;
    //Amount, 0 To 1 Value Based On Percentage Pressed Down.
    public static float leftHandTrigger;
    //Cooardinate System, Simple Vector2.
    public static Vector2 leftHandJoystick;
    //States, 0 = Released, 1 = Pressed
    public static bool leftHandClickJoystick;
    //States, 0 = Released, 1 = Pressed
    public static bool leftHandPrimary;
    //States, 0 = Released, 1 = Pressed
    public static bool leftHandSecondary;

    //Bool, True When Tracked
    public static bool leftHandActive;


    //Position, Simple Vector3.
    public static Vector3 rightHandPosition;
    //Rotation, Simple Quaternion.
    public static Quaternion rightHandRotation;

    //Amount, 0 To 1 Value Based On Percentage Pressed Down.
    public static float rightHandGrip;
    //Amount, 0 To 1 Value Based On Percentage Pressed Down.
    public static float rightHandTrigger;
    //Cooardinate System, Simple Vector2.
    public static Vector2 rightHandJoystick;
    //States, 0 = Released, 1 = Pressed
    public static bool rightHandClickJoystick;
    //States, 0 = Released, 1 = Pressed
    public static bool rightHandPrimary;
    //States, 0 = Released, 1 = Pressed
    public static bool rightHandSecondary;

    //Bool, True When Tracked
    public static bool rightHandActive;
}
