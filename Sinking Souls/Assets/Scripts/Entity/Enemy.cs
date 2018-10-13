using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {

    public enum EnemyType { };
    private EnemyType type;

    private AIController controller;

    private void Start() {
        OnStart();

        controller = GetComponent<AIController>();
        controller.SetupAI();
        EquipWeapon();
    }


}
