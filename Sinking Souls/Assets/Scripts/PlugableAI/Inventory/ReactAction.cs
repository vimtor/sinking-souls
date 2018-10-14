using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Actions/React")]
public class ReactAction : Action {

    public override void Act(AIController controller) {
        controller.SetAnimBool("REACT");
    }
}
