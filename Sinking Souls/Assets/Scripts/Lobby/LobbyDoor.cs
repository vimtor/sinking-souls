using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LobbyDoor : MonoBehaviour
{
    public ApplicationManager.GameState sceneToLoad;
    private Vector3 _startPosition;
    public GameObject level1controller;
    public GameObject level2controller;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        if (GameObject.Find("Ailin_Boss(Clone)") != null) transform.position = _startPosition + Vector3.down * 5;
        else transform.position = _startPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        switch (sceneToLoad)
        {
            case ApplicationManager.GameState.GAME:
                if (GameController.instance.m_RescuedBlacksmith) {
                    if (ApplicationManager.Instance.state == ApplicationManager.GameState.TABERN) {
                        GameController.instance.LevelGenerator.currentLevel--;
                    }


                    GameController.instance.LevelGenerator.currentLevel++;
                    if (GameController.instance.LevelGenerator.currentLevel == 2 &&
                        !GameController.instance.LevelGenerator.tabernaSpawned) {
                        // TODO: CHANGE THIS TO UNIQUE TAVERN SCENE.
                        ApplicationManager.Instance.ChangeScene(ApplicationManager.GameState.TABERN);
                        return;
                    }
                }
                else {
                    sceneToLoad = ApplicationManager.GameState.TUTORIAL;
                }
                break;
        }

        ApplicationManager.Instance.ChangeScene(sceneToLoad);
    }
}