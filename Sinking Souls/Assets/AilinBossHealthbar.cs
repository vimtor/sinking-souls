using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AilinBossHealthbar : BossHealthbar
{
    [HideInInspector] public Enemy ailin;

    protected override float GetCurrentHealth()
    {
        return ailin.Health;
    }

    protected override float GetMaxHealth()
    {
        return ailin.MaxHealth;
    }
}
