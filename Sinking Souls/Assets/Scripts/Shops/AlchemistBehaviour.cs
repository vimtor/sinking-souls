using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;
using TMPro;


public class AlchemistBehaviour : ShopBehaviour<Ability>
{
    protected override GameObject Configure(GameObject item, Ability ability)
    {
        // Configure the ui item.
        item.transform.Find("Icon").GetComponent<Image>().sprite = ability.sprite;
        item.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = ability.name;
        item.transform.Find("Price").GetComponent<TextMeshProUGUI>().text = ability.price.ToString();
        item.transform.SetParent(shopPanel.transform.GetChild(0), false);

        // Store values in the ShopItem component for easier acces later on.
        item.GetComponent<ShopItem>().price = ability.price;
        item.GetComponent<ShopItem>().ability = ability;

        return item;
    }

    public override void FillShop()
    {
        var abilities = Array.FindAll(GameController.instance.abilities, ability => ability.CanUpgrade());
        Array.ForEach(abilities, ability => SetupItem(ability));
    }
}