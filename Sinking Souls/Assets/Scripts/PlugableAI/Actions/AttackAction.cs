using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Actions/Attack")]
public class AttackAction : Action
{
    [Header("Configuration")]
    public int m_AttackType;
    public float m_RotationDamping;

    public override void StartAction(AIController controller)
    {
        base.StartAction(controller);

        // Reset speed float to avoid walking when the animation finishes.
        controller.Animator.SetFloat("Speed", 0);

        // Play the attack animation (which hits via AnimationEvents).
        controller.Animator.SetInteger("AttackType", m_AttackType);
        controller.Animator.SetTrigger("Attack");
    }

    public override void UpdateAction(AIController controller)
    {
        elapsed = controller.CheckIfAttackElapsed(m_AttackType);

        // To rotate while attacking.
        Rotate(controller);
    }

    private void Rotate(AIController controller)
    {
        Vector3 shooter = controller.gameObject.transform.position;
        Vector3 target = controller.player.transform.position;
        Vector3 direction = target - shooter;

        Vector3 newDir = Vector3.RotateTowards(controller.gameObject.transform.forward, direction, m_RotationDamping * Time.deltaTime, 0);
        controller.gameObject.transform.rotation = Quaternion.LookRotation(newDir);
    }
}
