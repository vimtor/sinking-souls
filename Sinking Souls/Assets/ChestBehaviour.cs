using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBehaviour : MonoBehaviour {

    public GameObject Button;
    private GameObject canvas;
    private GameObject instantiatedButton;
    public AnimationClip Open;
    private bool isOpened = false;

	// Use this for initialization
	void Start () {
        canvas = transform.Find("ChestCanvas").gameObject;
        isOpened = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isOpened) instantiatedButton = Instantiate(Button, canvas.transform, false);
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (!isOpened)
        {
            if (Input.GetButtonDown("BUTTON_A") || Input.GetKey(KeyCode.Return))
            {
                isOpened = true;
                Destroy(instantiatedButton);
                GetComponent<Animator>().SetTrigger("Open");

            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(instantiatedButton != null) instantiatedButton.GetComponent<popUpEffect>().destroy();
    }
    
}
