using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Actions/Animation")]
public class ChargeStabAction : Action {
    public string animName;
    private bool done = false;
    public override void Act(AIController controller) {
            controller.SetAnimBool(animName);
    }
}