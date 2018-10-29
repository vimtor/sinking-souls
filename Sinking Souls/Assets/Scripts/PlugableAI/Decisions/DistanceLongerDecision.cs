using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Decisions/Distance/Longer")]
public class DistanceLongerDecision : Decision {

    public float distance;

    public override bool Decide(AIController controller) {
        return (Vector3.Distance(controller.player.transform.position, controller.transform.position) > distance);
    }

}
