using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;
using TMPro;


public class InnkeeperBehaviour : ShopBehaviour<Enhancer>
{
    public Text m_BaseStat;
    public Text m_UpgradedStat;

    protected override GameObject Configure(GameObject item, Enhancer enhancer)
    {
        item.transform.Find("Icon").GetComponent<Image>().sprite = enhancer.sprite;
        item.transform.Find("Price").GetComponent<TextMeshProUGUI>().text = enhancer.basePrice.ToString();
        item.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = enhancer.name;
        item.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = enhancer.description;

        item.GetComponent<ShopItem>().enhancer = enhancer;

        item.transform.SetParent(m_ShopPanel.transform.GetChild(0), false);

        return item;
    }

    public override void FillShop()
    {
        Enhancer[] enhancers = GameController.instance.enhancers;

        // Select first selected game object.
        m_EventSystem.SetSelectedGameObject(SetupItem(enhancers[0]));

        // Instantiate the rest of the items unless the first one.
        Array.ForEach(enhancers.Skip(1).ToArray(), enhancer => SetupItem(enhancer));
    }

    protected override void UpdateShop()
    {
        base.UpdateShop();

        var enhancer = m_SelectedItem.GetComponent<ShopItem>();
        
        if (enhancer.life)
        {
            int playerHealth = (int)GameController.instance.player.GetComponent<Player>().Health;
            m_BaseStat.text = playerHealth.ToString();

            int upgradedStat = (int)(playerHealth * enhancer.enhancerMultiplier);
            m_UpgradedStat.text = upgradedStat.ToString();
        }
        else if (enhancer.damage)
        {
            int playerDamage = (int)GameController.instance.player.GetComponent<Player>().Weapon.damage;
            m_BaseStat.text = playerDamage.ToString();

            int upgradedStat = (int)(playerDamage * enhancer.enhancerMultiplier);
            m_UpgradedStat.text = upgradedStat.ToString();
        }
    }
}