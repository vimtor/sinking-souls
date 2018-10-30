using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewMember : Entity {

    private AIController controller;
    public Dictionary<string, float> clipLength = new Dictionary<string, float>();

    void Start () {
        OnStart();
        controller = GetComponent<AIController>();
        for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++) {
            var animationClip = animator.runtimeAnimatorController.animationClips[i];
            clipLength.Add(animationClip.name, animationClip.length);
        }
        controller.animator = animator;
    }

}
