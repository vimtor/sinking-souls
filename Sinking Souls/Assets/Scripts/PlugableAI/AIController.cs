using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.AI;
using UnityEngine;

public class AIController : MonoBehaviour {

    public State currentState;
    public State remainState;

    [HideInInspector] public GameObject player;
    private Animator m_Animator;
    public Animator Animator
    {
        get { return m_Animator; }
    }

    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public float stateTimeElapsed;
    [HideInInspector] public float timeElapsed;
    [HideInInspector] public float externalTime;
    [HideInInspector] public float count;
    [HideInInspector] public bool aiActive = false;
    [HideInInspector] public bool stop = false;
    [HideInInspector] public bool forceState = false;
    [HideInInspector] public State last;

    [HideInInspector] public AilinBoss Ailin;

    private Ability defaultAbility;

    public virtual void SetupAI() {
        stateTimeElapsed = 0;
        timeElapsed = count = 0;
        //inRangeTime = 0;
        aiActive = true;
        navMeshAgent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();

        defaultAbility = gameObject.GetComponent<Enemy>().ability;
        if (aiActive) navMeshAgent.enabled = true;
    }

    protected virtual void Update() {
        if (!aiActive)
            return;
        if(!player)
            player = GameController.instance.player;

        currentState.UpdateState(this);

        stateTimeElapsed += Time.deltaTime;
        timeElapsed += Time.deltaTime;
        count += Time.deltaTime;
    }

    public void TransitionToState(State nextState) {
        if(nextState != remainState) {
            currentState = nextState;
            OnExitState();
        }
    }

    public bool CheckIfCountDownElapsed(float duration) {
        return (stateTimeElapsed >= duration);
    }

    public bool CheckIfTimeElapsed(float duration) {
        return (timeElapsed >= duration);
    }

    public bool CountElapsed(float duration) {
        return (count >= duration);
    }

    //public bool CheckIfTimeTranscurred(float duration)
    //{
    //    return (inRangeTime >= duration);
    //}

    private void OnExitState()
    {
        try
        {
            stateTimeElapsed = 0;
            timeElapsed = 0;
            GetComponent<Enemy>().AbilityThrown = false;
            navMeshAgent.enabled = false;
            if (gameObject.GetComponent<ParticleSystem>()) gameObject.GetComponent<ParticleSystem>().Stop();
            if (stop)
            {
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                stop = false;
            }

            gameObject.GetComponent<Enemy>().Weapon.ShrinkCollision();
        }
        catch (Exception) {}
    }

    public void SetAnimBool(string str) {

        m_Animator.SetBool("RUN", false);
        m_Animator.SetBool("IDLE", false);
        m_Animator.SetBool("ATTACK", false);
        m_Animator.SetBool("REACT", false);
        m_Animator.SetBool("SPELL", false);
        m_Animator.SetBool("TELEPORT", false);
        m_Animator.SetBool("TURN", false);
        m_Animator.SetBool("CHARGESTAB", false);
        m_Animator.SetBool("STAB", false);
        m_Animator.SetBool("ENDSTAB", false);

        m_Animator.SetBool(str, true);

    }

}
