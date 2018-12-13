using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/AilinPilars")]
public class AilinPilarAbility : Ability {

    protected override void Configure(GameObject pilar) {

        pilar.transform.position = new Vector3(0,0.1f,0) + GameController.instance.player.transform.position + GameController.instance.player.GetComponent<Rigidbody>().velocity;
    }

}
