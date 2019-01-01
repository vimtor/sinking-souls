using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Actions/AugmentedSpell")]
public class AugmentedSpellAction : Action
{
    public override void Act(AIController controller) {

        // clipLength = controller.GetComponent<Enemy>().clipLength["SpellAnim"];
        controller.SetAnimBool("SPELL");

    }
}
