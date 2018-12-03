using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Loader : MonoBehaviour {

	// Use this for initialization
	void Start () {      
        Debug.Log("DONE");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameController.instance.scene = GameController.GameState.GAME;
            GameController.instance.gameObject.GetComponent<LevelGenerator>().currentLevel++;
            Debug.Log(GameController.instance.gameObject.GetComponent<LevelGenerator>().currentLevel);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }
    }
}
