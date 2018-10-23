using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {

    public enum EnemyType { };
    private EnemyType type;

    private AIController controller;
    public Dictionary<string, float> clipLength = new Dictionary<string, float>();

    private void Start() {
        OnStart();

        controller = GetComponent<AIController>();
        //controller.SetupAI();
        EquipWeapon();

        for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++) {
            var animationClip = animator.runtimeAnimatorController.animationClips[i];
            clipLength.Add(animationClip.name, animationClip.length);
        }
    }



}
