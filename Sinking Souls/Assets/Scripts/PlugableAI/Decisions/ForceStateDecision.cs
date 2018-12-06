using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Decisions/ForceState")]
public class ForceStateDecision : Decision {
    public override bool Decide(AIController controller) {
        if (controller.forceState) {
            controller.forceState = false;
            return true;
        }
        return false;
    }
}

