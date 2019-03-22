using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;
using TMPro;
using System.Collections;

public class BlacksmithBehaviour : ShopBehaviour<Modifier>
{
    protected override GameObject Configure(GameObject item, Modifier modifier)
    {
        // Configure the UI item.
        item.transform.Find("Icon").GetComponent<Image>().sprite = modifier.sprite;
        item.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = modifier.name;
        item.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = modifier.description;
        item.transform.Find("Price").GetComponent<TextMeshProUGUI>().text = modifier.price.ToString();
        item.transform.SetParent(shopPanel.transform.GetChild(0), false);

        // Store values in the ShopItem component for easier access later on.
        item.GetComponent<ShopItem>().price = modifier.price;
        item.GetComponent<ShopItem>().modifier = modifier;

        return item;
    }

    public override void FillShop()
    {
        var modifiers = Array.FindAll(GameController.instance.modifiers, modifier => modifier.owned);
        Array.ForEach(modifiers, modifier => SetupItem(modifier));
    }
}