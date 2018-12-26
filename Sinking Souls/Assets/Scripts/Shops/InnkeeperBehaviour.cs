using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InnkeeperBehaviour : MonoBehaviour
{

    //Shop interface
    [Header("ShopInterface")]
    [HideInInspector] public GameObject canvas;
    public GameObject shopPanel;
    public GameObject inGameShopPanel;
    public GameObject UIItem;

    //Selected item
    public GameObject holder;
    public int currentItem = 0;
    public int maxItems;

    //Range of the innkeeper
    public int range = 5;
    private Vector3 distPlayer;

    //HUD info 
    private Text price;
    private int totalSouls;
    [HideInInspector] public int remainingSouls;
    private Text remaining;
    private Text SoulsHUD;
    private Text baseStat;
    private Text upgradedStat;
    private Color defaultColor;

    private bool updating = false;

    private void Start()
    {
        canvas = GameObject.Find("Canvas");
        inGameShopPanel = Instantiate(shopPanel, canvas.transform, false);
        inGameShopPanel.transform.parent = canvas.transform;
        //inGameShopPanel.GetComponent<RectTransform>().position = new Vector2(-6.103516e-05f, 305.78f);
        FillShop();
    }

    /// <summary>
    /// Carga todas las abilidades disponibles en la tienda
    /// </summary>
    public void FillShop()
    {
        bool firstSelected = false;
        totalSouls = GameController.instance.souls;
        foreach (Enhancer en in GameController.instance.enhancers)
        {
            GameObject item = Instantiate(UIItem, inGameShopPanel.transform.GetChild(0), false);
            item.transform.Find("Icon").GetComponent<Image>().sprite = en.sprite;
            item.GetComponent<ShopItem>().price = en.basePrice;
            item.GetComponent<ShopItem>().priceMultiplier = en.priceMultiplier;
            item.transform.Find("Price").GetComponent<Text>().text = en.basePrice.ToString();    
            item.transform.Find("Name").GetComponent<Text>().text = en.name;
            item.transform.Find("Description").GetComponent<Text>().text = en.description;
            item.GetComponent<ShopItem>().baseEnhancer = en.baseEnhancer;
            item.GetComponent<ShopItem>().enhancerMultiplier = en.enhancerMultiplier;
            item.GetComponent<ShopItem>().life = en.life;
            item.GetComponent<ShopItem>().damage = en.damage;

            if (!firstSelected)
            {
                GameObject.Find("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem>().firstSelectedGameObject = item;
                holder = item;
                firstSelected = true;

            }
        }
        maxItems = GameController.instance.enhancers.Count;
    }

    public void UpdateShop()
    {
        inGameShopPanel.transform.GetChild(0).GetChild(currentItem).GetComponentInChildren<Button>().Select();
        holder = inGameShopPanel.transform.GetChild(0).GetChild(currentItem).gameObject;
        remainingSouls = totalSouls - holder.GetComponent<ShopItem>().price;
        price.text = holder.transform.Find("Price").GetComponent<Text>().text;
        remaining.text = remainingSouls.ToString();
        if(holder.GetComponent<ShopItem>().life)
        {
            baseStat.text = "Life: " + GameController.instance.player.GetComponent<Player>().Health.ToString();
            upgradedStat.text = (GameController.instance.player.GetComponent<Player>().Health + GameController.instance.player.GetComponent<Player>().Health * (holder.GetComponent<ShopItem>().baseEnhancer/100f)).ToString();
        }
        else if (holder.GetComponent<ShopItem>().damage)
        {
            baseStat.text = "Damage: " + GameController.instance.player.GetComponent<Player>().Weapon.baseDamage.ToString();
            upgradedStat.text = (GameController.instance.player.GetComponent<Player>().Weapon.baseDamage + GameController.instance.player.GetComponent<Player>().Weapon.baseDamage * (holder.GetComponent<ShopItem>().baseEnhancer / 100f)).ToString();
        }
    }

    private void Update()
    {
        distPlayer = GameController.instance.player.GetComponent<Player>().transform.position - this.transform.position;

        if (!inGameShopPanel.activeSelf)
        {
            //Open the store
            if (InputHandler.ButtonA() && (distPlayer.magnitude < range))
            {
                inGameShopPanel.SetActive(true);
                price = GameObject.Find("SoulsPrice").GetComponent<Text>();
                remaining = GameObject.Find("SoulsRemaining").GetComponent<Text>();
                baseStat = GameObject.Find("BaseStat").GetComponent<Text>();
                upgradedStat = GameObject.Find("UpgradedStat").GetComponent<Text>();
                UpdateShop();
                //Stop the player
                GameController.instance.player.GetComponent<Player>().CanMove = false;

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
                inGameShopPanel.SetActive(false);
                GameController.instance.player.GetComponent<Player>().CanMove = true;
                currentItem = 0;
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