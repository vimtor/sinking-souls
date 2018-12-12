using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public enum GameState { LOBBY, GAME, ARENA, LOADSCENE, TABERN };

    public GameState scene = GameState.LOBBY;
    public GameObject playerPrefab;

    [HideInInspector] public static GameController instance;
    [HideInInspector] public bool debugMode = false;
    [HideInInspector] public bool godMode = false;
    [HideInInspector] public GameObject currentRoom;
    [HideInInspector] public GameObject player;
    [HideInInspector] public Text lobbySoulsHUD;

    public bool blacksmith = false; // Consider making this a array that holds the unlocked/locked state of each friend.
    public bool alchemist = false;
    public bool innkeeper = false;
    public GameObject blueprint;
    public List<Modifier> modifiers = new List<Modifier>();
    public List<Ability> abilities;
    public List<Enhancer> enhancers;
    public int souls;
    public bool died;

    private LevelGenerator levelGenerator;
    private GameObject shop;

    private void Awake()
    {
        #region SINGLETON
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(gameObject);
        #endregion
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadScene();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public void SpawnBlueprint(Vector3 position)
    {
        var possibleModifiers = modifiers.FindAll(modifer => !modifer.owned && !modifer.picked);
        if (possibleModifiers == null) return;

        var spawnedModifier = possibleModifiers[Random.Range(0, possibleModifiers.Count - 1)];
        GameObject newBlueprint = Instantiate(blueprint);
        newBlueprint.transform.position = position + new Vector3(0, 1, 0);
        newBlueprint.GetComponent<BlueprintBehaviour>().modifier = spawnedModifier;
    }

    public void LoadScene() {
        switch (scene) {
            case GameState.TABERN:
                levelGenerator = GetComponent<LevelGenerator>();
                levelGenerator.takenPos = new List<Vector2>();
                currentRoom = SpawnLevel();
                foreach (GameObject crewMember in GameObject.FindGameObjectsWithTag("CrewMember"))
                {
                    if (innkeeper)
                    {
                        crewMember.SetActive(true);//if we have a list and not just a bool for each change this
                        crewMember.GetComponent<Animator>().SetBool("IDLE", true);
                    }
                }
                SpawnPlayer();
                #region Setup Camera
                CameraManager.instance.player = player.transform;
                CameraManager.instance.SetupCamera(currentRoom.transform.position);
            #endregion

                player.GetComponent<Player>().SetupPlayer();
                //GameObject.Find("Innkeeper").GetComponent<InnkeeperBehaviour>().FillShop();
                player.GetComponent<Player>().health = 100;// the player heals every time he enters the tabern

                break;

            case GameState.GAME:
                died = false;

                #region Setup Initial Room
                levelGenerator = GetComponent<LevelGenerator>();
                levelGenerator.takenPos = new List<Vector2>();
                currentRoom = SpawnLevel();
                currentRoom.GetComponent<SpawnController>().alreadySpawned = true;
                SpawnPlayer();

                player.GetComponent<Player>().SetupPlayer();

                #endregion

                #region Setup Camera
                CameraManager.instance.player = player.transform;
                CameraManager.instance.SetupCamera(currentRoom.transform.position);
                #endregion

                break;

            case GameState.LOBBY:
                gameObject.GetComponent<LevelGenerator>().currentLevel = -1;
                #region Setup Initial Room
                currentRoom = GameObject.Find("Map");
                SpawnPlayer();
                #endregion

                if (!died)
                {
                    modifiers.FindAll(modifier => modifier.picked).ForEach(modifier => modifier.owned = true);
                }

                modifiers.ForEach(modifier => modifier.picked = false);

                lobbySoulsHUD = GameObject.Find("SoulsNumber").GetComponent<Text>();
                lobbySoulsHUD.text = souls.ToString();

                foreach (GameObject crewMember in GameObject.FindGameObjectsWithTag("CrewMember"))
                {
                    if (blacksmith)
                    {
                        crewMember.SetActive(true);//if we have a list and not just a bool for each change this
                        crewMember.GetComponent<Animator>().SetBool("IDLE", true);
                    }
                    else if (alchemist)
                    {
                        crewMember.SetActive(true);//if we have a list and not just a bool for each change this
                        crewMember.GetComponent<Animator>().SetBool("IDLE", true);
                    }
                    else
                    {
                        crewMember.SetActive(false);//if we have a list and not just a bool for each change this
                    }
                }
                player.GetComponent<Player>().SetupPlayer();
                GameObject.Find("Alchemist").GetComponent<AlchemistBehaviour>().FillShop();
                GameObject.FindGameObjectWithTag("Shop").GetComponent<Shop>().FillShop();

                #region Setup Camera
                CameraManager.instance.player = player.transform;
                CameraManager.instance.SetupCamera(currentRoom.transform.position);
            #endregion

                GetComponent<LevelGenerator>().tabernaSpawned = false;
                break;
            case GameState.ARENA:
                currentRoom = GameObject.Find("Arena");
                SpawnPlayer();
                player.GetComponent<Player>().SetupPlayer();
                //currentRoom.GetComponent<SpawnController>().Spawn(player);
                godMode = true;
                CameraManager.instance.player = player.transform;
                CameraManager.instance.SetupCamera(currentRoom.transform.position);
                break;
            default:
                break;
        }
    }

    private void Update()
    {

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("God Mode activated.");
            godMode = !godMode;
        }
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Debug Mode activated.");
            debugMode = !debugMode;
        }

    }

    private GameObject SpawnLevel()
    {
        return levelGenerator.Spawn();
    }

    private void SpawnPlayer()
    {
        player = Instantiate(playerPrefab);
        player.transform.position = currentRoom.transform.position;
    }

    public void ChangeRoom(GameObject door)
    {
        Transform room = door.transform;
        while (room.parent != null)
        {
            room = room.parent.transform;
            if (room.tag == "Room") break;
        }

        currentRoom = room.gameObject;
        room.transform.Find("NavMesh").gameObject.SetActive(true);
        room.GetComponent<SpawnController>().Spawn(player);

    }

}
