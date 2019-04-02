using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemistItem : ShopItem
{
    private Ability ability;

    public void Setup(Ability item)
    {
        ability = item;
        price = ability.price;
    }

    protected override void BuyItem()
    {
        ability.UpgradeAbility();
    }
}
