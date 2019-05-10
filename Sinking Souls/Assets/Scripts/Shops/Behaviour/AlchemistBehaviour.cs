using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using TMPro;
using System.Collections;


public class AlchemistBehaviour : ShopBehaviour<Ability>
{
    [Header("Alchemist Upgrades")]
    public int upgradeCost = 400;
    public float lifeIncrease = 1.1f;
    public float upgradeMultiplier = 0.1f;

    public int upgradeCounts = 0;
    public int currentPrice;
    public GameObject dialog;

    public void Awake()
    {
        upgradeCounts = GameController.instance.GetComponent<GameController>().upgradeCounts;
        currentPrice = (int)(upgradeCost + (upgradeCost * upgradeMultiplier * upgradeCounts));
    }

    protected override GameObject Configure(GameObject item, Ability ability)
    {
        // Configure the ui item.
        item.transform.Find("Icon").GetComponent<Image>().sprite = ability.sprite;
        item.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = ability.name;
        item.transform.Find("Price").GetComponent<TextMeshProUGUI>().text = ability.price.ToString();
        item.transform.SetParent(shopPanel.transform.GetChild(0), false);

        // Store values in the ShopItem component for easier access later on.
        item.GetComponent<AlchemistItem>().Setup(ability);

        return item;
    }

    public override void FillShop()
    {
        foreach( Ability a in GameController.instance.abilities) {
            if (a.owned) SetupItem(a);
        }
        dialog.transform.GetChild(4).GetComponentInChildren<TextMeshProUGUI>().text = "  Upgrade life for " + currentPrice + " s";
        //Array.ForEach(GameController.instance.abilities, ability => SetupItem(ability));
    }

    public void UpgradeLife()
    {
        if (!GameController.instance.CanBuy(currentPrice)) return;

        GameController.instance.lobbySouls -= currentPrice;
        upgradeCounts++;
        GameController.instance.GetComponent<GameController>().upgradeCounts = upgradeCounts;

        currentPrice = (int)(upgradeCost + (upgradeCost * upgradeMultiplier * upgradeCounts));
        GameController.instance.player.GetComponent<Player>().MaxHealth *= lifeIncrease;
        GameController.instance.PlayerLifeHolder = GameController.instance.player.GetComponent<Player>().MaxHealth;
        GameController.instance.maxHealth = GameController.instance.player.GetComponent<Player>().MaxHealth;
        GameController.instance.player.GetComponent<Player>().Heal();
        dialog.transform.GetChild(4).GetComponentInChildren<TextMeshProUGUI>().text = "  Upgrade life for " + currentPrice + " s";

        SaveManager.Save();
    }

    private bool hiding;
    public override void UpdateMouse()
    {
        if (Math.Abs(InputManager.Mouse.magnitude) > 0.0f)
        {
            hiding = false;
            //Cursor.visible = true;
            GameController.instance.cursor.GetComponent<mouseCursor>().Show();
        }
        else if (!hiding && GameObject.Find("Alchemist Shop").GetComponent<AilinShopMenuController>().hit.collider == null)
        {
            hiding = true;
            StartCoroutine(HideMouse(3.0f));
        }
    }

    private IEnumerator HideMouse(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        if (hiding)
        {
            Cursor.visible = false;
            GameController.instance.cursor.GetComponent<mouseCursor>().Hide();

        }
    }

}
    
       
        
