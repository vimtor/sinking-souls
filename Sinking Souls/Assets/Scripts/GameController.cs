using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour {

    public enum GameState { LOBBY, GAME };
    public GameState scene = GameState.LOBBY;
    public static GameController instance;
    public bool debugMode = false;
    public bool godMode = false;
    public GameObject currentRoom;
    public GameObject playerGO;
    [HideInInspector]
    public GameObject player;

    public int blueSouls;
    public int redSouls;
    public int greenSouls;

    private LevelGenerator levelGenerator;

    private void Awake() {

        #region SINGLETON
        if (instance == null) {
            instance = this;
        }
        else if(instance != this) {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(gameObject);
        #endregion

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        LoadScene();
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start () {
        //LoadScene();
    }

    public void LoadScene() {
        switch (scene) {
            case GameState.GAME:
                #region Setup Initial Room
                levelGenerator = GetComponent<LevelGenerator>();
                currentRoom = SpawnLevel();
                currentRoom.GetComponent<SpawnController>().alreadySpawned = true;
                SpawnPlayer();

                player.GetComponent<Player>().SetupPlayer();
                #endregion

                #region Setup Camera
                Camera.main.GetComponent<CameraBehaviour>().player = player.transform;
                Camera.main.GetComponent<CameraBehaviour>().SetupCamera(currentRoom.transform.position);
                #endregion

                blueSouls = 0;
                greenSouls = 0;
                redSouls = 0;
            break;

            case GameState.LOBBY:
                #region Setup Initial Room
                currentRoom = GameObject.Find("Map");
                SpawnPlayer();
                #endregion

                player.GetComponent<Player>().SetupPlayer();

                #region Setup Camera
                Camera.main.GetComponent<CameraBehaviour>().player = player.transform;
                Camera.main.GetComponent<CameraBehaviour>().SetupCamera(currentRoom.transform.position);
                #endregion

            break;
        }
    }

    private void Update() {

        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.G)) {
            Debug.Log("God Mode activated.");
            godMode = !godMode;
        }
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.D)) {
            Debug.Log("Debug Mode activated.");
            debugMode = !debugMode;
        }

    }

    private GameObject SpawnLevel() {
        return levelGenerator.Spawn();
    }

    private void SpawnPlayer() {
        player = Instantiate(playerGO);
        player.transform.position = currentRoom.transform.position;
    }

    public void ChangeRoom(GameObject door) {

        Transform room = door.transform;
        while (room.parent != null) {
            room = room.parent.transform;
            if (room.tag == "Room") break;
        }

        currentRoom = room.gameObject;
        StartCoroutine(Camera.main.GetComponent<CameraBehaviour>().Transition(currentRoom.transform.position));
        room.GetComponent<SpawnController>().Spawn(player);

    }

}
