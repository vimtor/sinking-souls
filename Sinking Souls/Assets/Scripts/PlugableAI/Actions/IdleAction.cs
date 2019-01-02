using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Actions/Idle")]

public class IdleAction : Action {

    public override void UpdateAction(AIController controller)
    {
        controller.Animator.SetFloat("Speed", 0);
    }
}
