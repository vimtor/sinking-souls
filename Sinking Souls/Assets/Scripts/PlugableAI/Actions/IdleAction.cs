using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Actions/Idle")]
public class IdleAction : Action
{
    public Vector2 waitingRange = Vector2.zero;
    private float time = 0;
    private float waitTime = 0;
    public override void StartAction(AIController controller) {
        time = 0;
        base.StartAction(controller);

        // Reset speed float to avoid walking when the animation finishes.
        controller.Animator.SetFloat("Speed", 0);
        waitTime = Random.Range(waitingRange.x, waitingRange.y);
        Debug.Log("Idle action ");
        elapsed = false;
    }

    public override void UpdateAction(AIController controller)
    {
        Debug.Log("Update");
        Rotate(controller);
        if (time >= waitTime) {
            Debug.Log("Time " + time + ", waitTime " + waitTime);
            elapsed = true;
        }
        time += Time.deltaTime;
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
