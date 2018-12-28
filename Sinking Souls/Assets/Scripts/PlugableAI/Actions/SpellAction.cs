using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Actions/Spell")]
public class SpellAction : Action {

    public override void Act(AIController controller) {

        // Change to trigger when new animator.
        controller.SetAnimBool("SPELL");

    }

}
