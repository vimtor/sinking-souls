using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Actions/Turn")]
public class TurnAction : Action {

    public bool waitAnimation;
    public float speed;
    public override void UpdateAction(AIController controller) {
        Vector3 target = controller.player.transform.position - controller.gameObject.transform.position;
        target = new Vector3(target.x, 0, target.z);
        controller.gameObject.GetComponent<Transform>().forward = Vector3.RotateTowards(controller.gameObject.transform.forward, target,speed,0);
        controller.SetAnimBool("TURN");

    }
}
