using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Decisions/Animation")]
public class AnimationDecision : Decision {

    public override bool Decide(AIController controller) {
        if (controller.CheckIfCountDownElapsed(controller.GetComponent<Enemy>().clipLength["SpellAnim"])) Debug.Log("true");
        return (controller.CheckIfTimeElapsed(controller.GetComponent<Enemy>().clipLength["SpellAnim"] - 0.2f));
    }

}
