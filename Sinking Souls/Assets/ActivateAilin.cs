using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAilin : MonoBehaviour {

    public LevelGeneratiorConfiguration nextLevel;


	// Use this for initialization
	void Start () {
        if (gameObject.scene.name == "Game") {
            if (GameController.instance.m_RescuedAlchemist == true) Destroy(gameObject);
            else {
                GameController.instance.gameObject.GetComponent<LevelGenerator>().level = nextLevel;
            }
            GameController.instance.m_RescuedAlchemist = true;
        }
	}
}
