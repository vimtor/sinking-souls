using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Actions/Idle")]
public class IdleAction : Action
{
    public override void UpdateAction(AIController controller)
    {
        controller.Animator.SetFloat("Speed", 0);
        Rotate(controller);
    }

    private void Rotate(AIController controller)
    {
        Vector3 shooter = controller.gameObject.transform.position;
        Vector3 target = controller.player.transform.position;
        Vector3 direction = target - shooter;

        Vector3 newDir = Vector3.RotateTowards(controller.gameObject.transform.forward, direction, 10 * Time.deltaTime, 0);
        controller.gameObject.transform.rotation = Quaternion.LookRotation(newDir);
    }
}
