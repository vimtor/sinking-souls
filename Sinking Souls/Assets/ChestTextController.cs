using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChestTextController : MonoBehaviour {
    public string KeyBoard;
    public string controller;

	// Use this for initialization
	void Start () {
        if (InputManager.Xbox_One_Controller > 0)
        {
            if (GetComponent<TextMeshPro>()) GetComponent<TextMeshPro>().text = controller;
            else GetComponent<TextMeshProUGUI>().text = controller;
        }
        else
        {
            if (GetComponent<TextMeshPro>()) GetComponent<TextMeshPro>().text = KeyBoard;
            else GetComponent<TextMeshProUGUI>().text = KeyBoard;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
