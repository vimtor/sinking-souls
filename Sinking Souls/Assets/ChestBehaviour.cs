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
        canvas = GameObject.Find("Canvas");
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
            if (InputManager.ButtonA || Input.GetKey(KeyCode.Return))
            {
                Debug.Log("ENTRA");
                isOpened = true;
                Destroy(instantiatedButton);
                GetComponent<Animator>().SetTrigger("Open");

            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(instantiatedButton != null) Destroy(instantiatedButton);
    }
    
}
