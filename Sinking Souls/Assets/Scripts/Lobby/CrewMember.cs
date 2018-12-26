using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewMember : Entity {

    private AIController controller;
    public Dictionary<string, float> clipLength = new Dictionary<string, float>();

    void Start () {
        OnStart();
        controller = GetComponent<AIController>();
        for (int i = 0; i < m_Animator.runtimeAnimatorController.animationClips.Length; i++) {
            var animationClip = m_Animator.runtimeAnimatorController.animationClips[i];
            clipLength.Add(animationClip.name, animationClip.length);
        }
        controller.animator = m_Animator;
    }

}
