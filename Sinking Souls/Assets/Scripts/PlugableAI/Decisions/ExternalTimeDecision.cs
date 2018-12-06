using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Decisions/ExternalTime")]
public class ExternalTimeDecision : Decision {
    public override bool Decide(AIController controller) {
        return (controller.CheckIfTimeElapsed(controller.externalTime));
    }

}
