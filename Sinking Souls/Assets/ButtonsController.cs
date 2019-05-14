using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class ButtonsController : MonoBehaviour {

    [Header("Configuration:")]
    public float delay;
    public Color normalColor;
    public Color highlitedColor;

    private GameObject ESys;

    private float time = 0;
    public Button[] butArr;
    public int selected = 0;
    private Ray ray;
    public RaycastHit hit;
    public UnityEvent ButtonEvents;
    public bool mouseOnButton = false;



    void Start () {
        //Acces to necesary gameObjects
        ESys = GameObject.Find("EventSystem");
        butArr = gameObject.GetComponentsInChildren<Button>();
        foreach (Button but in butArr)
        {
            but.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = normalColor;
        }
        if (!(butArr == null || butArr.Length == 0)) butArr[selected].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = highlitedColor;
    }
	

	void Update () {
        //Init for pause menu
       // hit.transform.gameObject = null;
       
        if (transform.Find("MainMenu Content").gameObject != null)
        {
            if (butArr == null || butArr.Length == 0)
            {
                butArr = gameObject.GetComponentsInChildren<Button>();
                foreach (Button but in butArr)
                {
                    but.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = normalColor;
                }
                if (!(butArr == null || butArr.Length == 0)) butArr[selected].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = highlitedColor;
            }


            if (transform.Find("MainMenu Content").gameObject.activeInHierarchy)
            {
                ray.origin = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -100);
                ray.direction = new Vector3(0, 0, 1);

                //Key Input
                if (time >= delay)
                {
                    if (InputManager.ButtonA || Input.GetKeyDown(KeyCode.Return) || (mouseOnButton && GameController.instance.cursor.GetComponent<mouseCursor>().visible && Input.GetMouseButtonDown(0)))
                    {
                        if (ESys.GetComponent<UnityEngine.EventSystems.EventSystem>().currentSelectedGameObject != null)
                        {
                            Debug.Log("Accesing");
                            InputManager.ButtonA = false;
                            ButtonEvents = butArr[selected].onClick;
                            ButtonEvents.Invoke();
                        }
                        
                    }
                    else if (DownInput()) MoveDown();
                    else if (UpInput()) MoveUp();
                }

                //Mouse Input
                if (Physics.Raycast(ray, out hit) && GameController.instance.cursor.GetComponent<mouseCursor>().visible)
                {
                    mouseOnButton = true;
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
                    else
                    {
                        butArr[selected].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = highlitedColor;
                        butArr[selected].Select();
                    }
                }
                else if (GameController.instance.cursor.GetComponent<mouseCursor>().visible)
                {
                    ESys.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
                    mouseOnButton = false;
                }



                if (ESys.GetComponent<UnityEngine.EventSystems.EventSystem>().currentSelectedGameObject != null)
                {
                    butArr[selected].Select();      //Prevent EventSystem override
                }
                else butArr[selected].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = normalColor;



                time += Time.unscaledDeltaTime;
            }
            else
            {
                //Debug.Log("inactive");
                time = 0;
            }

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
        GameController.instance.cursor.GetComponent<mouseCursor>().Hide();
        butArr[selected].Select();
        time = 0;
    }

    void MoveUp()
    {
        butArr[selected].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = normalColor;
        if (selected - 1 >= 0) selected--;
        else selected = butArr.Length - 1;
        butArr[selected].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = highlitedColor;
        Cursor.visible = false;
        GameController.instance.cursor.GetComponent<mouseCursor>().Hide();
        butArr[selected].Select();
        time = 0;
    }

}
