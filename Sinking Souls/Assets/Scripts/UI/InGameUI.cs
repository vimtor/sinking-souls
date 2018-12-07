using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InGameUI : MonoBehaviour {

    public GameObject souls;
    public GameObject ability;
    public GameObject healthbar;

    [Header("Minimap Information")]
    public GameObject minimap;
    public GameObject minimapCamera;
    public float minimapHeight;

    private Player playerRef;

    private Image healthAmount;
    private Text soulsAmount;
    private Text abilityCooldown;
    private Image abilityOverlay;


    private void Start() {
        healthAmount = healthbar.transform.Find("Amount").GetComponent<Image>();
        soulsAmount = souls.GetComponent<Text>();
        abilityCooldown = ability.transform.Find("Cooldown").GetComponent<Text>();
        abilityOverlay = ability.transform.Find("Overlay").GetComponent<Image>();

        playerRef = GameController.instance.player.GetComponent<Player>();
    }

    private void Update() {
        healthAmount.fillAmount = playerRef.health / playerRef.maxHealth;
        soulsAmount.text = GameController.instance.souls.ToString();

        UpdateAbility();
        UpdateMinimap();
    }

    private void UpdateAbility() {
        int cooldown = (int)playerRef.abilityCooldown;
        Debug.Log(cooldown);

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

    public void UpdateMinimap() {
        Vector3 newPos = GameController.instance.currentRoom.transform.position;
        newPos.y = minimapHeight;
        minimapCamera.transform.position = newPos;
    }

}
