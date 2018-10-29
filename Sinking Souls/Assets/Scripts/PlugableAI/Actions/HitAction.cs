using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Actions/React")]
public class HitAction : Action {

    public override void Act(AIController controller) {

        elapsed = controller.CheckIfCountDownElapsed(controller.GetComponent<Enemy>().clipLength["ReactAnim"]);

        controller.navMeshAgent.enabled = false;
        controller.SetAnimBool("REACT");

    }
}
