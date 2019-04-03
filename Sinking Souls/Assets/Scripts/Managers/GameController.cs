using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.PostProcessing;

public class GameController : MonoBehaviour
{
    [Header("Enemies")]
    public GameObject mainEnemy;
    public GameObject casualEnemy;
    public float nextCasualTime;
    public float casualCounter = 0;
    public List<GameObject> roomEnemies;

    [Header("Prefabs")]
    public GameObject playerPrefab;
    public GameObject blueprint;

    #region Crew Members

    [Header("Crew Members")] public bool m_RescuedBlacksmith = false;
    public bool m_RescuedAlchemist = false;

    private GameObject m_BlacksmithObject;
    private GameObject m_AlchemistObject;

    #endregion

    [Header("Items")]
    public int lobbySouls;
    public int runSouls;

    public Modifier[] modifiers;
    public Ability[] abilities;
    public Enhancer[] enhancers;
  
    [Header("Levels")]
    public PostProcessProfile postProcesingProfileLevel1;
    public PostProcessProfile postProcesingProfileLevel2;

    public LevelGeneratiorConfiguration level1;
    public LevelGeneratiorConfiguration level2;

    [HideInInspector] public bool died;
    [HideInInspector] public bool inTavern;
    [HideInInspector] public static GameController instance;
    [HideInInspector] public bool debugMode = false;
    [HideInInspector] public bool godMode = false;
    [HideInInspector] public GameObject currentRoom;
    [HideInInspector] public GameObject player;
    [HideInInspector] public Text lobbySoulsHUD;


    private LevelGenerator levelGenerator;

    public LevelGenerator LevelGenerator
    {
        get { return levelGenerator; }
    }

    private void Awake()
    {
        #region SINGLETON

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        #endregion

        Cursor.visible = false;
        levelGenerator = GetComponent<LevelGenerator>();
    }

    public void SetupScene(ApplicationManager.GameState scene)
    {
        switch (scene)
        {
            case ApplicationManager.GameState.MAIN_MENU:
                break;

            case ApplicationManager.GameState.TABERN:
                inTavern = true;
                currentRoom = GameObject.Find("SpawnPoint");

                var innkeeperBehaviour = FindObjectOfType<InnkeeperBehaviour>();
                innkeeperBehaviour.FillShop();

                SetupGame();
                player.GetComponent<Player>().Heal();
                break;

            case ApplicationManager.GameState.GAME:
                if (m_RescuedBlacksmith) levelGenerator.level = level1;
                if (m_RescuedAlchemist) levelGenerator.level = level2;
                died = false;

                levelGenerator.takenPos = new List<Vector2>();
                currentRoom = SpawnLevel();
                currentRoom.GetComponent<SpawnController>().alreadySpawned = true;

                SetupGame();
                GameObject.Find("Post Processing").gameObject.GetComponent<PostProcessVolume>().profile = postProcesingProfileLevel2;

                if (GetComponent<LevelGenerator>().level.name != "DeathIsland") {
                    player.transform.Find("DeathIsland").gameObject.SetActive(false);
                    GameObject.Find("Post Processing").gameObject.GetComponent<PostProcessVolume>().profile = postProcesingProfileLevel1;
                }
                break;

            case ApplicationManager.GameState.TUTORIAL:
                inTavern = false;
                runSouls = 0;

                levelGenerator.currentLevel = -1;
                currentRoom = GameObject.Find("PlayerSpawn");
                SetupGame();
                player.transform.Find("DeathIsland").gameObject.SetActive(false);
                GameObject.Find("Post Processing").gameObject.GetComponent<PostProcessVolume>().profile = postProcesingProfileLevel1;

            break;

            case ApplicationManager.GameState.LOBBY:
                if (m_RescuedBlacksmith) GetComponent<LevelGenerator>().level = level1;
                if (m_RescuedAlchemist) GetComponent<LevelGenerator>().level = level2;
                AudioManager.Instance.PlayMusic("Waves");

                inTavern = false;
                runSouls = 0;

                levelGenerator.currentLevel = -1;
                currentRoom = GameObject.Find("PlayerSpawn");

                SetupGame();
                player.transform.Find("DeathIsland").gameObject.SetActive(false);

                if (died)
                {
                    runSouls = 0;
                }
                else
                {
                    lobbySouls += runSouls;

                    var pickedModifiers = Array.FindAll(modifiers, modifier => modifier.picked);
                    Array.ForEach(pickedModifiers, modifier => modifier.owned = true);
                }

                Array.ForEach(modifiers, modifier => modifier.picked = false);

                #region Crew Members

                m_BlacksmithObject = GameObject.Find("Galen");
                m_AlchemistObject = GameObject.Find("Ailin");

                m_BlacksmithObject.SetActive(m_RescuedBlacksmith);
                m_AlchemistObject.SetActive(m_RescuedAlchemist);

                if (m_RescuedBlacksmith) m_BlacksmithObject.GetComponent<BlacksmithBehaviour>().FillShop();
                if (m_RescuedAlchemist) m_AlchemistObject.GetComponent<AlchemistBehaviour>().FillShop();

                #endregion

                levelGenerator.tabernaSpawned = false;
                SaveManager.Save();
                break;

            case ApplicationManager.GameState.ARENA:
                currentRoom = GameObject.Find("Arena");

                SetupGame();

                godMode = true;
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            godMode = false;
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            godMode = true;
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            player.transform.position = GetComponent<LevelGenerator>().lastRoom.GetComponent<DoorBehaviour>().nextDoor
                .transform.position;
            roomEnemies = new List<GameObject>();
        }

        if (Input.GetKeyDown(KeyCode.F4)) {
            m_RescuedAlchemist = true;
            m_RescuedBlacksmith = true;
            GetComponent<LevelGenerator>().level = level2;
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            lobbySouls += 100;
        }

        #region ENEMY CONTROLLER
        if (ApplicationManager.Instance.state == ApplicationManager.GameState.MAIN_MENU) return;
        if (ApplicationManager.Instance.state == ApplicationManager.GameState.LOBBY) return;

        if (roomEnemies.Any())
        {
            if (player.GetComponent<Player>().lockedEnemy != null && player.GetComponent<Player>().lockedEnemy != mainEnemy)
                mainEnemy = player.GetComponent<Player>().lockedEnemy;
            else if (mainEnemy == null) mainEnemy = roomEnemies[UnityEngine.Random.Range(0, roomEnemies.Count)];


            // Time for attack for causal enemies.
            if (nextCasualTime < casualCounter)
            {
                casualCounter = 0;
                nextCasualTime = UnityEngine.Random.Range(4, 6);
                casualEnemy = roomEnemies[UnityEngine.Random.Range(0, roomEnemies.Count)];
            }

            casualCounter += Time.deltaTime;
        }
        #endregion

    }


    #region Spawn Functions

    private GameObject SpawnLevel()
    {
        return levelGenerator.Spawn();
    }

    private void SpawnPlayer()
    {
        player = Instantiate(playerPrefab);
        if (!currentRoom.GetComponent<SpawnController>()) player.transform.position = currentRoom.transform.position;
        else player.transform.position = currentRoom.GetComponent<SpawnController>().spawnHolder.transform.GetChild(0).gameObject.transform.position;  
    }

    public void SpawnBlueprint(Vector3 position)
    {
        var possibleModifiers = Array.FindAll(modifiers, modifer => !modifer.owned && !modifer.picked);
        if (possibleModifiers.Length == 0) return;

        var spawnedModifier = possibleModifiers[UnityEngine.Random.Range(0, possibleModifiers.Length - 1)];

        GameObject newBlueprint = Instantiate(blueprint);
        newBlueprint.transform.position = position + new Vector3(0, 1, 0);
        newBlueprint.GetComponent<BlueprintBehaviour>().modifier = spawnedModifier;
    }

    #endregion

    #region Setup Functions

    private void SetupGame()
    {
        SpawnPlayer();
        SetupCamera();
        SetupPlayer();
    }

    private void SetupCamera()
    {
        CameraManager.instance.player = player.transform;
        CameraManager.instance.SetupCamera(currentRoom.transform.position);
    }

    private void SetupPlayer()
    {
        player.GetComponent<Player>().SetupPlayer();
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

    #endregion

    public bool CanBuy(int price)
    {
        return (lobbySouls - price) >= 0;
    }

    public void LoadGame()
    {
        var save = SaveManager.Load();
        if (save == null) return;

        lobbySouls = save.souls;
        runSouls = save.runSouls;
        m_RescuedAlchemist = save.alchemist;
        m_RescuedBlacksmith = save.blacksmith;
        player.GetComponent<Player>().MaxHealth = save.maxHealth;
    }
}

