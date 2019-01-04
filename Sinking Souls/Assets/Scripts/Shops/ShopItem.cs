using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour {

    [HideInInspector] public int price;
    [HideInInspector] public int baseEnhancer;
    [HideInInspector] public float enhancerMultiplier;
    [HideInInspector] public float priceMultiplier;
    [HideInInspector] public bool life;
    [HideInInspector] public bool damage;
    [HideInInspector] public Modifier modifier;

    public void EquipModifier()
    {
        GameController.instance.player.GetComponent<Player>().EquipModifier(modifier);
    }
}
