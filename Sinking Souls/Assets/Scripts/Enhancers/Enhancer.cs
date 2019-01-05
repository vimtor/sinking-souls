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
    public int baseEnhancer;

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
            float currentHealth = playerRef.MaxHealth;

            playerRef.MaxHealth = currentHealth * baseEnhancer;
            playerRef.Health = currentHealth * baseEnhancer;
        }
        else if (damage)
        {
            playerRef.Weapon.damage *= baseEnhancer;
        }
        else
        {
            Debug.LogError("Enhancer should be either life or damage.");
            return;
        }

        basePrice = (int)(basePrice * priceMultiplier);
        baseEnhancer = (int)(baseEnhancer * enhancerMultiplier);
        m_BuyNumber++;
    }

    public bool CanUse()
    {
        return m_MaxBuys >= m_BuyNumber;
    }
}