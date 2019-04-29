using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class AlchemistDialogWrapper : MonoBehaviour {

    [Header("Configuration:")]
    public float delay;
    public Color normalTextColor;
    public Color highlightedTextColor;
    public Color normalUnavailableTextColor;
    public Color highlightedUnavailableTextColor;


    private GameObject ESys;

    public float time = 0;
    public bool reset = false;
    public Button[] itemArr;
    public int selected = 0;
    private Ray ray;
    public RaycastHit hit;
    public UnityEvent ButtonEvents;



    void Start()
    {
        //Acces to necesary gameObjects
        ESys = GameObject.Find("EventSystem");

    }


    void Update()
    {
        //Load menu items (because is a dynamic content)
        if (itemArr == null || itemArr.Length == 0)
        {
            itemArr = gameObject.GetComponentsInChildren<Button>();
            if (!(itemArr == null || itemArr.Length == 0))
            {
                itemArr[selected].Select();
            }
        }


        //Shop is closed
        if (!gameObject.activeSelf)
        {
            selected = 0;
            reset = true;
            time = 0;
        }
        //Shop is open
        else
        {
            ray.origin = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -100);
            ray.direction = new Vector3(0, 0, 1);

            //Key Input
            if (time >= delay)
            {
                if (InputManager.ButtonA || Input.GetKeyDown(KeyCode.Return) || (Physics.Raycast(ray, out hit) && Cursor.visible && Input.GetMouseButtonDown(0)))
                {
                    InputManager.ButtonA = false;
                    ButtonEvents = itemArr[selected].onClick;
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
                if (hit.transform.gameObject.GetComponent<Button>() != itemArr[selected])
                {
                    for (int i = 0; i < itemArr.Length; i++)
                    {
                        if (hit.transform.gameObject.GetComponent<Button>() == itemArr[i])
                        {
                            selected = i;
                            itemArr[selected].Select();
                        }
                    }
                }
            }
            else if (Cursor.visible) ESys.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);

            if (reset)
            {
                selected = 0;
                itemArr[selected].Select();
                reset = false;
                time = 0;
            }
            itemArr[selected].Select();      //Prevent EventSystem override



            time += Time.unscaledDeltaTime;
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
        selected = (selected + 1) % itemArr.Length;
        itemArr[selected].Select();
        Cursor.visible = false;
        time = 0;
    }

    void MoveUp()
    {
        if (selected - 1 >= 0) selected--;
        else selected = itemArr.Length - 1;
        itemArr[selected].Select();
        Cursor.visible = false;
        time = 0;
    }

}
