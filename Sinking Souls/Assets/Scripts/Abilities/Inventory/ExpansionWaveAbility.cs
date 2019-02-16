using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/ExpansionWave")]
public class ExpansionWaveAbility : Ability
{

    protected override void Configure(GameObject expansionWave)
    {
        expansionWave.transform.position = parent.transform.position;
        expansionWave.transform.parent = parent.transform;
    }

}