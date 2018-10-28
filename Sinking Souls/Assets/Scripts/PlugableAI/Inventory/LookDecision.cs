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

        Debug.DrawRay(controller.transform.position, controller.transform.forward, Color.green);

        if (Physics.SphereCast(controller.transform.position, 2, controller.transform.forward, out hit)) {
            if (hit.collider.CompareTag("Player")) {

                #region Rotate torwards the target
                Vector3 targetDir = controller.player.transform.position - controller.transform.position;

                // The step size is equal to speed times frame time.
                float step = 5.0f * Time.deltaTime;

                // Move our position a step closer to the target.
                Vector3 newDir = Vector3.RotateTowards(controller.transform.forward, targetDir, step, 0.0f);
                controller.transform.rotation = Quaternion.LookRotation(newDir);
                #endregion

                return true;
            }
        }

        return false;
    }

}
