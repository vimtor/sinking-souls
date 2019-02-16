using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/RageCanon")]
public class RageCanonAbility : Ability {

    protected override void Configure(GameObject canon) {

        canon.GetComponent<RageCanonBehaviour>().position = parent.GetComponent<Entity>().WeaponHand;
    }
}
