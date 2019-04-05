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
    
    [Header("Minimap Information")]
    public GameObject minimap;
    public GameObject minimapCamera;
    public float minimapHeight;
    public float minimapSize = 80.0f;
    public float iconSize = 15.0f;

    private Player playerRef;
    private GameObject[] icons;


    private void Start()
    {
        playerRef = GameController.instance.player.GetComponent<Player>();
        abilityIcon.sprite = playerRef.Abilities[0].sprite;

        if (!displayMinimap)
        {
            minimap.SetActive(false);
        }
        else
        {
            icons = GameObject.FindGameObjectsWithTag("Room Icon");

            foreach (var icon in icons)
            {
                icon.SetActive(false);
            }
        }
        
    }

    private void Update()
    {
        healthbar.fillAmount = playerRef.Health / playerRef.MaxHealth;
        soulsAmount.text = GameController.instance.runSouls.ToString();

        UpdateAbility();

        if (displayMinimap)
        {
            UpdateMinimap();
            UpdateIcons();
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
        Vector3 newPos = GameController.instance.currentRoom.transform.position;
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

    private void UpdateIcons()
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
    