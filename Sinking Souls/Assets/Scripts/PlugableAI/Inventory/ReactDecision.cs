using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Decisions/React")]
public class ReactDecision : Decision {

    public override bool Decide(AIController controller) {

        if (!controller.CheckIfCountDownElapsed((controller.GetComponent<Enemy>().clipLength["ReactAnim"]) / 2)) {  
            return controller.GetComponent<Enemy>().hit;
        }

        if (controller.stateTimeElapsed >= controller.GetComponent<Enemy>().clipLength["ReactAnim"]) controller.stateTimeElapsed = 0;
        return false;
    }
}
