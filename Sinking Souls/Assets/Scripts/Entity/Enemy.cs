using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {

    private AIController m_AIController;
    private bool dash;
    public int m_Souls;
    private float dashSpeed;

    [HideInInspector] public Ability[] abilities;



    private void Start()
    {
        OnStart();

        m_AIController = GetComponent<AIController>();
        abilities = GetComponent<Enemy>().Abilities;
    }

    private void Update()
    {
        if (m_Health <= 0) Die();
        if (dash) {
            
        }
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
