using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public enum GameState { LOBBY, GAME, ARENA, LOADSCENE, TABERN };
    public GameState scene = GameState.LOBBY;

    [Header("Prefabs")]
    public GameObject playerPrefab;
    public GameObject blueprint;

    [Header("Crew Members")]
    public bool blacksmith = false;
    public bool alchemist = false;
    
    [Header("Items")]
    public int souls;
    public List<Modifier> modifiers;
    public List<Ability> abilities;
    public List<Enhancer> enhancers;
    
    [HideInInspector] public bool died;
    [HideInInspector] public static GameController instance;
    [HideInInspector] public bool debugMode = false;
    [HideInInspector] public bool godMode = false;
    [HideInInspector] public GameObject currentRoom;
    [HideInInspector] public GameObject player;
    [HideInInspector] public Text lobbySoulsHUD;

    private LevelGenerator levelGenerator;

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

        levelGenerator = GetComponent<LevelGenerator>();
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

    public void LoadScene() {
        switch (scene) {
            case GameState.TABERN:
                levelGenerator.takenPos = new List<Vector2>();
                currentRoom = SpawnLevel();

                GameObject.Find("Innkeeper").SetActive(true);

                SpawnPlayer();
                player.GetComponent<Player>().Heal();

                SetupCamera();

                break;

            case GameState.GAME:
                died = false;
                
                levelGenerator.takenPos = new List<Vector2>();
                currentRoom = SpawnLevel();
                currentRoom.GetComponent<SpawnController>().alreadySpawned = true;

                SpawnPlayer();
                SetupCamera();

                break;

            case GameState.LOBBY:
                levelGenerator.currentLevel = -1;
                currentRoom = GameObject.Find("Map");

                SpawnPlayer();

                if (!died)
                {
                    modifiers.FindAll(modifier => modifier.picked).ForEach(modifier => modifier.owned = true);
                }

                modifiers.ForEach(modifier => modifier.picked = false);

                lobbySoulsHUD = GameObject.Find("SoulsNumber").GetComponent<Text>();
                lobbySoulsHUD.text = souls.ToString();

                GameObject.Find("Blacksmith").SetActive(blacksmith);
                GameObject.Find("Alchemist").SetActive(alchemist);

                GameObject.Find("Alchemist").GetComponent<AlchemistBehaviour>().FillShop();
                GameObject.FindGameObjectWithTag("Shop").GetComponent<Shop>().FillShop();

                SetupCamera();

                levelGenerator.tabernaSpawned = false;
                break;

            case GameState.ARENA:
                currentRoom = GameObject.Find("Arena");
                SpawnPlayer();

                godMode = true;
                SetupCamera();
                break;

            default:
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {

            if (Input.GetKeyDown(KeyCode.F)) {
                debugMode = !debugMode;

                string status = debugMode ? "activated" : "deactivated";
                Debug.Log("Debug mode " + status);
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                godMode = !godMode;

                string status = godMode ? "activated" : "deactivated";
                Debug.Log("God mode " + status);
            }  
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
        player.GetComponent<Player>().SetupPlayer();
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

    private void SetupCamera() {
        CameraManager.instance.player = player.transform;
        CameraManager.instance.SetupCamera(currentRoom.transform.position);
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
