using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PlugableAI/Decisions/AilinTeleport")]
public class AilinTeleportDecision : Decision {

    public float minDistance;
    public float reactionTime;


    public override bool Decide(AilinBoss controller)
    {
        if (!(Vector3.Distance(controller.player.transform.position, controller.transform.position) < minDistance)) controller.timeInRange = 0;
        return (controller.CheckIfTimeTranscurred(reactionTime));
    }
}
