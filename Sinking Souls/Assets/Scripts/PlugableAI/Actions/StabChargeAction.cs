using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Actions/StabChargeAction")]
public class StabChargeAction : Action {
    
    public override void UpdateAction(AIController controller) {
        controller.gameObject.GetComponent<ParticleSystem>().Play();
        controller.SetAnimBool("STAB");
        Rotate(controller, 3);
    }

    private void Rotate(AIController controller, float _speed) {
        Vector3 myPos = controller.gameObject.transform.position;
        Vector3 playerPos = controller.player.transform.position;
        Vector3 facingDir = playerPos - myPos;

        Vector3 newDir = Vector3.RotateTowards(controller.gameObject.transform.forward, facingDir, _speed * Time.deltaTime, 0);

        controller.gameObject.transform.rotation = Quaternion.LookRotation(newDir);
    }
}