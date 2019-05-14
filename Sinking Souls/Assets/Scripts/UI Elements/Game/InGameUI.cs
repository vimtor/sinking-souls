using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameUI : MonoBehaviour
{
    [Header("Configuration")] public bool displayMinimap;

    [Header("Healthbar Information")]
    public Image healthbar;
    public TextMeshProUGUI soulsAmount;

    [Header("Ability Information")]
    public Image abilityIcon;
    public Image abilityOverlay;
    public TextMeshProUGUI abilityCooldown;
    
    [Header("Minimap")]
    public GameObject minimap;
    public GameObject minimapCamera;
    public float minimapHeight;

    [Header("Minimap Death Island")]
    [HideInInspector] public float minimapSizeDead = 160.0f;
    public float iconSizeDead = 15.0f;

    [Header("Minimap Triton Island")]
    [HideInInspector] public float minimapSizeTriton = 80.0f;
    public float iconSizeTriton = 15.0f;


    private float minimapSize;
    private float iconSize;

    private Player playerRef;
    private GameObject[] icons;


    private void Start()
    {
        //minimapCamera.transform.forward = GameController.instance.currentRoom.GetComponent<SpawnController>().spawnHolder.transform.GetChild(0).gameObject.transform.forward;
        if (minimapCamera != null) {
            minimapCamera.transform.forward = new Vector3(GameObject.Find("Game Camera").transform.forward.x, 0, GameObject.Find("Game Camera").transform.forward.z);
            minimapCamera.transform.Rotate(new Vector3(90, 0, 0));
        }
        //Debug.Log(GameController.instance.player.transform.forward);

        playerRef = GameController.instance.player.GetComponent<Player>();
        abilityIcon.sprite = playerRef.Abilities[0].sprite;

        if (!displayMinimap)
        {
            minimap.SetActive(false);
        }
        else
        {
            icons = GameObject.FindGameObjectsWithTag("Room Icon");

            if (GameController.instance.LevelGenerator.level.name == "DeathIsland")
            {
                minimapSize = minimapSizeDead;
                iconSize = iconSizeDead;
            }
            else
            {
                minimapSize = minimapSizeTriton;
                iconSize = iconSizeTriton;
            }

            ResizeIcons();

            foreach (var icon in icons)
            {
                icon.SetActive(false);
            }
        }
    }

    private void Update()
    {
        healthbar.fillAmount = playerRef.Health / (playerRef.MaxHealth + GameController.instance.extraLife);
        soulsAmount.text = GameController.instance.runSouls.ToString();

        UpdateAbility();

        
        if (displayMinimap)
        {
            UpdateMinimap();
        }
    }

    private void UpdateAbility()
    {
        int cooldown = (int)playerRef.AbilityCooldown;

        if (cooldown > 0) {
            abilityOverlay.enabled = true;
            abilityCooldown.enabled = true;
            abilityCooldown.text = cooldown.ToString();
        }
        else {
            abilityOverlay.enabled = false;
            abilityCooldown.enabled = false;
        }
    }

    public void UpdateMinimap()
    {
        Vector3 newPos = GameController.instance.player.transform.position;
        newPos.y = minimapHeight;
        minimapCamera.transform.position = newPos;

        minimapCamera.GetComponent<Camera>().orthographicSize = minimapSize;

        var currentRoom = GameController.instance.currentRoom;
        currentRoom.GetComponent<doorController>().roomIcon.SetActive(true);
        currentRoom.GetComponent<doorController>().roomUnknown.SetActive(false);

        var adjacentDoors = currentRoom.GetComponentsInChildren<DoorBehaviour>();
        var adjacentRooms = adjacentDoors.Select(door => door.nextDoor.transform.parent.gameObject.GetComponent<doorController>()).ToList();

        foreach (var room in adjacentRooms) {
            if (room.roomIcon.activeSelf == false) {
                room.roomUnknown.SetActive(true);
            }
        }
    }

    private void ResizeIcons()
    {
        foreach (var icon in icons)
        {
            var scale = icon.transform.localScale;
            scale.x = iconSize;
            scale.z = iconSize;

            icon.transform.localScale = scale;
        }
    }
}
    