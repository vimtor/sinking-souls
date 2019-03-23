using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAilin : MonoBehaviour {

    public LevelGeneratiorConfiguration nextLevel;
    public bool Galen = false;
    public bool Ailin = true;

	// Use this for initialization
	void Start () {
        if (Ailin) {
            if (gameObject.scene.name == "Game") {
                if (GameController.instance.m_RescuedAlchemist == true) Destroy(gameObject);
                else {
                    GameController.instance.gameObject.GetComponent<LevelGenerator>().level = nextLevel;
                }
                GameController.instance.m_RescuedAlchemist = true;
            }
        }
        else if (Galen) {
            if (gameObject.scene.name == "Tutorial") {
                if (GameController.instance.m_RescuedBlacksmith == true) Destroy(gameObject);
                else {
                    GameController.instance.gameObject.GetComponent<LevelGenerator>().level = nextLevel;
                }
                GameController.instance.m_RescuedBlacksmith = true;
            }
        }
	}
}
