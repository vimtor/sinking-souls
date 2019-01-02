using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Actions/React")]
public class HitAction : Action
{
    public override void UpdateAction(AIController controller)
    {
        controller.navMeshAgent.enabled = false;
        controller.SetAnimBool("REACT");

    }
}
