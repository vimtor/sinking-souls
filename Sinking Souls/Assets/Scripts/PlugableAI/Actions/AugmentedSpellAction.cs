using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Actions/AugmentedSpell")]
public class AugmentedSpellAction : Action {

    [Range(0.0f, 1.0f)] public float actionFrame = 0.5f;
    public int amount = 4;
    public int range = 20;
    private float clipLength;

    public override void Act(AIController controller) {

        clipLength = controller.GetComponent<Enemy>().clipLength["SpellAnim"];
        controller.SetAnimBool("SPELL");
        if (!controller.GetComponent<Enemy>().AbilityThrown && (controller.CheckIfCountDownElapsed(clipLength * actionFrame))) {
            for (int i = 0; i < amount; i++) {
                controller.GetComponent<Enemy>().ability.Use(controller.gameObject);
                controller.GetComponent<Enemy>().AbilityThrown = false;
            }
            controller.GetComponent<Enemy>().AbilityThrown = true;
        }

        if (controller.CheckIfCountDownElapsed(controller.GetComponent<Enemy>().clipLength["SpellAnim"])) {
            controller.stateTimeElapsed = 0;
            controller.GetComponent<Enemy>().AbilityThrown = false;
        }

    }
}
