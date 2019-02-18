using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "PlugableAI/Actions/InprovedChase")]
public class InprovedChase : Action {

    public float stopingDistance;
    public float enemyDistance;

    public override void StartAction(AIController controller) {
        elapsed = false;
        CalculatePoint(controller);

    }

    public override void UpdateAction(AIController controller) {
        Rotate(controller);
        CalculatePoint(controller);

        controller.navMeshAgent.enabled = true;
        float speed = Vector3.Magnitude(controller.navMeshAgent.velocity);

        controller.Animator.SetFloat("Speed", speed);

        NavMeshHit hit;

        if (NavMesh.SamplePosition(controller.targgetPoint, out hit, Mathf.Infinity, 10)) {
            controller.navMeshAgent.SetDestination(hit.position);
        }
    }

    private void Rotate(AIController controller) {
        Vector3 shooter = controller.gameObject.transform.position;
        Vector3 target = controller.player.transform.position;
        Vector3 direction = target - shooter;

        Vector3 newDir = Vector3.RotateTowards(controller.gameObject.transform.forward, direction, 10 * Time.deltaTime, 0);
        controller.gameObject.transform.rotation = Quaternion.LookRotation(newDir);
    }

    private void CalculatePoint(AIController controller) {
        elapsed = false;
        controller.targgetPoint = controller.player.transform.position - controller.transform.position;
        controller.targgetPoint = controller.player.transform.position - controller.targgetPoint.normalized * stopingDistance;
        List<Vector3> targgets = new List<Vector3>();

        foreach (GameObject go in GameController.instance.roomEnemies) {
            if (go.GetComponent<AIController>().type == AIController.Type.MELEE) {
                if (go.GetComponent<AIController>().targgetPoint != null) {
                    if (Vector3.Distance(go.GetComponent<AIController>().targgetPoint, controller.targgetPoint) < enemyDistance) {
                        targgets.Add(go.GetComponent<AIController>().targgetPoint);
                    }
                }
            }
        }

        if (targgets.Count > 1) {
            float max = 0, min = Mathf.Infinity;
            foreach (Vector3 v in targgets) {
                float dist = Vector3.Distance(v, controller.targgetPoint);
                if (dist < min) min = dist;
                else if (dist > max) max = dist;
            }
            for (int i = 0; i < targgets.Count; i++) {
                targgets[i] = controller.player.GetComponent<Player>().map(targgets[i].magnitude, min, max, max, min) * targgets[i].normalized;
            }
        }

        foreach (Vector3 v in targgets) {
            controller.targgetPoint -= v;
        }
    }

}
