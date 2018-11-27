using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PlugableAI/Actions/Chase")]
public class ChaseAction : Action {
    public float speed = 4;
	public override void Act(AIController controller) {
        controller.navMeshAgent.enabled = true;
        controller.navMeshAgent.speed = speed;
        controller.SetAnimBool("RUN");
        controller.navMeshAgent.SetDestination(controller.player.transform.position);
    }

}
