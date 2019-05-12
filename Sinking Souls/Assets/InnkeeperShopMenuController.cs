using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class InnkeeperShopMenuController : MonoBehaviour {

    [Header("Configuration:")]
    public float delay;
    public Color normalTextColor;
    public Color highlightedTextColor;
    public Color normalImageColor;
    public Color highlightedImageColor;
    public Color normalUnavailableTextColor;
    public Color highlightedUnavailableTextColor;
    public Color normalUnavailableImageColor;
    public Color highlightedUnavailableImageColor;


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
        selected = 0;
    }


    void Update()
    {
        //Load menu items (because is a dynamic content)
        if (itemArr == null || itemArr.Length == 0)
        {
            itemArr = gameObject.GetComponentsInChildren<Button>();
            foreach (Button but in itemArr)
            {
                //Detect if the player can buy the item or it is too expensive
                int price = 0;
                int.TryParse(but.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text, out price);

                //Actuallice item properties
                if (GameController.instance.lobbySouls >= price) SetItemColor(but, normalTextColor, normalImageColor);   //Can buy
                else SetItemColor(but, normalUnavailableTextColor, normalUnavailableImageColor);                         //Can not buy

            }
            if (!(itemArr == null || itemArr.Length == 0))
            {
                Highlight(itemArr[selected]);
            }
        }


        //Shop is closed
        if (!gameObject.activeSelf)
        {
            reset = true;
            selected = 0;
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
                if (InputManager.ButtonA || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E) || (Physics.Raycast(ray, out hit) && GameController.instance.cursor.GetComponent<mouseCursor>().visible && Input.GetMouseButtonDown(0)))
                {
                    InputManager.ButtonA = false;
                    ButtonEvents = itemArr[selected].onClick;
                    ButtonEvents.Invoke();
                    Refresh();
                }
                else if (DownInput()) MoveDown();
                else if (UpInput()) MoveUp();
                if (InputManager.ButtonB)
                {
                    Cursor.visible = false;
                    GameController.instance.cursor.GetComponent<mouseCursor>().InstaHide();
                }
            }
            else if (InputManager.ButtonA) InputManager.ButtonA = false;

            //Mouse Input
            if (Physics.Raycast(ray, out hit) && GameController.instance.cursor.GetComponent<mouseCursor>().visible)
            {
                Debug.Log(hit.transform.gameObject.name);
                if (hit.transform.gameObject.GetComponent<Button>() != itemArr[selected])
                {
                    Normalize(itemArr[selected]);
                    for (int i = 0; i < itemArr.Length; i++)
                    {
                        if (hit.transform.gameObject.GetComponent<Button>() == itemArr[i])
                        {
                            selected = i;
                            Highlight(itemArr[selected]);
                            itemArr[selected].Select();
                        }
                    }
                }
                else
                {
                    itemArr[selected].Select();
                }
            }
            else if (GameController.instance.cursor.GetComponent<mouseCursor>().visible)
            {
                ESys.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
            }

            if (ESys.GetComponent<UnityEngine.EventSystems.EventSystem>().currentSelectedGameObject != null)
            {
                //Highlight(itemArr[selected]);
                itemArr[selected].Select();     //Prevent EventSystem override
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
                itemArr[selected].Select();
                reset = false;
                time = 0;
            }

            time += Time.unscaledDeltaTime;
        }

    }

    void SetItemColor(Button item, Color textColor, Color imageColor)
    {
        //Texts color
        item.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = textColor;
        item.gameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().color = textColor;
        item.gameObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>().color = textColor;

        //Background color 
        item.gameObject.GetComponent<Image>().color = imageColor;
    }

    void Highlight(Button item)
    {
        //Is this item available?
        if (item.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color == normalTextColor) SetItemColor(item, highlightedTextColor, highlightedImageColor);
        else SetItemColor(item, highlightedUnavailableTextColor, highlightedUnavailableImageColor);
    }

    void Normalize(Button item)
    {
        if (item.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color == highlightedTextColor) SetItemColor(item, normalTextColor, normalImageColor);
        else SetItemColor(item, normalUnavailableTextColor, normalUnavailableImageColor);
    }

    void Refresh()
    {
        foreach (Button but in itemArr)
        {
            //Detect if the player can buy the item or it is too expensive
            int price = 0;
            int.TryParse(but.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text, out price);

            //Actuallice item properties
            if (GameController.instance.lobbySouls >= price) SetItemColor(but, normalTextColor, normalImageColor);   //Can buy
            else SetItemColor(but, normalUnavailableTextColor, normalUnavailableImageColor);                        //Can not buy

        }
        Highlight(itemArr[selected]);
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
        itemArr[selected].Select();
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
        itemArr[selected].Select();
        Cursor.visible = false;
        GameController.instance.cursor.GetComponent<mouseCursor>().Hide();

        time = 0;
    }

}
