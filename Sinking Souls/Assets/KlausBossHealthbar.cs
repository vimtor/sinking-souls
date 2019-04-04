using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KlausBossHealthbar : BossHealthbar
{
    [HideInInspector] public KlausBossAI klaus;

    protected override float GetCurrentHealth()
    {
        return klaus.life;
    }

    protected override float GetMaxHealth()
    {
        return klaus.life;
    }
}
