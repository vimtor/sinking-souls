using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Enhancer")]
public class Enhancer : ScriptableObject
{
    [Header("Enhancer Information")]
    public Sprite sprite;
    new public string name;
    public string description;

    public int basePrice;
    public int price;
    public float baseEnhancer;
    public float enhancer;

    public float priceMultiplier;
    public float enhancerMultiplier;
    
    public bool life;
    public bool damage;

    public int m_MaxBuys;
    public int m_BuyNumber;

    public void Use()
    {
        if (!CanUse()) return;

        Player playerRef = GameController.instance.player.GetComponent<Player>();

        if (life)
        {
            float currentHealth = playerRef.Health;

            //playerRef.MaxHealth = currentHealth * baseEnhancer;
            playerRef.Health = currentHealth * enhancer;
            GameController.instance.extraLife += playerRef.Health - GameController.instance.player.GetComponent<Player>().MaxHealth;
            GameController.instance.PlayerLifeHolder = playerRef.Health;
            //playerRef.Heal();
        }
        else if (damage)
        {
            playerRef.Weapon.baseDamage *= enhancer;
        }
        else
        {
            Debug.LogError("Enhancer should be either life or damage.");
            return;
        }

        m_BuyNumber++;

        price = (int)(basePrice * priceMultiplier * m_BuyNumber);
        //enhancer = (int)(baseEnhancer * enhancerMultiplier * m_BuyNumber);
    }

    public bool CanUse()
    {
        return true;//m_MaxBuys >= m_BuyNumber;
    }
}