using System;
using System.Collections;
using UnityEngine;

// Each Update captures if the button is pressed and gets cleaned when the fixed updates finish.
//
// If you want to check input on the fixed update use the simple getter,
// but in the update clean the input once captured.

public class InputManager : MonoBehaviour {

    public static Vector2 LeftJoystick;
    public static Vector2 RightJoystick;
    public static Vector2 Mouse;
    public static Vector2 Dpad;

    #region Button Variables
    public static bool m_ButtonX;
    public static bool ButtonX
    {
        get { return m_ButtonX; }
        set { m_ButtonX = value; }
    }

    private static bool m_ButtonA;
    public static bool ButtonA
    {
        get { return m_ButtonA; }
        set { m_ButtonA = value; }
    }
    public static bool GetButtonA()
    {
        return Input.GetButtonDown("BUTTON_A") || Input.GetKeyDown(KeyCode.U);
    }

    private static bool m_ButtonB;
    public static bool ButtonB
    {
        get { return m_ButtonB; }
        set { m_ButtonB = value; }
    }
    public static bool GetButtonB()
    {
        return Input.GetButtonDown("BUTTON_B") || Input.GetKeyDown(KeyCode.I);
    }

    private static bool m_ButtonY;
    public static bool ButtonY
    {
        get { return m_ButtonY; }
        set { m_ButtonY = value; }
    }

    private static bool m_ButtonRT;
    public static bool ButtonRT
    {
        get { return m_ButtonRT; }
        set { m_ButtonRT = value; }
    }

    private static bool m_ButtonStart;
    public static bool ButtonStart
    {
        get { return m_ButtonStart; }
        set { m_ButtonStart = value; }
    }

    private static bool m_ButtonSelect;
    public static bool ButtonSelect
    {
        get { return m_ButtonSelect; }
        set { m_ButtonSelect = value; }
    }


    private static bool m_ButtonRJ;
    public static bool ButtonRJ {
        get { return m_ButtonRJ; }
        set { m_ButtonRJ = value; }
    }


    #endregion

    public static bool LeftJoystickZero() {
        return LeftJoystick == Vector2.zero;
    }



    private void Start()
    {
        StartCoroutine(CleanInput());
    }

    public static int Xbox_One_Controller = 0;
    public static int PS4_Controller = 0;

    public float cursorHideTime = 3.0f;
    private bool hiding;

    bool keyboardUsed() {
        return
            Input.GetMouseButton(0)             ||
            Input.GetMouseButton(1)             ||
            Input.GetMouseButton(2)             ||
            Input.GetKeyDown(KeyCode.Space)     ||
            Input.GetKey(KeyCode.E)             ||
            Input.GetKey(KeyCode.F)             ||
            Input.GetKey(KeyCode.Escape)        ||
            Input.GetKey(KeyCode.Return)        ||
            Input.GetKey(KeyCode.W)             ||
            Input.GetKey(KeyCode.A)             ||
            Input.GetKey(KeyCode.S)             ||
            Input.GetKey(KeyCode.D)             ||
            Input.GetKey(KeyCode.Q)             ||
            Input.GetKey(KeyCode.DownArrow)     ||
            Input.GetKey(KeyCode.LeftArrow)     ||
            Input.GetKey(KeyCode.RightArrow)    ||
            Input.GetKey(KeyCode.UpArrow)       ||
            Input.GetKey(KeyCode.C)             ||
            Input.GetKey(KeyCode.X)             ||
            Input.GetKey(KeyCode.Tab);

    }

    void Update()
    {
        if (Input.GetButtonDown("BUTTON_A")) m_ButtonA = true;
        if (Input.GetButtonDown("BUTTON_B")) m_ButtonB = true;
        if (Input.GetButtonDown("BUTTON_X")) m_ButtonX = true;
        if (Input.GetButtonDown("BUTTON_Y")) m_ButtonY = true;
        if (Input.GetButtonDown("BUTTON_RIGHTJOYSTICK")) m_ButtonRJ = true;

        if (Input.GetAxis("BUTTON_RT") >= 0.5) m_ButtonRT = true;

        if (Input.GetButtonDown("START") || Input.GetKeyDown(KeyCode.Escape)) m_ButtonStart = true;
        if (Input.GetButtonDown("SELECT")) m_ButtonSelect = true;
        if (Input.GetButtonUp("SELECT")) m_ButtonSelect = false;

        LeftJoystick = new Vector2(Input.GetAxis("JOYSTICK_LH"), Input.GetAxis("JOYSTICK_LV"));
        RightJoystick = new Vector2(Input.GetAxis("JOYSTICK_RH"), Input.GetAxis("JOYSTICK_RV"));
        Dpad = new Vector2(Input.GetAxis("DPAD_H"), Input.GetAxis("DPAD_V"));
        Mouse = new Vector2(Input.GetAxis("MouseX"), Input.GetAxis("MouseY"));



        if (keyboardUsed()) Xbox_One_Controller = 0;
        else if ((m_ButtonA && !Input.GetMouseButton(0)) || m_ButtonRJ && !Input.GetKey(KeyCode.E) || (m_ButtonB && !Input.GetKey(KeyCode.Space)) || m_ButtonX || m_ButtonY || m_ButtonRJ || (m_ButtonRT && !Input.GetKey(KeyCode.F)) || m_ButtonStart || LeftJoystick.magnitude != 0 || RightJoystick.magnitude != 0 || Dpad.magnitude != 0) Xbox_One_Controller = 1;

        if(Input.GetKeyDown(KeyCode.F)) m_ButtonRT = true;
        if (Input.GetKeyDown(KeyCode.E)) m_ButtonRJ = true;
        if (LeftJoystickZero()) LeftJoystick = new Vector2(Input.GetAxis("KEY_HORIZONTAL"), Input.GetAxis("KEY_VERTICAL"));
    }

    IEnumerator CleanInput()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            m_ButtonA = false;
            m_ButtonB = false;
            m_ButtonX = false;
            m_ButtonY = false;
            m_ButtonRJ = false;
            m_ButtonRT = false;
        }
    }

    // Hide cursor if not use it for certain time.
    private IEnumerator HideMouse(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        if (hiding)
        {
            Cursor.visible = false;
            GameController.instance.cursor.GetComponent<mouseCursor>().Hide();
        }
    }

    public void UpdateMouse()
    {
        if (Mathf.Abs(Mouse.magnitude) > 0.0f)
        {
            hiding = false;
            //Cursor.visible = true;
            GameController.instance.cursor.GetComponent<mouseCursor>().Show();
        }
        else if (!hiding)
        {
            hiding = true;
            //StartCoroutine(HideMouse(cursorHideTime));
        }
    }

}
