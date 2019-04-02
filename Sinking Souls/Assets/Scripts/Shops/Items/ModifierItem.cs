using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierItem : ShopItem
{
    public Modifier modifier;

    public void Setup(Modifier item)
    {
        modifier = item;
        price = modifier.price;
    }

    protected override void BuyItem()
    {
        GameController.instance.player.GetComponent<Player>().EquipModifier(modifier);
        AudioManager.Instance.PlayEffect("Forge");
    }
}
