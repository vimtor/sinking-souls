using UnityEngine;
using TMPro;

public abstract class ShopItem
{
    public TextMeshProUGUI priceText;
    public int price;

    public void Buy()
    {
        if (!GameController.instance.CanBuy(price)) return;

        BuyItem();
        GameController.instance.lobbySouls -= price;
    }

    protected abstract void BuyItem();
}
