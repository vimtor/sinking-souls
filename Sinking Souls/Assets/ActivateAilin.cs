using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAilin : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (gameObject.scene.name == "Game") {
            if (GameController.instance.m_RescuedAlchemist == true) Destroy(gameObject);
            GameController.instance.m_RescuedAlchemist = true;
        }
	}
}
