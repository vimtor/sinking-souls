using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Decisions/Skeleton MeleeAttack Decision")]
public class SkeletonMeleeAttackDecision : Decision {

    public State strongAttack;
    public State lightAttack;
    public float distance;

    public override bool Decide(AIController controller) {
        if (Vector3.Distance(controller.player.transform.position, controller.transform.position) > distance) return false;

        if (Random.Range(0f,100f) <= 35) controller.CurrentState.transitions[0].trueState = strongAttack;
        else controller.CurrentState.transitions[0].trueState = lightAttack;

        return true;
    }
}
