using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {

    private AIController m_AIController;
    public int m_Souls;

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
    }

    private void Die()
    {
        if (GameController.instance.godMode)
        {
            GameController.instance.SpawnBlueprint(transform.position);
        }

        GameController.instance.RunSouls += m_Souls;
        Destroy(gameObject);
    }

}
