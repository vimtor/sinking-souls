using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PlugableAI/Actions/Chase")]
public class ChaseAction : Action
{

    public float StartDistance;
    public float StopDistance;

    public override void UpdateAction(AIController controller)
    {
        controller.navMeshAgent.enabled = true;
        float speed = Vector3.Magnitude(controller.navMeshAgent.velocity);
        if (Vector3.Distance(controller.transform.position, controller.player.transform.position) <= StartDistance)
        {
            controller.Animator.SetFloat("Speed", controller.player.GetComponent<Player>().map(Vector3.Distance(controller.transform.position, controller.player.transform.position) - StopDistance, 0, (StartDistance - StopDistance), 0, 1));
        }
        else controller.Animator.SetFloat("Speed", speed);
        controller.navMeshAgent.SetDestination(controller.player.transform.position);
    }
}
