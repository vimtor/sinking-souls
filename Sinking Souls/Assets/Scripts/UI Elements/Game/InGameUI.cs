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

    private Player playerRef;

    private void Start()
    {
        playerRef = GameController.instance.player.GetComponent<Player>();
        abilityIcon.sprite = playerRef.Abilities[0].sprite;

        if (!displayMinimap) minimap.SetActive(false);
    }

    private void Update()
    {
        healthbar.fillAmount = playerRef.Health / playerRef.MaxHealth;
        soulsAmount.text = GameController.instance.runSouls.ToString();

        UpdateAbility();
        if (displayMinimap) UpdateMinimap();
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
    }

}
    