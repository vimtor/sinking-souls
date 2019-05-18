using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.AI;
using UnityEngine;

public class AIController : MonoBehaviour {

    public State m_CurrentState;
    [HideInInspector] public bool main;
    [HideInInspector] public enum Type { MELEE, ARCHER, SORCERER};
    public bool started = false;
    public Vector3 improvedChaseDirection;
    public Type type;
    public State CurrentState
    {
        get { return m_CurrentState; }
        set { m_CurrentState = value; }
    }

    public State remainState;

    
    private Animator m_Animator;
    public Animator Animator
    {
        get { return m_Animator; }
    }

    private float[] m_AttackLengths;

    [HideInInspector] public GameObject player;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public float stateTimeElapsed;
    [HideInInspector] public float timeElapsed;
    [HideInInspector] public float externalTime;
    [HideInInspector] public float count;
    public float waitTime;
    public bool aiActive = false;
    [HideInInspector] public bool stop = false;
    [HideInInspector] public bool forceState = false;
    [HideInInspector] public State last;

    [HideInInspector] public AilinBoss Ailin;

    private Ability defaultAbility;

    public virtual void SetupAI()
    {
        
        stateTimeElapsed = 0;
        timeElapsed = count = 0;
        //inRangeTime = 0;
        aiActive = true;
        navMeshAgent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();

        var animationClips = m_Animator.runtimeAnimatorController.animationClips;
        var attackClips = Array.FindAll(animationClips, clip => clip.name.Contains("Attack"));
        m_AttackLengths = attackClips.Select(clip => clip.length).ToArray();

        // defaultAbility = gameObject.GetComponent<Enemy>().ability;
        if (aiActive) navMeshAgent.enabled = true;
    }

    protected virtual void Update()
    {
        if (!aiActive) return;
        if (!player) player = GameController.instance.player;

        m_CurrentState.UpdateState(this);

        stateTimeElapsed += Time.deltaTime;
        timeElapsed += Time.deltaTime;
        count += Time.deltaTime;
    }

    public void TransitionToState(State nextState)
    {
        if (nextState != remainState)
        {
            m_CurrentState = Instantiate(nextState);
            OnExitState();
        }
    }

    public bool CheckIfCountDownElapsed(float duration) {
        return (stateTimeElapsed >= duration);
    }

    public bool CheckIfTimeElapsed(float duration) {
        return (timeElapsed >= duration);
    }

    public bool CheckIfAttackElapsed(int attackID)
    {
        return (stateTimeElapsed >= m_AttackLengths[attackID]);
    }

    public bool CheckIfAttackElapsed(int attackID, float speed)
    {
        return (stateTimeElapsed >= m_AttackLengths[attackID]/speed);
    }

    public bool CountElapsed(float duration) {
        return (count >= duration);
    }

    private void OnExitState()
    {
        // To avoid skipping the next state start actions.
        m_CurrentState.InitialActionsDone = false;

        // Reset time counters.
        stateTimeElapsed = 0.0f;

        // Reset the rest of variables needed.
        navMeshAgent.enabled = false;
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
