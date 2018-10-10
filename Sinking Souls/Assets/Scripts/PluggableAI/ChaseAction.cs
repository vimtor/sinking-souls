using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PlugableAI/Actions/Chase")]
public class ChaseAction : Action {

	public override void Act(AIController controller) {

        controller.navMeshAgent.SetDestination(controller.player.transform.position);
    }

}
