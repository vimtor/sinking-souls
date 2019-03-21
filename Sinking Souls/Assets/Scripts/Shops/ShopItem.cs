using UnityEngine;
using TMPro;

public class ShopItem : MonoBehaviour
{
    public TextMeshProUGUI m_PriceText;

    [HideInInspector] public int price;
    [HideInInspector] public int baseEnhancer;
    [HideInInspector] public float enhancerMultiplier;
    [HideInInspector] public float priceMultiplier;
    [HideInInspector] public bool life;
    [HideInInspector] public bool damage;

    [HideInInspector] public Modifier modifier;
    [HideInInspector] public Ability ability;
    [HideInInspector] public Enhancer enhancer;


    public void EquipModifier()
    {
        if (GameController.instance.CanBuy(price))
        {
            GameController.instance.player.GetComponent<Player>().EquipModifier(modifier);
            GameController.instance.lobbySouls -= price;
            AudioManager.Instance.PlayEffect("Forge");
        }
    }

    public void UpgradeAbility()
    {
        if (GameController.instance.CanBuy(price))
        {
            ability.UpgradeAbility();
            m_PriceText.text = ability.price.ToString();
            GameController.instance.lobbySouls -= price;
        }
    }

    public void BuyEnhancer()
    {
        if (GameController.instance.CanBuy(price))
        {
            enhancer.Use();
            m_PriceText.text = enhancer.basePrice.ToString();
            GameController.instance.lobbySouls -= price;
        }
    }
}
