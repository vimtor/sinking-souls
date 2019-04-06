using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class combatCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//per asegurar
	}

    bool checkActiveEnimy() {
        foreach(GameObject en in GameController.instance.roomEnemies) {
            if (en.GetComponent<AIController>().aiActive) return true;
        }
        return false;
    }

    public float speed;
	// Update is called once per frame
	void Update () {
        if(ApplicationManager.Instance.state == ApplicationManager.GameState.GAME) {
            if (!checkActiveEnimy()) {
                zoomOut();
            }
            else {
                zoomIn();
            }
        }
        else {
            GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = 17;
        }
    }

    void zoomIn() {
        if(GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView > 17) {
            GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView -= speed * Time.deltaTime;
        }
    }

    void zoomOut() {
        if (GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView < 25) {
            GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView += speed *0.8f * Time.deltaTime;
        }
    }
}
