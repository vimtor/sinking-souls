using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Actions/Attack")]
public class AttackAction : Action {

    public override void Act(AIController controller) {
        Debug.Log("pam");
        //controller.enemyAnimator.SetBool("ATTACK");
    }
}
