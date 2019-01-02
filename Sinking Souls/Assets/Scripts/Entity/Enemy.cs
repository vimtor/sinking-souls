using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {

    private AIController controller;
    public int souls;

    [HideInInspector] public Ability[] abilities;


    private void Start()
    {
        OnStart();

        controller = GetComponent<AIController>();
        abilities = GetComponent<Enemy>().Abilities;
    }

    private void Update() {
        if (m_Health <= 0) Die();
    }

    private void Die() {
        if (GameController.instance.godMode) {
            GameController.instance.SpawnBlueprint(transform.position);
        }
        Destroy(gameObject);
    }

}
