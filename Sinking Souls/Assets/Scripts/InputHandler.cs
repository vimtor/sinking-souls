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
        return ButtonSwitch(buttonX);
    }

    private static bool buttonA;
    public static bool ButtonA() {
        return ButtonSwitch(buttonA);
    }

    private static bool buttonB;
    public static bool ButtonB() {
        return ButtonSwitch(buttonB);
    }

    private static bool buttonY;
    public static bool ButtonY() {
        return ButtonSwitch(buttonY);
    }

    private static bool buttonRT;
    public static bool ButtonRT() {
        return ButtonSwitch(buttonY);
    }
    #endregion

    public static bool ButtonSwitch(bool button) {
        if (button) {
            button = false;
            return true;
        }

        return false;
    }

    void Update() {

        buttonA = Input.GetButtonDown("BUTTON_A");
        buttonB = Input.GetButtonDown("BUTTON_B");
        buttonX = Input.GetButtonDown("BUTTON_X");
        buttonY = Input.GetButtonDown("BUTTON_Y");
        buttonRT = Input.GetAxis("BUTTON_RT") >= 0.5;

        LeftJoystick = new Vector2(Input.GetAxis("JOYSTICK_LH"), Input.GetAxis("JOYSTICK_LV"));
        RightJoystick = new Vector2(Input.GetAxis("JOYSTICK_RH"), Input.GetAxis("JOYSTICK_RV"));

    }

}
