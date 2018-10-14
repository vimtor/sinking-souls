using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Decisions/Chase")]
public class ChaseDecision : Decision {

    public override bool Decide(AIController controller) {
        float distance;
        distance = Vector3.Distance(controller.player.transform.position, controller.transform.position);
        return (distance > 3);// && playerInRoom();
    }
}
