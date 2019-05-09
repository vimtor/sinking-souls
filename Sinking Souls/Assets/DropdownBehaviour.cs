using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropdownBehaviour : MonoBehaviour
{


    public float delay;
    public Color normalColor;
    public Color highlightedColor;

    private GameObject ESys;

    public float time = 0;
   
    private Ray ray;
    public RaycastHit hit;
    private Scrollbar scroll;
    public Toggle[] toggleArr = null;
    public TextMeshProUGUI label;
    private TMP_Dropdown dropdown;
    public int scrollPos = 0;
    public int selected = 0;
    public int lastSelected = 0;

    private void Start()
    {
        dropdown = gameObject.GetComponent<TMP_Dropdown>();
        selected = 0;
        lastSelected = 0;
        
    }

    private void Update()
    {

        //Open dropdown
        if (dropdown.IsExpanded)
        {

            //Empty List
            if (toggleArr == null || toggleArr.Length == 0)
            {
                //Fill List
                toggleArr = gameObject.GetComponentsInChildren<Toggle>(false);

                //Adjust scrollbar
                scroll = transform.Find("Dropdown List/Scrollbar").GetComponent<Scrollbar>();
                scroll.numberOfSteps = (toggleArr.Length);

                for (int i = 0; i < toggleArr.Length; i++)
                {
                    toggleArr[i].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = normalColor;
                    if(toggleArr[i].gameObject.GetComponentInChildren<TextMeshProUGUI>().text == label.text)
                    {
                        selected = lastSelected = i;
                        SetScroll(selected);
                    }
                }
                time = 0;
                Debug.Log(scroll.value);
            }
            //Filled list 
            else
            {
                Debug.Log(scroll.value);
                //Key Inputs
                if (time >= delay)
                {
                    Debug.Log("Te pille");
                    if (DownInput()) MoveDown();
                    else if (UpInput()) MoveUp();
                }
                else SetScroll(selected);
                //Mouse Inputs
                ray.origin = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -100);
                ray.direction = new Vector3(0, 0, 1);

                if (Input.mouseScrollDelta.y < 0) ScrollDown();
                else if (Input.mouseScrollDelta.y > 0) ScrollUp();

                if (Physics.Raycast(ray, out hit) && GameController.instance.cursor.GetComponent<mouseCursor>().visible && InputManager.Mouse.magnitude != 0)
                {
                    //Debug.Log(hit.transform.gameObject.name);
                    if (hit.transform.gameObject != toggleArr[selected].gameObject)
                    {
                        toggleArr[selected].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = normalColor;
                        for (int i = 0; i < toggleArr.Length; i++)
                        {
                            if (hit.transform.gameObject.name == toggleArr[i].name)
                            {
                                selected = i;
                                toggleArr[selected].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = highlightedColor;
                            }
                        }
                    }
                }
                //else if (Cursor.visible) ESys.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);

                toggleArr[selected].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = highlightedColor;
                toggleArr[selected].Select();
                scroll.size = 0.2f;
                //SetScroll(selected);

            }

            if (InputManager.ButtonB || Input.GetKey(KeyCode.Escape))
            {
                InputManager.ButtonB = false;

                dropdown.value = lastSelected;
                dropdown.Hide();
                label.text = toggleArr[lastSelected].gameObject.GetComponentInChildren<TextMeshProUGUI>().text;
            }
            else if (Input.GetKeyDown(KeyCode.Return) && (time >= delay))
            {
                InputManager.ButtonA = false;
                time = 0;
                dropdown.value = lastSelected = selected;
                toggleArr[lastSelected].GetComponent<Toggle>().isOn = !toggleArr[lastSelected].GetComponent<Toggle>().isOn;
                //dropdown.Hide();
                label.text = toggleArr[lastSelected].gameObject.GetComponentInChildren<TextMeshProUGUI>().text;
            }

            time += Time.unscaledDeltaTime;
        }
        //Closed dropdown
        else
        {
            toggleArr = null;
            //label.text = toggleArr[lastSelected].gameObject.GetComponentInChildren<TextMeshProUGUI>().text;
        }

        

        
    }




    ///Key input
    bool DownInput()
    {
        return InputManager.LeftJoystick.y > 0.1f || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
    }

    bool UpInput()
    {
        return InputManager.LeftJoystick.y < -0.1f || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
    }

    void SetScroll(int _pos, float _size = 0.2f)
    {
        Debug.Log(_pos);
        scrollPos = _pos;
        scroll.size = _size;
        scroll.value = 1 - ((float)scrollPos / (scroll.numberOfSteps-1));   //1 - Because the scroll goes Bottom - Up
    }



    ///Movement
    void ScrollDown()
    {
        if (scrollPos < scroll.numberOfSteps-1) scrollPos++;
        scroll.value = 1 - ((float)scrollPos / (scroll.numberOfSteps-1));   //1 - Because the scroll goes Bottom - Up
    }

    void ScrollUp()
    {
        if (scrollPos > 0) scrollPos--;
        scroll.value = 1 - ((float)scrollPos / (scroll.numberOfSteps-1));    //1 - Because the scroll goes Bottom - Up
    }


    void MoveDown()
    {
        toggleArr[selected].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = normalColor;
        if (selected + 1 < dropdown.options.Count) selected++; // % dropdown.options.Count;
        SetScroll(selected);
        toggleArr[selected].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = highlightedColor;
        Cursor.visible = false;
        GameController.instance.cursor.GetComponent<mouseCursor>().Hide();
        time = 0;
    }

    void MoveUp()
    {
        toggleArr[selected].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = normalColor;
        if (selected - 1 >= 0) selected--;
        //else selected = dropdown.options.Count - 1;
        SetScroll(selected);
        toggleArr[selected].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = highlightedColor;
        Cursor.visible = false;
        GameController.instance.cursor.GetComponent<mouseCursor>().Hide();
        time = 0;
    }


}
