using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AlchemistBehaviour : MonoBehaviour
{
    [Header("Shop Interface")]
    public GameObject shopPanel;
    public GameObject UIItem;

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

    private void Start() {
        //lobbySoulsHUD = GameObject.Find("SoulsNumber").GetComponent<Text>();
        //defaultColor = lobbySoulsHUD.color;
    }

    /// <summary>
    /// Carga todas las abilidades disponibles en la tienda.
    /// </summary>
    public void FillShop() {
        bool firstSelected = false;
        totalSouls = GameController.instance.souls;
        foreach (Ability ab in GameController.instance.abilities) {
            GameObject item = Instantiate(UIItem);

            item.transform.Find("Icon").GetComponent<Image>().sprite = ab.sprite;

            item.GetComponent<ShopItem>().price = ab.price;
            item.transform.Find("Price").GetComponent<Text>().text = ab.price.ToString();

            item.transform.Find("Name").GetComponent<Text>().text = ab.name;

            item.transform.SetParent(shopPanel.transform.GetChild(0), false);

            if (!firstSelected) {
                GameObject.Find("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem>().firstSelectedGameObject = item;
                holder = item;
                firstSelected = true;
            }
        }

        maxItems = GameController.instance.abilities.Count;
    }

    public void UpdateShop() {
        shopPanel.transform.GetChild(0).GetChild(currentItem).GetComponentInChildren<Button>().Select();
        holder = shopPanel.transform.GetChild(0).GetChild(currentItem).gameObject;
        remainingSouls = totalSouls - holder.GetComponent<ShopItem>().price;
        price.text = holder.transform.Find("Price").GetComponent<Text>().text;
        remaining.text = remainingSouls.ToString();
    }

    private void Update() {
        if (!GameController.instance.alchemist) return;

        distPlayer = GameController.instance.player.GetComponent<Player>().transform.position - transform.position;

        if (!shopPanel.activeSelf) {
            // Open the store.
            if (InputManager.ButtonA && (distPlayer.magnitude < range)) {
                shopPanel.SetActive(true);
                price = GameObject.Find("SoulsPrice").GetComponent<Text>();
                remaining = GameObject.Find("SoulsRemaining").GetComponent<Text>();
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