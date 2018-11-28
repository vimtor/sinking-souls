using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [CreateAssetMenu(menuName = "PlugableAI/Actions/Stab")]
    public class StabAction : Action {
        public float dashTime;
        public float dashSpeed;

        public override void Act(AIController controller) {
        controller.transform.GetComponent<Rigidbody>().velocity = controller.transform.forward.normalized * dashSpeed;
        controller.SetAnimBool("ENDSTAB");
        controller.gameObject.GetComponent<Enemy>().weapon.CriticAttack();
        controller.gameObject.GetComponent<Enemy>().weapon.GrowCollision(2);
        if (controller.CheckIfCountDownElapsed(dashTime))
            controller.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
