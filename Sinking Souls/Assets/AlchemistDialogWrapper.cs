using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System;

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
                foreach (Button but in itemArr) Normalize(but);
                selected = 0;
                Highlight(itemArr[selected]);
            }
        }
        else
        {
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
                UpdateMouse();
                ray.origin = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -100);
                ray.direction = new Vector3(0, 0, 1);

                //Key Input
                if (time >= delay)
                {
                    if (InputManager.ButtonA || Input.GetKeyDown(KeyCode.Return) || (Physics.Raycast(ray, out hit) && GameController.instance.cursor.GetComponent<mouseCursor>().visible && Input.GetMouseButtonDown(0)))
                    {
                        InputManager.ButtonA = false;
                        ButtonEvents = itemArr[selected].onClick;
                        ButtonEvents.Invoke();
                        Highlight(itemArr[selected]);
                    }
                    else if (DownInput()) MoveDown();
                    else if (UpInput()) MoveUp();
                    if (InputManager.ButtonB || Input.GetKeyDown(KeyCode.E))
                    {
                        InputManager.ButtonB = false;
                        gameObject.SetActive(false);
                        selected = 0;
                        reset = true;
                        time = 0;
                    }
                    
                }
                else if (InputManager.ButtonA) InputManager.ButtonA = false;

                //Mouse Input
                if (Physics.Raycast(ray, out hit) && GameController.instance.cursor.GetComponent<mouseCursor>().visible)
                {
                    Debug.Log(hit.transform.gameObject.name);
                    if (hit.transform.gameObject.GetComponent<Button>() != itemArr[selected])
                    {
                        for (int i = 0; i < itemArr.Length; i++)
                        {
                            if (hit.transform.gameObject.GetComponent<Button>() == itemArr[i])
                            {
                                Normalize(itemArr[selected]);
                                selected = i;
                                Highlight(itemArr[selected]);
                            }
                        }
                    }
                    else
                    {
                        Highlight(itemArr[selected]);
                    }
                }
                else if (GameController.instance.cursor.GetComponent<mouseCursor>().visible)
                {
                    ESys.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
                }

                if (ESys.GetComponent<UnityEngine.EventSystems.EventSystem>().currentSelectedGameObject != null)
                {
                    //Highlight(itemArr[selected]);
                    Highlight(itemArr[selected]);     //Prevent EventSystem override
                }
                else
                {
                    Normalize(itemArr[selected]);
                }

            if (reset)
                {
                    Normalize(itemArr[selected]);
                    selected = 0;
                    Highlight(itemArr[selected]);
                    reset = false;
                    time = 0;
                }

                time += Time.unscaledDeltaTime;
            }
        }

    }


    void Highlight(Button but)
    {
        //If this button have a price, check if the player can pay it
        if(but == itemArr[1])
        {
            if (GameObject.Find("Ailin").GetComponent<AlchemistBehaviour>().upgradeCost > GameController.instance.lobbySouls) but.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = highlightedUnavailableTextColor;
            else but.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = highlightedTextColor;
        }
        else but.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = highlightedTextColor;
        but.Select();
    }

    void Normalize(Button but)
    {
        //If this button have a price, check if the player can pay it
        if (but == itemArr[1])
        {
            if (GameObject.Find("Ailin").GetComponent<AlchemistBehaviour>().upgradeCost > GameController.instance.lobbySouls) but.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = normalUnavailableTextColor;
            else but.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = normalTextColor;
        }
        else but.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = normalTextColor;
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
        Normalize(itemArr[selected]);
        selected = (selected + 1) % itemArr.Length;
        Highlight(itemArr[selected]);
        Cursor.visible = false;
        GameController.instance.cursor.GetComponent<mouseCursor>().Hide();

        time = 0;
    }

    void MoveUp()
    {
        Normalize(itemArr[selected]);
        if (selected - 1 >= 0) selected--;
        else selected = itemArr.Length - 1;
        Highlight(itemArr[selected]);
        Cursor.visible = false;
        GameController.instance.cursor.GetComponent<mouseCursor>().Hide();
        time = 0;
    }

    private bool hiding;
    public void UpdateMouse()
    {
        if (Math.Abs(InputManager.Mouse.magnitude) > 0.0f)
        {
            hiding = false;
            //Cursor.visible = true;
            GameController.instance.cursor.GetComponent<mouseCursor>().Show();
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
        if (hiding)
        {
            Cursor.visible = false;
            GameController.instance.cursor.GetComponent<mouseCursor>().Hide();
        }
    }

}
