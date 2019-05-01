using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Actions/Idle")]
public class IdleAction : Action
{
    public float distance = 5;
    public Vector2 waitingRange = Vector2.zero;
    public float unlockedMultiplier = 1;

    public bool seen = false;

    private float time = 0;
    private float waitTime = 0;
    private int layerMask = ~((1 << 12) | (1 << 2) | (1 << 16));



    public override void StartAction(AIController controller) {
        time = 0;
        base.StartAction(controller);

        // Reset speed float to avoid walking when the animation finishes.
        controller.Animator.SetFloat("Speed", 0);
        waitTime = Random.Range(waitingRange.x, waitingRange.y);
        controller.waitTime = waitTime;
        // Check if I am the locked enemy.
        if (controller.player.GetComponent<Player>().lockedEnemy != controller.gameObject)
        {
            // Augment the waiting time to avoid overwhelming the player.
            controller.waitTime = waitTime * unlockedMultiplier;
        }

        elapsed = false;
    }

    public override void UpdateAction(AIController controller)
    {
        Rotate(controller);

        if (time >= controller.waitTime)
        {
            elapsed = true;
        }
        else if(!seen)
        {
            if (Vector3.Distance(controller.transform.position, controller.player.transform.position) <= distance) elapsed = true;
            else elapsed = false;
        }
        else
        {
            RaycastHit hit;
           
            Physics.Raycast(controller.transform.position + new Vector3(0,1,0), (controller.player.transform.position - controller.transform.position), out hit, Mathf.Infinity, layerMask);
            elapsed = (hit.transform.tag != "Player");
        }

        time += Time.deltaTime;
    }

    private void Rotate(AIController controller)
    {
        Vector3 shooter = controller.gameObject.transform.position;
        Vector3 target = controller.player.transform.position;
        Vector3 direction = target - shooter;

        Vector3 newDir = Vector3.RotateTowards(controller.gameObject.transform.forward, direction, 10 * Time.deltaTime, 0);
        newDir = new Vector3(newDir.x, 0, newDir.z);

        controller.gameObject.transform.rotation = Quaternion.LookRotation(newDir);
    }
}
