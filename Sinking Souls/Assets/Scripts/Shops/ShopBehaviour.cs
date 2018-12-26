using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ShopBehaviour<T> : MonoBehaviour {

    // Shop interface
    [Header("Shop Interface")]
    public GameObject shopPanel;
    public GameObject UIItem;

    [Header("Shop Behaviour")]
    public float scrollDelay = 0.3f;
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
    private Color defaultColor;

    private bool updating = false;

    protected void Update() {
        if (!GameController.instance.alchemist) return;

        distPlayer = GameController.instance.player.GetComponent<Player>().transform.position - transform.position;

        if (!shopPanel.activeSelf) {
            // Open the store.
            if (InputHandler.ButtonA() && (distPlayer.magnitude < range)) {
                shopPanel.SetActive(true);
                SetupShop();
                UpdateShop();

                // Stop the player.
                GameController.instance.player.GetComponent<Player>().CanMove = false;
            }
        }
        else {
            // Browse the store.
            if (InputHandler.LeftJoystick.y == 1 && !updating) {
                updating = true;
                currentItem = (currentItem + 1) % maxItems;
                StartCoroutine(WaitTime());
            }
            else if (InputHandler.LeftJoystick.y == -1 && !updating) {
                updating = true;
                currentItem = mod(currentItem - 1, maxItems);
                StartCoroutine(WaitTime());
            }

            // Close the store.
            if (InputHandler.ButtonB()) {
                shopPanel.SetActive(false);
                GameController.instance.player.GetComponent<Player>().CanMove = true;
                currentItem = 0;
            }
        }
    }

    protected abstract void SetItem(T commodity, GameObject item);

    protected abstract void UpdateShop();

    protected abstract void SetupShop();

    public void FillShop(List<T> commodities) {
        bool firstSelected = false;
        totalSouls = GameController.instance.souls;

        foreach (T commodity in commodities) {
            GameObject item = Instantiate(UIItem);
            SetItem(commodity, item);

            if (!firstSelected) {
                GameObject.Find("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem>().firstSelectedGameObject = item;
                holder = item;
                firstSelected = true;
            }
        }

        maxItems = GameController.instance.abilities.Count;
    }

    protected IEnumerator WaitTime() {
        UpdateShop();
        yield return new WaitForSeconds(scrollDelay);
        updating = false;
    }

    //Module function (takes into account the negative numbers)
    protected int mod(int x, int m) {
        return (x % m + m) % m;
    }

}
