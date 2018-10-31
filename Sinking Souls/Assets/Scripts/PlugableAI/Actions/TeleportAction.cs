using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu (menuName = "PlugableAI/Actions/Teleport")]
public class TeleportAction : Action {

    [Range(0.0f, 1.0f)] public float actionFrame;

    public override void Act(AIController controller) {

        controller.SetAnimBool("TELEPORT");
        float clipLenght = controller.GetComponent<Enemy>().clipLength["TeleportAnim"];

        elapsed = controller.CheckIfCountDownElapsed(clipLenght);

        if (controller.CheckIfCountDownElapsed(clipLenght * actionFrame)) {
            Vector3 newPosition = RandomNavmeshLocation(controller, 10);
            controller.transform.position = newPosition;
            elapsed = true;
        }
        
    }

    public Vector3 RandomNavmeshLocation(AIController controller, float radius) {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += controller.transform.position;
        NavMeshHit hit;

        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1)) {
            finalPosition = hit.position;
        }

        return finalPosition;
    }

}
