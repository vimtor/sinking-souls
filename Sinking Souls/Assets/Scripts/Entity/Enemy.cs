using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {

    private AIController m_AIController;
    public int m_Souls;

    [HideInInspector] public Ability[] abilities;

    protected void AddForce(float force) {
        Debug.Log("Addforce " + force); 
        m_Rigidbody.velocity = force * transform.forward.normalized;
        Debug.Log("velocity1 " + m_Rigidbody.velocity);
    }
    protected void Stop() {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    private void Start()
    {
        OnStart();

        m_AIController = GetComponent<AIController>();
        abilities = GetComponent<Enemy>().Abilities;
    }

    private void Update()
    {
        if (m_Health <= 0) Die();
        Debug.Log("velocity " + m_Rigidbody.velocity);
    }

    private void Die()
    {
        if (GameController.instance.godMode)
        {
            GameController.instance.SpawnBlueprint(transform.position);
        }

        GameController.instance.RunSouls += m_Souls;
        foreach(GameObject target in GameController.instance.roomEnemies)
        {
            if(target.transform.position == transform.position)
            {
                GameController.instance.roomEnemies.Remove(target);
                break;
            }
        }
        Destroy(gameObject);
    }

}
