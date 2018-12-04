using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {

    public enum EnemyType { };
    private EnemyType type;

    private AIController controller;
    public Dictionary<string, float> clipLength = new Dictionary<string, float>();
    public Soul soul;

    public Ability ability;


    private void Start() {
        OnStart();

        controller = GetComponent<AIController>();
        EquipWeapon();

        for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++) {
            var animationClip = animator.runtimeAnimatorController.animationClips[i];
            clipLength.Add(animationClip.name, animationClip.length);
        }
    }

    private void Update() {
        if (health <= 0) Die();
    }

    private void Die() {
        soul.Spawn(transform.position);
        if (GameController.instance.godMode) GameController.instance.SpawnBlueprint(transform.position);
        Destroy(gameObject);
    }

}
