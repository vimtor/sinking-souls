using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlacksmithBehaviour : MonoBehaviour {

    public GameObject shopPanel;

	private void Start () {
        /// Talk()
        shopPanel.SetActive(false);
        GameController.instance.blacksmith = true;
	}

    private void Update() {
        if(InputHandler.ButtonA()) {
            if(Vector3.Distance(transform.position, GameController.instance.player.transform.position) < 3) {
                Talk();
            }
        }
    }

    public void Talk() {
        shopPanel.SetActive(true);
    }
	
}
