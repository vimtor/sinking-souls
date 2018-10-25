using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController instance;

    public bool debugMode = false;
    public bool godMode = false;

    public GameObject player;

    private LevelGenerator levelGenerator;
    public GameObject currentRoom;

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

    private void Start () {

        #region Setup Initial Room
        levelGenerator = GetComponent<LevelGenerator>();
        currentRoom = SpawnLevel(); // SpawnLevel() returns the initial room.
        currentRoom.GetComponent<SpawnController>().alreadySpawned = true;
        SpawnPlayer();

        player.GetComponent<Player>().SetupPlayer();
        #endregion

        #region Setup Camera
        Camera.main.GetComponent<CameraBehaviour>().player = player.transform;
        Camera.main.GetComponent<CameraBehaviour>().SetupCamera(currentRoom.transform.position);
        #endregion

    }

    private void Update() {

        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.G)) {
            Debug.Log("God Mode activated.");
            godMode = !godMode;
        }

    }

    private GameObject SpawnLevel() {
        return levelGenerator.Spawn();
    }

    private void SpawnPlayer() {
        player = Instantiate(player);
        player.transform.position = currentRoom.transform.position;
    }

    public void ChangeRoom(GameObject door) {

        Transform room = door.transform;
        while (room.parent != null) {
            room = room.parent.transform;
            if (room.tag == "Room") break;
        }

        currentRoom = room.gameObject;
        Camera.main.GetComponent<CameraBehaviour>().Transition(currentRoom.transform.position);
        room.GetComponent<SpawnController>().Spawn(player);

    }

}
