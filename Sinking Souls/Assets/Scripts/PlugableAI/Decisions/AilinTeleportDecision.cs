using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PlugableAI/Decisions/AilinTeleport")]
public class AilinTeleportDecision : Decision {

    public float minDistance;
    public float reactionTime;

    void update(AIController controller)
    {
        if (!(Vector3.Distance(controller.player.transform.position, controller.transform.position) < minDistance)) controller.inRangeTime = 0;
    }

    public override bool Decide(AIController controller)
    {
        return (controller.CheckIfTimeTranscurred(reactionTime));
    }
}
