using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "PlugableAI/Actions/NewStabAction")]
public class NewStabAction : Action
{
    public int m_AttackType;
    public float time = 0;
    public float waitTime = 0;
    private bool fase2 = false;

    public override void StartAction(AIController controller) {
        fase2 = false;
        time = 0;
        base.StartAction(controller);

        // Reset speed float to avoid walking when the animation finishes.
        controller.Animator.SetFloat("Speed", 0);

        // Play the attack animation (which hits via AnimationEvents).
        controller.Animator.SetInteger("AttackType", m_AttackType);
        controller.Animator.SetTrigger("Attack");
        elapsed = false;
    }

    public override void UpdateAction(AIController controller) {
        if (!fase2) {
            if (time >= waitTime) {
                fase2 = true;
                controller.Animator.SetTrigger("Stab");
                time = 0;
            }
            Rotate(controller);
            time += Time.deltaTime;
            controller.stateTimeElapsed = 0;
        }
        else {
            if (time >= 4.17f) {
                elapsed = true;
            }
            time += Time.deltaTime;
        }

    }

    private void Rotate(AIController controller) {
        Vector3 shooter = controller.gameObject.transform.position;
        Vector3 target = controller.player.transform.position;
        Vector3 direction = target - shooter;

        Vector3 newDir = Vector3.RotateTowards(controller.gameObject.transform.forward, direction, 10 * Time.deltaTime, 0);
        newDir = new Vector3(newDir.x, 0, newDir.z);

        controller.gameObject.transform.rotation = Quaternion.LookRotation(newDir);
    }
}