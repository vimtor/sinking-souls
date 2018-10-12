using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Abilities/BasicDash")]
public class BasicDash : Ability {

    public float dashSpeed;

    public override void Use(GameObject player) {
        player.GetComponent<Rigidbody>().velocity = player.GetComponent<Transform>().forward.normalized * dashSpeed;
    }

}
