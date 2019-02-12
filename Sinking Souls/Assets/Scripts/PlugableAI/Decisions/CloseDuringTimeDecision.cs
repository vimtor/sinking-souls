using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Decisions/CloseDuringTime")]
public class CloseDuringTimeDecision : Decision {

    public float time;
    public float distance;
    private bool inRange = false;


    public override bool Decide(AIController controller) {
        if(Vector3.Distance(controller.player.transform.position, controller.gameObject.transform.position) <= distance && inRange == false) {
            inRange = true;
            controller.timeElapsed = 0;
            
        }
        if(Vector3.Distance(controller.player.transform.position, controller.gameObject.transform.position) >= distance)
            inRange = false;
        return (controller.CheckIfTimeElapsed(time) && inRange);
    }
}
