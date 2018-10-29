using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Decisions/Distance")]
public class DistanceDecision : Decision {

    [Tooltip("If the player is nearer than this distance, it will return true.")]
    public float minimunDistance;

    public override bool Decide(AIController controller) {
        return (minimunDistance < Vector3.Distance(controller.player.transform.position, controller.transform.position));
    }

}
