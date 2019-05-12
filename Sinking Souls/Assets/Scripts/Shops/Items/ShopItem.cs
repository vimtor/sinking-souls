using UnityEngine;
using TMPro;

public abstract class ShopItem : MonoBehaviour
{
    public TextMeshProUGUI priceText;
    public int price;

    public void Buy()
    {
        if (!GameController.instance.CanBuy(price)) return;
        GameController.instance.lobbySouls -= price;
        BuyItem();
    }

    protected abstract void BuyItem();
}
