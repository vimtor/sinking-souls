using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Bomb")]
public class BombAbility : Ability {

    public float explotionForce;

    public override void Use(GameObject parent) {

        entity = parent.GetComponent<Entity>();

        if (!entity.thrown) {

            GameObject bomb = SetPrefab(parent);

            bomb.GetComponent<Rigidbody>().AddForce(parent.transform.forward * 50);
            bomb.GetComponent<BombBehaviour>().explotionForce = explotionForce;

            entity.thrown = true;
        }

    }

}
