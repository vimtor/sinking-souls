using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PlugableAI/Actions/Look")]
public class LookAction : Action {

    public override void Act(AIController controller) {
        Look(controller);
    }

    private void Look(AIController controller) {
        Vector3 rotateDir = new Vector3(controller.player.transform.position.x,
                                        controller.transform.position.y,
                                        controller.player.transform.position.z);

        controller.transform.LookAt(rotateDir);
    }

}
