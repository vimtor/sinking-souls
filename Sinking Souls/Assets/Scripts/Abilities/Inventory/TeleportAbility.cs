using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Abilities/Teleport")]
public class TeleportAbility : Ability
{
    public float m_Radius;

    public override void Activate()
    {
        if (m_Radius < 1.0f) m_Radius = 10.0f;
        parent.transform.position = RandomNavmeshLocation();
    }

    public Vector3 RandomNavmeshLocation()
    {
        bool found = false;
        Vector3 finalPosition = Vector3.zero;

        while (!found)
        {
            Vector3 randomDirection = Random.insideUnitSphere * m_Radius;
            randomDirection += parent.transform.position;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomDirection, out hit, m_Radius, 1))
            {
                finalPosition = hit.position;
                found = true;
            }
        }

        return finalPosition;
    }
}
