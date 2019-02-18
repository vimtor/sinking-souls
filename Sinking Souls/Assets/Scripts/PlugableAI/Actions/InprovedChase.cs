using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "PlugableAI/Actions/InprovedChase")]
public class InprovedChase : Action {

    public float stopingDistance;
    public float enemyDistance;
    public float distancePlayer;
    public Vector3 targetPoint;
   

    public override void StartAction(AIController controller) {
        elapsed = false;
        CalculatePoint(controller);

    }

    public override void UpdateAction(AIController controller) {
        if (!controller.started)
        {
            controller.started = true;
            CalculatePoint(controller);
        }
        else targetPoint = controller.player.transform.position + controller.improvedChaseDirection * distancePlayer;

        Rotate(controller);
        
        

        controller.navMeshAgent.enabled = true;
        float speed = Vector3.Magnitude(controller.navMeshAgent.velocity);
        controller.Animator.SetFloat("Speed", speed);


        NavMeshHit hit;

        if (NavMesh.SamplePosition(targetPoint, out hit, Mathf.Infinity, NavMesh.AllAreas)) {
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

        controller.improvedChaseDirection = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100)).normalized;
        Debug.Log(controller.improvedChaseDirection);
        Debug.Log(this.GetInstanceID());
        targetPoint = controller.player.transform.position + controller.improvedChaseDirection * distancePlayer;
    
    }

}
