using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InnkeeperItem : ShopItem
{
    private Enhancer enhancer;

    public void Setup(Enhancer item)
    {
        enhancer = item;
        price = enhancer.price;
    }

    protected override void BuyItem()
    {
        enhancer.Use();
        price = enhancer.price;
        transform.Find("Price").GetComponent<TextMeshProUGUI>().text = price.ToString();

        AudioManager.Instance.PlayEffect("TavernItem");
    }
}
