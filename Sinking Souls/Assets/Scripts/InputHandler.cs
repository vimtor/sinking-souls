using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Source Implementaion: https://drive.google.com/file/d/1CPFBDIOvw45Y8DtUXx_A4j4VAxMRJuav/view?usp=sharing


public class InputHandler : MonoBehaviour {

    public static Vector2 LeftJoystick;
    public static Vector2 RightJoystick;

    #region --- BUTTON FUNCTIONS ---
    private static bool buttonX;
    public static bool ButtonX() {
        return ButtonSwitch(ref buttonX);
    }

    private static bool buttonA;
    public static bool ButtonA() {
        return ButtonSwitch(ref buttonA);
    }

    private static bool buttonB;
    public static bool ButtonB() {
        return ButtonSwitch(ref buttonB);
    }

    private static bool buttonY;
    public static bool ButtonY() {
        return ButtonSwitch(ref buttonY);
    }

    private static bool buttonRT;
    public static bool ButtonRT() {
        return ButtonSwitch(ref buttonY);
    }
    #endregion

    public static bool ButtonSwitch(ref bool button) {
        if (button) {
            button = false;
            return true;
        }

        return false;
    }

    public static bool LeftJoystickZero() {
        return LeftJoystick == Vector2.zero;
    }

    void Update() {

        if (!buttonA) buttonA = Input.GetButtonDown("BUTTON_A");
        if (!buttonB) buttonB = Input.GetButtonDown("BUTTON_B");
        if (!buttonX) buttonX = Input.GetButtonDown("BUTTON_X");
        if (!buttonY) buttonY = Input.GetButtonDown("BUTTON_Y");
        if (!buttonRT) buttonRT = Input.GetAxis("BUTTON_RT") >= 0.5;

        LeftJoystick = new Vector2(Input.GetAxis("JOYSTICK_LH"), Input.GetAxis("JOYSTICK_LV"));
        RightJoystick = new Vector2(Input.GetAxis("JOYSTICK_RH"), Input.GetAxis("JOYSTICK_RV"));

    }



}
