using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController instance;
    public GameObject player;

    private LevelGenerator levelGenerator;
    public GameObject currentRoom;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if(instance != this) {
            Destroy(this);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start () {
        levelGenerator = GetComponent<LevelGenerator>();
        currentRoom = SpawnLevel();
        currentRoom.GetComponent<SpawnController>().alreadySpawned = true;
        SpawnPlayer();

        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().SetupPlayer();

        Camera.main.GetComponent<CameraBehaviour>().player = GameObject.FindGameObjectWithTag("Player").transform;
        Camera.main.GetComponent<CameraBehaviour>().SetupCamera(currentRoom.transform.position);
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
        Camera.main.GetComponent<CameraBehaviour>().SetupCenter(currentRoom.transform.position);
        room.GetComponent<SpawnController>().Spawn(player);

    }

}
