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


    public static ButtonType Button;
    public static Vector2 LeftJoystick;
    public static Vector2 RightJoystick;



    private void FixedUpdate() {

        if (Input.GetButtonDown("BUTTON_A"))        Button = ButtonType.BUTTON_A;
        else if (Input.GetButtonDown("BUTTON_B"))   Button = ButtonType.BUTTON_B;
        else if (Input.GetButtonDown("BUTTON_X"))   Button = ButtonType.BUTTON_X;
        else if (Input.GetButtonDown("BUTTON_Y"))   Button = ButtonType.BUTTON_Y;
        else if (Input.GetAxis("BUTTON_RT") >= 0.5) Button = ButtonType.BUTTON_RT;
        else Button = ButtonType.NONE;

        LeftJoystick = new Vector2(Input.GetAxis("JOYSTICK_LH"), Input.GetAxis("JOYSTICK_LV"));
        RightJoystick = new Vector2(Input.GetAxis("JOYSTICK_RH"), Input.GetAxis("JOYSTICK_RV"));
   
    }

}
