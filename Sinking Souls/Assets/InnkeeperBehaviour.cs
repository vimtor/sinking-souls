using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InnkeeperBehaviour : MonoBehaviour
{

    //Shop interface
    [Header("ShopInterface")]
    public GameObject shopPanel;
    public GameObject UIItem;

    //Selected item
    private GameObject holder;
    private int currentItem = 0;
    private int maxItems;

    //Range of the innkeeper
    public int range = 5;
    private Vector3 distPlayer;

    //HUD info 
    private Text price;
    private int totalSouls;
    [HideInInspector] public int remainingSouls;
    private Text remaining;
    private Text SoulsHUD;
    private Color defaultColor;

    private bool updating = false;

    private void Start()
    {
        GameController.instance.inkeeper = true;
        FillShop();
    }

    /// <summary>
    /// Carga todas las abilidades disponibles en la tienda
    /// </summary>
    public void FillShop()
    {
        bool firstSelected = false;
        totalSouls = GameController.instance.souls;
        foreach (Ability ab in GameController.instance.abilities)
        {
            GameObject item = Instantiate(UIItem);
            item.transform.Find("Icon").GetComponent<Image>().sprite = ab.sprite;
            item.GetComponent<ShopItem>().price = ab.price;
            item.transform.Find("Price").GetComponent<Text>().text = ab.price.ToString();
            item.transform.Find("Name").GetComponent<Text>().text = ab.name;
            item.gameObject.transform.SetParent(shopPanel.transform.GetChild(0), false);

            if (!firstSelected)
            {
                GameObject.Find("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem>().firstSelectedGameObject = item;
                holder = item;
                firstSelected = true;

            }
        }
        maxItems = GameController.instance.abilities.Count;
    }

    public void UpdateShop()
    {
        shopPanel.transform.GetChild(0).GetChild(currentItem).GetComponentInChildren<Button>().Select();
        holder = shopPanel.transform.GetChild(0).GetChild(currentItem).gameObject;
        remainingSouls = totalSouls - holder.GetComponent<ShopItem>().price;
        price.text = holder.transform.Find("Price").GetComponent<Text>().text;
        remaining.text = remainingSouls.ToString();
    }

    private void Update()
    {
        distPlayer = GameController.instance.player.GetComponent<Player>().transform.position - this.transform.position;

        if (GameController.instance.alchemist)
        {
            if (!shopPanel.activeSelf)
            {
                //Open the store
                if (InputHandler.ButtonA() && (distPlayer.magnitude < range))
                {
                    shopPanel.SetActive(true);
                    price = GameObject.Find("SoulsPrice").GetComponent<Text>();
                    remaining = GameObject.Find("SoulsRemaining").GetComponent<Text>();
                    UpdateShop();
                    //Stop the player
                    GameController.instance.player.GetComponent<Player>().state = Player.State.IDLE;
                    GameController.instance.player.GetComponent<Player>().HandleInput();
                    GameController.instance.player.GetComponent<Player>().move = false;

                }
            }
            else
            {
                //Browse the store
                if (InputHandler.LeftJoystick.y == 1 && !updating)
                {
                    updating = true;
                    currentItem = (currentItem + 1) % maxItems;
                    StartCoroutine(waitTime(0.3f));
                }
                else if (InputHandler.LeftJoystick.y == -1 && !updating)
                {
                    updating = true;
                    currentItem = mod(currentItem - 1, maxItems);
                    StartCoroutine(waitTime(0.3f));
                }
                //Close the store
                if (InputHandler.ButtonB())
                {
                    shopPanel.SetActive(false);
                    GameController.instance.player.GetComponent<Player>().move = true;
                    currentItem = 0;
                }
            }
        }
    }

    IEnumerator waitTime(float time)
    {
        UpdateShop();
        yield return new WaitForSeconds(time);
        updating = false;
    }

    //Module function (takes into account the negative numbers)
    int mod(int x, int m)
    {
        return (x % m + m) % m;
    }

}