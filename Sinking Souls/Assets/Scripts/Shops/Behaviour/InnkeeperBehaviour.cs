using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;
using TMPro;
using System.Collections;

public class InnkeeperBehaviour : ShopBehaviour<Enhancer>
{
    protected override GameObject Configure(GameObject item, Enhancer enhancer)
    {
        item.transform.Find("Icon").GetComponent<Image>().sprite = enhancer.sprite;
        item.transform.Find("Price").GetComponent<TextMeshProUGUI>().text = enhancer.price.ToString();
        item.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = enhancer.name;
        item.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = enhancer.description;
        item.transform.SetParent(shopPanel.transform.GetChild(0), false);

        item.GetComponent<InnkeeperItem>().Setup(enhancer);

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
        else if (!hiding && GameObject.Find("Innkeeper Shop").GetComponent<ShopMenuController>().hit.collider == null)
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
        var enhancers = GameController.instance.enhancers;
        Array.ForEach(enhancers, enhancer => SetupItem(enhancer));
    }
}