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
        item.GetComponent<ModifierItem>().Setup(modifier);

        return item;
    }

    private bool hiding;
    public override void UpdateMouse()
    {
        if (Math.Abs(InputManager.Mouse.magnitude) > 0.0f)
        {
            hiding = false;
            Cursor.visible = true;
        }
        else if (!hiding && GameObject.Find("Blacksmith Shop").GetComponent<ShopMenuController>().hit.collider == null)
        {
            hiding = true;
            StartCoroutine(HideMouse(3.0f));
        }
    }

    private IEnumerator HideMouse(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        if (hiding) Cursor.visible = false;
    }

    public override void FillShop()
    {
        var modifiers = Array.FindAll(GameController.instance.modifiers, modifier => modifier.owned);
        Array.ForEach(modifiers, modifier => SetupItem(modifier));
    }
}