using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Decisions/Distance/Longer")]

public class DistanceLongerDecision : Decision {
    [Tooltip("True if equals or bigger than this")]
    public float distance = 0;

    public override bool Decide(AIController controller) {
        
        return (distance >= (controller.transform.position - controller.player.transform.position).magnitude);
    }

}
