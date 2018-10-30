using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/BasicDash")]
public class BasicDash : Ability {

    public float dashSpeed;

    protected override void Configure(GameObject empty) {
        entity.transform.GetComponent<Rigidbody>().velocity = entity.transform.forward.normalized * dashSpeed;
        entity.thrown = false;
    }

}
