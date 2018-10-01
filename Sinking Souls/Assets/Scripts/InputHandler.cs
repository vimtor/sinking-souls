using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Source Implementaion: https://drive.google.com/file/d/1CPFBDIOvw45Y8DtUXx_A4j4VAxMRJuav/view?usp=sharing

public class InputHandler : MonoBehaviour{

    // Different button types. 
    public enum ButtonType {
        NONE,
        BUTTON_A,
        BUTTON_B,
        BUTTON_X,
        BUTTON_Y,
        BUTTON_RT
    }

	public static class InputInfo {
        public static ButtonType Button;
        public static Vector2 LeftJoystick;
        public static Vector2 RightJoystick;
    }


    private void FixedUpdate() {

        if (Input.GetButtonDown("BUTTON_A"))        InputInfo.Button = ButtonType.BUTTON_A;
        else if (Input.GetButtonDown("BUTTON_B"))   InputInfo.Button = ButtonType.BUTTON_B;
        else if (Input.GetButtonDown("BUTTON_X"))   InputInfo.Button = ButtonType.BUTTON_X;
        else if (Input.GetButtonDown("BUTTON_Y"))   InputInfo.Button = ButtonType.BUTTON_Y;
        else if (Input.GetAxis("BUTTON_RT") >= 0.5) InputInfo.Button = ButtonType.BUTTON_RT;
        else InputInfo.Button = ButtonType.NONE;

        InputInfo.LeftJoystick = new Vector2(Input.GetAxis("JOYSTICK_LH"), Input.GetAxis("JOYSTICK_LV"));
        InputInfo.RightJoystick = new Vector2(Input.GetAxis("JOYSTICK_RH"), Input.GetAxis("JOYSTICK_RV"));
   
    }

}
