using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    private TextMeshProUGUI soulsAmount;
    private TextMeshProUGUI abilityCooldown;
    private Image abilityOverlay;


    private void Start()
    {
        healthAmount = healthbar.transform.Find("Amount").GetComponent<Image>();
        soulsAmount = souls.GetComponent<TextMeshProUGUI>();
        abilityCooldown = ability.transform.Find("Cooldown").GetComponent<TextMeshProUGUI>();
        abilityOverlay = ability.transform.Find("Overlay").GetComponent<Image>();

        playerRef = GameController.instance.player.GetComponent<Player>();
    }

    private void Update()
    {
        healthAmount.fillAmount = playerRef.Health / playerRef.MaxHealth;
        soulsAmount.text = GameController.instance.RunSouls.ToString();

        UpdateAbility();
        UpdateMinimap();
    }

    private void UpdateAbility() {
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

    public void UpdateMinimap() {
        Vector3 newPos = GameController.instance.currentRoom.transform.position;
        newPos.y = minimapHeight;
        minimapCamera.transform.position = newPos;

        GameObject roomIcon = GameController.instance.currentRoom.transform.Find("RoomIcon").gameObject;
        roomIcon.SetActive(true);
    }

}
    