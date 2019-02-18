using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "PlugableAI/Actions/InprovedChase")]
public class InprovedChase : Action {

    public float distancePlayer;
    public Vector3 targetPoint;
    private bool checkDist = true;
   

    public override void StartAction(AIController controller) {
    }

    public override void UpdateAction(AIController controller) {
        if (!controller.started)
        {
            controller.started = true;
            CalculatePoint(controller);
        }
        else targetPoint = controller.player.transform.position + controller.improvedChaseDirection * distancePlayer;

        if (Vector3.Distance(controller.transform.position, controller.player.transform.position) < 1.5f && checkDist)
        {
            Debug.Log("algo");
            checkDist = false;
            controller.StartCoroutine(PickPosition(0.5f, controller));
        }

        if (Vector3.Distance(controller.transform.position, targetPoint) < 1) Rotate(controller);

            elapsed = true;

        controller.navMeshAgent.enabled = true;
        float speed = Vector3.Magnitude(controller.navMeshAgent.velocity);
        controller.Animator.SetFloat("Speed", speed);


        NavMeshHit hit;

        if (NavMesh.SamplePosition(targetPoint, out hit, Mathf.Infinity, NavMesh.AllAreas)) {
            controller.navMeshAgent.SetDestination(hit.position);          
        }
    }

    IEnumerator PickPosition(float time, AIController controller)
    {
            yield return new WaitForSeconds(time);
            checkDist = true;
            if (Vector3.Distance(controller.transform.position, controller.player.transform.position) < 1.5f)
            {
                CalculatePoint(controller);
                checkDist = false;
                controller.StartCoroutine(PickPosition(0.5f, controller));
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

        controller.improvedChaseDirection = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100)).normalized;
        targetPoint = controller.player.transform.position + controller.improvedChaseDirection * distancePlayer;
    
    }

}
