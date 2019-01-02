using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Decisions/Look")]
public class LookDecision : Decision
{
    public LayerMask m_LayerMask;

    [Tooltip("From which height the AI can see.")]
    [Range(0.0f, 3.0f)]
    public float m_AttackHeight;

    public override bool Decide(AIController controller)
    {
        return Look(controller);
    }

    private bool Look(AIController controller)
    {
        Vector3 shooter = controller.transform.position;
        Vector3 target = controller.player.transform.position;

        target.y += m_AttackHeight;
        shooter.y += m_AttackHeight;

        Vector3 direction = target - shooter;

        RaycastHit hit;
        if (Physics.Raycast(shooter, direction, out hit, Mathf.Infinity, m_LayerMask))
        {
            if (GameController.instance.debugMode)
            {
                Debug.DrawRay(shooter, direction.normalized * hit.distance, Color.green);
            }

            if (hit.collider.CompareTag("Player"))
            {

                Vector3 rotateDir = new Vector3(controller.player.transform.position.x,
                                                controller.transform.position.y,
                                                controller.player.transform.position.z);

                controller.transform.LookAt(rotateDir);

                return true;
            }

        }
        
        return false;
    }

}
