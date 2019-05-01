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
        if (other.tag == "Player") if (!isOpened) instantiatedButton = Instantiate(Button, canvas.transform, false);
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player") {
            if (!isOpened) {
                if (Input.GetButtonDown("BUTTON_A") || Input.GetKey(KeyCode.Return)) {
                    isOpened = true;
                    Destroy(instantiatedButton);
                    GetComponent<Animator>().SetTrigger("Open");

                    GiveContent();

                }
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")if(instantiatedButton != null) instantiatedButton.GetComponent<popUpEffect>().destroy();
    }
    
    public void GiveContent() {
        int rand = Random.Range(0, 2);

        if (rand == 0) {// give a modifier
            foreach (Modifier mod in GameController.instance.modifiers) {
                if (!mod.owned) {
                    Debug.Log("You Got: " + mod.name);
                    mod.owned = true;
                    return;
                }
            }
        }
        //give a ability
        foreach (Ability ab in GameController.instance.abilities) {
            if (!ab.owned) {
                Debug.Log("You Got: " + ab.name);

                ab.owned = true;
                return;
            }
        }
        if(rand == 1) {
            foreach (Modifier mod in GameController.instance.modifiers) {
                if (!mod.owned) {
                    Debug.Log("You Got: " + mod.name);

                    mod.owned = true;
                    return;
                }
            }
        }
        //if all unlocked give 50 souls
        Debug.Log("You Got: 50 souls");

        GameController.instance.AddSouls(50);
        
    }

}
