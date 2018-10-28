using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Actions/Spell")]
public class SpellAction : Action {

    [Range(0.0f, 1.0f)] public float actionFrame;

    float clipLength;

    public override void Act(AIController controller) {

        clipLength = controller.GetComponent<Enemy>().clipLength["SpellAnim"];
        controller.SetAnimBool("SPELL");

        if (!controller.GetComponent<Enemy>().thrown && (controller.CheckIfCountDownElapsed(clipLength * actionFrame))) {
            controller.GetComponent<Enemy>().ability.Use(controller.gameObject);
        }

        if (controller.CheckIfCountDownElapsed(controller.GetComponent<Enemy>().clipLength["SpellAnim"])) {
            controller.stateTimeElapsed = 0;
            controller.GetComponent<Enemy>().thrown = false;
            
        }

    }

}
