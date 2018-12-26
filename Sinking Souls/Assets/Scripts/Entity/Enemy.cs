using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {

    private AIController controller;
    public Dictionary<string, float> clipLength = new Dictionary<string, float>();
    public int souls;

    [HideInInspector] public Ability ability;


    private void Start() {
        OnStart();

        controller = GetComponent<AIController>();
        ability = GetComponent<Enemy>().Ability;

        EquipWeapon();

        for (int i = 0; i < m_Animator.runtimeAnimatorController.animationClips.Length; i++) {
            var animationClip = m_Animator.runtimeAnimatorController.animationClips[i];
            clipLength.Add(animationClip.name, animationClip.length);
        }
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
