using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Arrow")]
public class ArrowAbility : Ability {

    public override void Use(GameObject parent) {

        entity = parent.GetComponent<Entity>();

        if (!entity.thrown) {

            GameObject arrow = SetPrefab(parent);
            arrow.GetComponent<Rigidbody>().AddForce(parent.transform.forward * 50);

            entity.thrown = true;
        }

    }
}
