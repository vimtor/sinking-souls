using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/EnergyBall")]
public class EnergyBallAbility : Ability {

    protected override void Configure(GameObject Ball) {

        Ball.transform.rotation = Quaternion.LookRotation(GameController.instance.player.transform.position - parent.transform.position);
        Ball.transform.position = parent.GetComponent<Entity>().WeaponHand.transform.position;
        Ball.GetComponent<EnergyBallBehaviour>().GrowingPosition = parent.GetComponent<Entity>().WeaponHand;
    }

}
