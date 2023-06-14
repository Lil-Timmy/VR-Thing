using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class PlayerInput
{

    //TEMP 
    public static float shift;

    public static float space;

    public static float d;

    public static float a;

    public static float w;

    public static float s;
    //TEMP


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
    public static int leftHandClickJoystick;
    //States, 0 = Released, 1 = Pressed
    public static int leftHandPrimary;
    //States, 0 = Released, 1 = Pressed
    public static int leftHandSecondary;

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
    public static int rightHandClickJoystick;
    //States, 0 = Released, 1 = Pressed
    public static int rightHandPrimary;
    //States, 0 = Released, 1 = Pressed
    public static int rightHandSecondary;

    //Bool, True When Tracked
    public static bool rightHandActive;
}
