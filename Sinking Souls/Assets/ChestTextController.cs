using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChestTextController : MonoBehaviour {
    public string KeyBoard;
    public string controller;

	// Use this for initialization
	void Start () {
		if(InputManager.Xbox_One_Controller > 0) {
            GetComponent<TextMeshPro>().text = controller;
        }else GetComponent<TextMeshPro>().text = KeyBoard;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
