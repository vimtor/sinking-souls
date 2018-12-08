using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PlugableAI/Actions/AilinReact")]
public class AilinHitAction : Action
{

    public bool waitAnimation;

    public override void Act(AilinBoss controller)
    {
        controller.rageTime = 0;
        //Default reaction
        if (waitAnimation)
            elapsed = controller.CheckIfCountDownElapsed(controller.GetComponent<Enemy>().clipLength["ReactAnim"]);

        controller.navMeshAgent.enabled = false;
        controller.SetAnimBool("REACT");

    }
}
   
