using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[CreateAssetMenu(menuName = "PlugableAI/Actions/RotativeAttack")]
public class RotativeAttack : Action {
    public float duration;
    public float speed;
    private Vector3 direction;
    public override void StartAction(AIController controller) {
        base.StartAction(controller);

        // Reset speed float to avoid walking when the animation finishes.
        controller.Animator.SetFloat("Speed", 0);

        // Play the attack animation (which hits via AnimationEvents).
        controller.Animator.SetInteger("AttackType", 0);
        controller.Animator.SetTrigger("Attack");
        controller.Animator.SetBool("StopRotation", false);

        Vector3 endpoint = (controller.player.transform.position + controller.player.transform.forward * controller.player.GetComponent<Rigidbody>().velocity.magnitude);
        Vector3 toEndpoint = (controller.player.transform.position + controller.player.transform.forward * controller.player.GetComponent<Rigidbody>().velocity.magnitude) - controller.gameObject.transform.position;
        float timeToEndpoint = toEndpoint.magnitude / speed;
        direction = (controller.player.transform.position + controller.player.transform.forward * controller.player.GetComponent<Rigidbody>().velocity.magnitude * timeToEndpoint) - controller.gameObject.transform.position;
    }

    public override void UpdateAction(AIController controller) {
        controller.navMeshAgent.enabled = true;
        elapsed = false;

        controller.navMeshAgent.speed = speed;

        if (controller.CheckIfCountDownElapsed(duration)) {
            elapsed = true;
            controller.Animator.SetBool("StopRotation", true);
            Debug.Log("OUT");
            controller.navMeshAgent.speed = 3.5f;
        }

        controller.navMeshAgent.SetDestination(controller.gameObject.transform.position + direction.normalized);
    }
}
