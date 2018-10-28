using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Actions/Spell")]
public class SpellAction : Action {

    public override void Act(AIController controller) {

        controller.SetAnimBool("THROW");
        controller.GetComponent<Enemy>().ability.Use(controller.gameObject);

    }

}
