using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Actions/React")]
public class HitAction : Action
{
    public override void Act(AIController controller)
    {
        controller.navMeshAgent.enabled = false;
        controller.SetAnimBool("REACT");

    }
}
