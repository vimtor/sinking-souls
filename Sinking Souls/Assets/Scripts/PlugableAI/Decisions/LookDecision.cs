using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Decisions/Look")]
public class LookDecision : Decision {

    public override bool Decide(AIController controller) {
        bool targetVisible = Look(controller);
        return targetVisible;
    }

    private bool Look(AIController controller) {
        RaycastHit hit;
        Vector3 targetDir = controller.player.transform.position - controller.transform.position;
        LayerMask layerMask = 1 << 14;

        if (Physics.Raycast(controller.transform.position, targetDir, out hit, Mathf.Infinity, ~layerMask)) {

            if (GameController.instance.debugMode) {
                Debug.DrawRay(controller.transform.position, targetDir.normalized * hit.distance, Color.green);
            }

            if (hit.collider.tag == "Player") {

                #region Rotate torwards the target
                Vector3 rotateDir = new Vector3(controller.player.transform.position.x,
                                                controller.transform.position.y,
                                                controller.player.transform.position.z);

                controller.transform.LookAt(rotateDir);
                #endregion

                return true;
            }
        }
        
        return false;
    }

}
