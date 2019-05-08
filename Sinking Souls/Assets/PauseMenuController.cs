using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System;

public class PauseMenuController : MonoBehaviour {

    [Header("Configuration:")]
    public float delay;
    public Color normalColor;
    public Color highlitedColor;

    private GameObject ESys;

    public float time = 0;
    public bool reset = false;
    public Button[] butArr;
    public int selected = 0;
    private Ray ray;
    public RaycastHit hit;
    public UnityEvent ButtonEvents;



    void Start () {
        //Acces to necesary gameObjects
        ESys = GameObject.Find("EventSystem");
        
    }
	

	void Update () {
        //Init for pause menu
        if(butArr == null || butArr.Length == 0)
        {
            butArr = gameObject.GetComponentsInChildren<Button>();
            foreach (Button but in butArr)
            {
                but.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = normalColor;
            }
            if (!(butArr == null || butArr.Length == 0)) butArr[selected].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = highlitedColor;
        }

        if (!gameObject.GetComponent<PauseMenu>().isPaused)
        {
            reset = true;
        }

        if (transform.Find("Pause Content").gameObject.activeInHierarchy)
        {
            UpdateMouse();
            ray.origin = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -100);
            ray.direction = new Vector3(0, 0, 1);

            //Key Input
            if (time >= delay)
            {
                if (InputManager.ButtonA || Input.GetKeyDown(KeyCode.Return) || (Physics.Raycast(ray, out hit) && Cursor.visible && Input.GetMouseButtonDown(0)))
                {
                    Debug.Log("Accesing");
                    InputManager.ButtonA = false;
                    ButtonEvents = butArr[selected].onClick;
                    ButtonEvents.Invoke();
                }
                else if (DownInput()) MoveDown();
                else if (UpInput()) MoveUp();
            }
            else if (InputManager.ButtonA) InputManager.ButtonA = false;

            //Mouse Input
                if (Physics.Raycast(ray, out hit) && Cursor.visible)
            {
                Debug.Log(hit.transform.gameObject.name);
                if (hit.transform.gameObject.GetComponent<Button>() != butArr[selected])
                {
                    butArr[selected].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = normalColor;
                    for (int i = 0; i < butArr.Length; i++)
                    {
                        if (hit.transform.gameObject.GetComponent<Button>() == butArr[i])
                        {
                            selected = i;
                            butArr[selected].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = highlitedColor;
                            butArr[selected].Select();
                        }
                    }
                }
            }

            if (reset)
            {
                butArr[selected].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = normalColor;
                selected = 0;
                butArr[selected].Select();
                butArr[selected].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = highlitedColor;
                reset = false;
                time = 0;
            }
            butArr[selected].Select();      //Prevent EventSystem override



            time += Time.unscaledDeltaTime;
        }
        else
        {
            time = 0;            
        }

    }


    ///Input
    bool DownInput()
    {
        return InputManager.LeftJoystick.y > 0.1f || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || InputManager.Dpad.y < -0.1f;
    }

    bool UpInput()
    {
        return InputManager.LeftJoystick.y < -0.1f || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || InputManager.Dpad.y > 0.1f;
    }


    ///Movement
    void MoveDown()
    {        
        butArr[selected].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = normalColor;
        selected = (selected + 1) % butArr.Length;
        butArr[selected].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = highlitedColor;
        Cursor.visible = false;
        time = 0;
    }

    void MoveUp()
    {
        butArr[selected].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = normalColor;
        if (selected - 1 >= 0) selected--;
        else selected = butArr.Length - 1;
        butArr[selected].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = highlitedColor;
        Cursor.visible = false;
        time = 0;
    }

    private bool hiding;
    public void UpdateMouse()
    {
        if (Math.Abs(InputManager.Mouse.magnitude) > 0.0f)
        {
            hiding = false;
            Cursor.visible = true;
        }
        else if (!hiding && hit.collider == null)
        {
            hiding = true;
            StartCoroutine(HideMouse(3.0f));
        }
    }

    private IEnumerator HideMouse(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        if (hiding) Cursor.visible = false;
    }

}
