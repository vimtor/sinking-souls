using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "PlugableAI/Actions/Scape")]
public class ScapeAction : Action {

    public float range;
    public float minDist;

    private Vector3 targget = Vector3.zero;
    private int layerMask = ~((1 << 12) | (1 << 2) | (1 << 16));

    public override void StartAction(AIController controller) {
        elapsed = false;

        Vector3 direction = controller.player.transform.position - controller.transform.position;

        for(int i = 0; i < 200; i++) {
            targget = RandomNavmeshLocation(controller.player.transform.position);
            if (Vector3.Angle(direction, (targget - controller.transform.position)) > 90 && Vector3.Distance(targget, controller.player.transform.position) > minDist) break;
       
        }

    }

    public override void UpdateAction(AIController controller) {
        Rotate(controller);

        controller.navMeshAgent.enabled = true;
        float speed = Vector3.Magnitude(controller.navMeshAgent.velocity);

        controller.Animator.SetTrigger("Back");

        controller.navMeshAgent.SetDestination(targget);

        RaycastHit hit;
        Physics.Raycast(controller.transform.position, (controller.player.transform.position - controller.transform.position), out hit, Mathf.Infinity, layerMask);

        if (hit.transform.tag != "Player" || Vector3.Distance(controller.transform.position, targget) < 2) {
            elapsed = true;
            controller.Animator.SetTrigger("Forward");

        }
    }

    private void Rotate(AIController controller) {
        Vector3 shooter = controller.gameObject.transform.position;
        Vector3 target = controller.player.transform.position;
        Vector3 direction = target - shooter;

        Vector3 newDir = Vector3.RotateTowards(controller.gameObject.transform.forward, direction, 10 * Time.deltaTime, 0);
        controller.gameObject.transform.rotation = Quaternion.LookRotation(newDir);
    }

    public Vector3 RandomNavmeshLocation(Vector3 center) {
        bool found = false;
        Vector3 finalPosition = Vector3.zero;

        while (!found) {
            Vector3 randomDirection = Random.insideUnitSphere * range;
            randomDirection += center;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomDirection, out hit, range, 1)) {
                finalPosition = hit.position;
                found = true;
            }
        }

        return finalPosition;
    }

}
