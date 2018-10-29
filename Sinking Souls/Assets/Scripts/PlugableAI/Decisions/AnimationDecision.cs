using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Decisions/Animation")]
public class AnimationDecision : Decision {

    public override bool Decide(AIController controller) {
        return (controller.CheckIfCountDownElapsed(controller.GetComponent<Enemy>().clipLength["SpellAnim"]));
    }

}
