using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PlugableAI/Decisions/Archer")]
public class ArcherDecision : Decision {

    public State closeNotLocked;//      scape
    public State closeLocked; //        remain (pegar)
    public State notCloseInSight;//     shoot 2
    public State notInSight;//          chase 1
    private int layerMask = ~((1 << 12) | (1 << 2) | (1 << 16));

    public float closeDistance;

    public override bool Decide(AIController controller) {
        RaycastHit hit;
        Physics.Raycast(controller.transform.position + new Vector3(0,1,0), (controller.player.transform.position  - controller.transform.position) , out hit, Mathf.Infinity, layerMask);
        if (hit.transform.tag != "Player") {// no te veo
            controller.CurrentState.transitions[0].trueState = notInSight;
            return true;
        }
        else {// in sight
            if (Vector3.Distance(controller.player.transform.position, controller.transform.position) > closeDistance + 2) {// not close
                controller.CurrentState.transitions[0].trueState = notCloseInSight;
                return true;
            }else {// close
                if(controller.player.GetComponent<Player>().lockedEnemy == controller.gameObject) {//locked
                    controller.CurrentState.transitions[0].trueState = closeLocked;
                    return true;
                }
                else {// not locked
                    controller.CurrentState.transitions[0].trueState = closeNotLocked;
                    return true;
                }
            }
        }
    }

}
