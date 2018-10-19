using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Actions/Attack")]
public class AttackAction : Action {

    public override void Act(AIController controller) {
        controller.SetAnimBool("ATTACK");
        /*if(controller.CheckIfCountDownElapsed((controller.GetComponent<Enemy>().clipLength["AttackAnim"]) / 3))*/ controller.GetComponent<Enemy>().weapon.Attack();
        Rotate(controller, 2);
    }

    private void Rotate(AIController controller, float _speed) {
        Vector3 myPos = controller.gameObject.transform.position;
        Vector3 playerPos = controller.player.transform.position;
        Vector3 facingDir = playerPos - myPos;

        Vector3 newDir = Vector3.RotateTowards(controller.gameObject.transform.forward, facingDir, _speed * Time.deltaTime, 0);

        controller.gameObject.transform.rotation = Quaternion.LookRotation(newDir);
    }
}
