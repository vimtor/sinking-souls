using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Bomb")]
public class BombAbility : Ability {

    public float explotionForce;

    protected override void Configure(GameObject bomb) {
        bomb.GetComponent<Rigidbody>().AddForce(parent.transform.forward * 50);
        bomb.GetComponent<BombBehaviour>().explotionForce = explotionForce;
    }

}
