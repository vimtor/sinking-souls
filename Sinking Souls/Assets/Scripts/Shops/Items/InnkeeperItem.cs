using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnkeeperItem : ShopItem
{
    public Enhancer enhancer;

    public void Setup(Enhancer item)
    {
        enhancer = item;
        price = enhancer.basePrice;
    }

    protected override void BuyItem()
    {
        enhancer.Use();
    }
}
