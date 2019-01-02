using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "PlugableAI/Actions/Wander")]
public class WanderAction : Action {

    public float range;

    private bool isMoving = false;
    private Vector3 newPosition;

    public override void UpdateAction(AIController controller) {

        Debug.Log("Wandering");

        if (!isMoving) {
            newPosition = RandomNavmeshLocation(controller.transform.position, range);
            controller.navMeshAgent.enabled = true;
            controller.SetAnimBool("RUN");
            controller.navMeshAgent.SetDestination(newPosition);
            isMoving = true;
        }

    }

    public Vector3 RandomNavmeshLocation(Vector3 origin, float radius) {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += origin;

        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, -1)) {
            finalPosition = hit.position;
        }

        return finalPosition;
    }

}
