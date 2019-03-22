using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PlugableAI/Actions/Chase")]
public class ChaseAction : Action
{
    public override void UpdateAction(AIController controller)
    {
        controller.navMeshAgent.enabled = true;
        float speed = Vector3.Magnitude(controller.navMeshAgent.velocity);
        controller.Animator.SetFloat("Speed", speed);
        controller.navMeshAgent.SetDestination(controller.player.transform.position);
    }

}
