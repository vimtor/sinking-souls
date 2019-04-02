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
        item.transform.SetParent(shopPanel.transform.GetChild(0), false);

        item.GetComponent<InnkeeperItem>().Setup(enhancer);

        return item;
    }

    public override void FillShop()
    {
        var enhancers = GameController.instance.enhancers;
        Array.ForEach(enhancers, enhancer => SetupItem(enhancer));
    }
}