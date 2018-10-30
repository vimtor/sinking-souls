using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Actions/Idle")]

public class IdleAction : Action {

    public override void Act(AIController controller) {
        controller.SetAnimBool("IDLE");
    }
}
