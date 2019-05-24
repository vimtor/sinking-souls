using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControlsMenuController : MonoBehaviour {

    public float timePressed = 0;
    public bool stayOpen = false;

    GameObject Controller;
    GameObject X;
    GameObject A;
    GameObject B;
    GameObject Y;
    GameObject RT;
    GameObject start;
    GameObject Select;
    GameObject LJ;
    GameObject Dpad;
    GameObject RJ;
    GameObject KeyBoard;

    public string TextX;
    public string TextA;
    public string TextB;
    public string TextY;
    public string TextRT;
    public string Textstart;
    public string TextSelect;
    public string TextLJ;
    public string TextDpad;
    public string TextRJ;

    public string KeyBoardX     ;
    public string KeyBoardA     ;
    public string KeyBoardB     ;
    public string KeyBoardY     ;
    public string KeyBoardRT    ;
    public string KeyBoardstart ;
    public string KeyBoardSelect;
    public string KeyBoardLJ    ;
    public string KeyBoardDpad  ;
    public string KeyBoardRJ    ;

    private Color original;

	// Use this for initialization

    public void ShowController() {
        if (GameObject.Find("Pause Content") || GameObject.Find("Settings Menu"))
        {
            Debug.Log("Bingo");
            return;
        }
        GetComponent<Image>().color = original;
        KeyBoard.SetActive(false);
        Controller.SetActive(true);

        X = Controller.transform.Find("X").gameObject;
        A = Controller.transform.Find("A").gameObject;
        B = Controller.transform.Find("B").gameObject;
        Y = Controller.transform.Find("Y").gameObject;
        RT = Controller.transform.Find("RT").gameObject;
        start = Controller.transform.Find("Start").gameObject;
        Select = Controller.transform.Find("Select").gameObject;
        LJ = Controller.transform.Find("LJ").gameObject;
        Dpad = Controller.transform.Find("DPad").gameObject;
        RJ = Controller.transform.Find("RJ").gameObject;

        X.GetComponent<TextMeshProUGUI>().text = TextX;
        A.GetComponent<TextMeshProUGUI>().text = TextA;
        B.GetComponent<TextMeshProUGUI>().text = TextB;
        Y.GetComponent<TextMeshProUGUI>().text = TextY;
        RT.GetComponent<TextMeshProUGUI>().text = TextRT;
        start.GetComponent<TextMeshProUGUI>().text = Textstart;
        Select.GetComponent<TextMeshProUGUI>().text = TextSelect;
        LJ.GetComponent<TextMeshProUGUI>().text = TextLJ;
        Dpad.GetComponent<TextMeshProUGUI>().text = TextDpad;
        RJ.GetComponent<TextMeshProUGUI>().text = TextRJ;
    }
    public void ShowKeyboard() {
        if (GameObject.Find("Pause Content") || GameObject.Find("Settings Menu")) return;

        GetComponent<Image>().color = original;

        KeyBoard.SetActive(true);
        Controller.SetActive(false);

        X = KeyBoard.transform.Find("X").gameObject;
        A = KeyBoard.transform.Find("A").gameObject;
        B = KeyBoard.transform.Find("B").gameObject;
        Y = KeyBoard.transform.Find("Y").gameObject;
        RT = KeyBoard.transform.Find("RT").gameObject;
        start = KeyBoard.transform.Find("Start").gameObject;
        Select = KeyBoard.transform.Find("Select").gameObject;
        LJ = KeyBoard.transform.Find("LJ").gameObject;
        Dpad = KeyBoard.transform.Find("DPad").gameObject;
        RJ = KeyBoard.transform.Find("RJ").gameObject;

        X.GetComponent<TextMeshProUGUI>().text      = KeyBoardX     +": "+TextX;
        A.GetComponent<TextMeshProUGUI>().text      = KeyBoardA     +": "+TextA;
        B.GetComponent<TextMeshProUGUI>().text      = KeyBoardB     +": "+TextB;
        Y.GetComponent<TextMeshProUGUI>().text      = KeyBoardY     +": "+TextY;
        RT.GetComponent<TextMeshProUGUI>().text     = KeyBoardRT    +": "+TextRT;
        start.GetComponent<TextMeshProUGUI>().text  = KeyBoardstart +": "+Textstart;
        Select.GetComponent<TextMeshProUGUI>().text = KeyBoardSelect+": "+TextSelect;
        LJ.GetComponent<TextMeshProUGUI>().text     = KeyBoardLJ    +": "+TextLJ;
        Dpad.GetComponent<TextMeshProUGUI>().text   = KeyBoardDpad  +": "+TextDpad;
        RJ.GetComponent<TextMeshProUGUI>().text     = KeyBoardRJ    +": " + TextRJ;

    }

	void Start () {
        Controller = transform.Find("Controller").gameObject;
        KeyBoard = transform.Find("KeyBoard").gameObject;
        Controller.SetActive(false);
        KeyBoard.SetActive(false);
        original = GetComponent<Image>().color;
        GetComponent<Image>().color = new Color(original.r, original.g, original.b, 0);
        
    }

    void HideAll() {
        Controller.SetActive(false);
        KeyBoard.SetActive(false);
        GetComponent<Image>().color = new Color(original.r, original.g, original.b, 0);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Tab))
        {
            if (timePressed == 0.0f) stayOpen = !stayOpen;
            else if (timePressed > 0.23f) stayOpen = false;
            timePressed += Time.unscaledDeltaTime;
            ShowKeyboard();
        }
        else if (InputManager.ButtonSelect)
        {
            if (timePressed == 0.0f) stayOpen = !stayOpen;
            else if (timePressed > 0.23f) stayOpen = false;
            timePressed += Time.unscaledDeltaTime;
            ShowController();
        }
        else
        {
            timePressed = 0;
            if(!stayOpen) HideAll();
        }
    }
}
