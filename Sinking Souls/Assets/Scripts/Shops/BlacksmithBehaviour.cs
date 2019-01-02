using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlacksmithBehaviour : MonoBehaviour
{
    [Header("Shop Interface")]
    public GameObject shopPanel;
    public GameObject weaponItemUI;
    public GameObject modifierItemUI;

    [Header("Shop Behaviour")]
    public float scrollDelay;
    public int range = 5;

    // Selected item.
    private GameObject holder;
    private int currentItem = 0;
    private int maxItems;

    // Range of the crewmate.
    private Vector3 distPlayer;

    // HUD info.
    private Text price;
    private int totalSouls;
    [HideInInspector] public int remainingSouls;
    private Text remaining;
    private Text lobbySoulsHUD;
    private Color defaultColor;

    private bool updating = false;

    public void FillShop() {
        bool firstSelected = false;
        totalSouls = GameController.instance.souls;

        if (GameController.instance.weapons.Count == 0)
        {
            Debug.LogError("Weapon list is empty.");
            return;
        }

        foreach (var weapon in GameController.instance.weapons)
        {
            GameObject weaponItem = Instantiate(weaponItemUI);

            weaponItem.transform.Find("Icon").GetComponent<Image>().sprite = weapon.sprite;
            weaponItem.transform.Find("Name").GetComponent<Text>().text = weapon.name;
            weaponItem.transform.SetParent(shopPanel.transform.GetChild(0), false);

            if (!firstSelected) {
                GameObject.Find("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem>().firstSelectedGameObject = weaponItem;
                holder = weaponItem;
                firstSelected = true;
            }

            //foreach(var modifier in GameController.instance.modifiers) {

            //}
        }

        maxItems = GameController.instance.abilities.Count;
    }

    public void UpdateShop() {
        shopPanel.transform.GetChild(0).GetChild(currentItem).GetComponentInChildren<Button>().Select();
        holder = shopPanel.transform.GetChild(0).GetChild(currentItem).gameObject;
    }

    private void Update() {
        if (!GameController.instance.blacksmith) return;

        distPlayer = GameController.instance.player.GetComponent<Player>().transform.position - transform.position;

        if (!shopPanel.activeSelf) {
            // Open the store.
            if (InputManager.ButtonA && (distPlayer.magnitude < range)) {
                shopPanel.SetActive(true);

                UpdateShop();

                // Stop the player.
                GameController.instance.player.GetComponent<Player>().CanMove = false;
            }
        }
        else {
            // Browse the store.
            if (InputManager.LeftJoystick.y == 1 && !updating) {
                updating = true;
                currentItem = (currentItem + 1) % maxItems;
                StartCoroutine(WaitTime());
            }
            else if (InputManager.LeftJoystick.y == -1 && !updating) {
                updating = true;
                currentItem = mod(currentItem - 1, maxItems);
                StartCoroutine(WaitTime());
            }

            // Close the store.
            if (InputManager.ButtonB) {
                shopPanel.SetActive(false);
                GameController.instance.player.GetComponent<Player>().CanMove = true;
                currentItem = 0;
            }
        }
    }

    IEnumerator WaitTime() {
        UpdateShop();
        yield return new WaitForSeconds(scrollDelay);
        updating = false;
    }

    //Module function (takes into account the negative numbers)
    int mod(int x, int m) {
        return (x % m + m) % m;
    }
}
