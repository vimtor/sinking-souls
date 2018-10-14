using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Decisions/Attack")]
public class AttackDecision : Decision {
    

    public override bool Decide(AIController controller) {
        float distance;
        distance = Vector3.Distance(controller.player.transform.position, controller.transform.position);
        //Debug.Log(distance);
        return (distance < 3);

    }
}
