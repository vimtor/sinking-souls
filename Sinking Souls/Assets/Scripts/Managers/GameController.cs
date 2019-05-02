using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.PostProcessing;
using TMPro;

public class GameController : MonoBehaviour
{
    [Header("Enemies")]
    public GameObject mainEnemy;
    public GameObject casualEnemy;
    public float nextCasualTime;
    public float casualCounter = 0;
    public List<GameObject> roomEnemies;
    public float PlayerLifeHolder = 100;
    public float extraLife = 0;

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
    public int tabbernSoulsHolder = -1;

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
     public float maxHealth = 300.0f;
    [HideInInspector] public static GameController instance;
    [HideInInspector] public bool debugMode = false;
    [HideInInspector] public bool godMode = false;
    public GameObject currentRoom;
    [HideInInspector] public GameObject player;
    [HideInInspector] public Text lobbySoulsHUD;

    [Header("Chests")]
    public float spawnProvabilty;
    public int minimumPerLevel;
    public int maximumPerLevel;
    public int activeChests = 0;
    public int soulsUISize;

    private LevelGenerator levelGenerator;

    public LevelGenerator LevelGenerator
    {
        get { return levelGenerator; }
    }

    public int percentageOfKeepedSouls;

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
    Coroutine waitToStop;

    private bool growing = false;

    public void AddSouls(int ammount) {
        //runSouls += ammount;
        int soulsPerTick = 1;
        int ticks = ammount;
        if (ticks > 20) {
            soulsPerTick = ammount / 20;
            ticks = 20;
        }
        StartCoroutine(AddSoulRutine(0.1f, ticks, soulsPerTick));
    }
    IEnumerator AddSoulRutine(float t, int count, int soulsPerTick) {

        yield return new WaitForSecondsRealtime(t);
        growing = true;

        AddSoul(soulsPerTick);
        if (waitToStop != null)StopCoroutine(waitToStop);
        if (count > 1) {
            StartCoroutine(AddSoulRutine(t, count - 1, soulsPerTick));
        }
        else waitToStop = StartCoroutine(StopGrowing(1.5f));
    }

    IEnumerator StopGrowing(float t) {
        yield return new WaitForSecondsRealtime(t);
        growing = false;
    }

    private void AddSoul(int tick) {
        runSouls += tick;
        if (GameObject.FindGameObjectWithTag("SoulsUI").transform.localScale.x < 2) {
            GameObject.FindGameObjectWithTag("SoulsUI").transform.localScale += new Vector3(0.06f, 0.06f, 0.06f); // o.1 of size every soul
            GameObject.FindGameObjectWithTag("SoulsUI").GetComponent<TextMeshProUGUI>().color -= new Color(0.1f, 0, 0.1f, 0);
        }

    }

    public void SetupScene(ApplicationManager.GameState scene)
    {
        switch (scene)
        {
            case ApplicationManager.GameState.MAIN_MENU:
                break;

            case ApplicationManager.GameState.TABERN:
                tabbernSoulsHolder = lobbySouls;
                lobbySouls = runSouls;
                inTavern = true;
                currentRoom = GameObject.Find("SpawnPoint");

                var innkeeperBehaviour = FindObjectOfType<InnkeeperBehaviour>();
                innkeeperBehaviour.FillShop();

                SetupGame();
                player.GetComponent<Player>().Heal();
                PlayerLifeHolder = player.GetComponent<Player>().Health;
                player.transform.Find("DeathIsland").gameObject.SetActive(false);

            break;

            case ApplicationManager.GameState.GAME:
                activeChests = 0;
                roomEnemies = new List<GameObject>();
                foreach(Enhancer en in enhancers) {
                    en.price = en.basePrice;
                    en.m_BuyNumber = 0;
                }
                if(tabbernSoulsHolder != -1) {//if ultra feo por la mierda de sistema
                    runSouls = lobbySouls;
                    lobbySouls = tabbernSoulsHolder;
                    tabbernSoulsHolder = -1;
                }
                if (m_RescuedBlacksmith) levelGenerator.level = level1;
                if (m_RescuedAlchemist) levelGenerator.level = level2;
                died = false;

                levelGenerator.takenPos = new List<Vector2>();
                currentRoom = SpawnLevel();
                currentRoom.GetComponent<SpawnController>().alreadySpawned = true;

                SetupGame();
                player.GetComponent<Player>().Health = PlayerLifeHolder;
                GameObject.Find("Post Processing").gameObject.GetComponent<PostProcessVolume>().profile = postProcesingProfileLevel2;

                if (GetComponent<LevelGenerator>().level.name != "DeathIsland")
                {
                    player.transform.Find("DeathIsland").gameObject.SetActive(false);
                    GameObject.Find("Post Processing").gameObject.GetComponent<PostProcessVolume>().profile = postProcesingProfileLevel1;
                    AudioManager.Instance.PlayMusic("TritonTheme");
                }
                else
                {
                    AudioManager.Instance.PlayEffect("Wind");
                    AudioManager.Instance.PlayMusic("DeathTheme");
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
                extraLife = 0;
                if (tabbernSoulsHolder != -1) {//if ultra feo por la mierda de sistema vol 2
                    runSouls = lobbySouls;
                    lobbySouls = tabbernSoulsHolder;
                    tabbernSoulsHolder = -1;
                }
                if (m_RescuedBlacksmith) GetComponent<LevelGenerator>().level = level1;
                if (m_RescuedAlchemist) GetComponent<LevelGenerator>().level = level2;
                AudioManager.Instance.PlayMusic("Waves");

                inTavern = false;

                levelGenerator.currentLevel = -1;
                currentRoom = GameObject.Find("PlayerSpawn");
                
                SetupGame();
                //restart life holder to max health an heal player
                player.GetComponent<Player>().Heal();
                PlayerLifeHolder = player.GetComponent<Player>().Health;
                player.GetComponent<Player>().Weapon.baseDamage = player.GetComponent<Player>().Weapon.originalDamage;
                player.transform.Find("DeathIsland").gameObject.SetActive(false);

                if (died)
                {
                    Debug.Log(" Souls added: " + runSouls * (percentageOfKeepedSouls / 100f));
                    lobbySouls += (int)(runSouls * (percentageOfKeepedSouls / 100f));
                    runSouls = 0;
                }
                else
                {
                    lobbySouls += runSouls;
                    Debug.Log("Did some souls adding");
                    var pickedModifiers = Array.FindAll(modifiers, modifier => modifier.picked);
                    Array.ForEach(pickedModifiers, modifier => modifier.owned = true);
                }
                runSouls = 0;

                Array.ForEach(modifiers, modifier => modifier.picked = false);
                player.GetComponent<Player>().Heal();

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
        if (!growing && GameObject.FindGameObjectWithTag("SoulsUI")) {
            if (GameObject.FindGameObjectWithTag("SoulsUI").transform.localScale.x > soulsUISize) {
                GameObject.FindGameObjectWithTag("SoulsUI").transform.localScale -= new Vector3(1, 1, 1) * 0.7f * Time.deltaTime;
                GameObject.FindGameObjectWithTag("SoulsUI").GetComponent<TextMeshProUGUI>().color += Color.white * 0.1f;

            }
            else {
                GameObject.FindGameObjectWithTag("SoulsUI").transform.localScale = new Vector3(1, 1, 1) * soulsUISize;
                GameObject.FindGameObjectWithTag("SoulsUI").GetComponent<TextMeshProUGUI>().color = Color.white;

            }
        }

        if (lobbySouls < 0) lobbySouls = 0;
        if (runSouls < 0) runSouls = 0;
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("God Mode: FALSE");
            godMode = false;
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Log("God Mode: TRUE");
            godMode = true;
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            player.transform.position = GetComponent<LevelGenerator>().lastRoom.GetComponent<DoorBehaviour>().nextDoor.transform.position - GetComponent<LevelGenerator>().lastRoom.GetComponent<DoorBehaviour>().nextDoor.transform.forward;
            roomEnemies = new List<GameObject>();
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
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


        if (currentRoom != null) {
            if (currentRoom.GetComponent<doorController>() && currentRoom.GetComponent<doorController>().closing == true) {

                checkPlayerdistAndEnemies();
            }
        }

        PlayerLifeHolder = player.GetComponent<Player>().Health;

    }

    bool checkPlayerdistAndEnemies() {
        if (currentRoom.GetComponent<doorController>().checkDistance()) {
            foreach (GameObject en in roomEnemies) {
                if (currentRoom.GetComponent<doorController>().checkEnemyDistance(en)) en.GetComponent<AIController>().SetupAI();
            }
            return false;
        }
        return false;
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
        else {
            player.transform.position = currentRoom.GetComponent<SpawnController>().spawnHolder.transform.GetChild(0).gameObject.transform.position;
            player.transform.rotation = currentRoom.GetComponent<SpawnController>().spawnHolder.transform.GetChild(0).gameObject.transform.rotation;

        }
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

        //spawned enemies in update to enshure distance of the player
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
        maxHealth = save.maxHealth;
    }
}

