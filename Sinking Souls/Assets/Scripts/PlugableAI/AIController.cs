using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class AIController : MonoBehaviour {

    public State currentState;
    public State remainState;

    [HideInInspector] public GameObject player;
    [HideInInspector] public Animator animator;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public float stateTimeElapsed;
    [HideInInspector] public float timeElapsed;
    [HideInInspector] public float externalTime;
    [HideInInspector] public bool aiActive = false;
    [HideInInspector] public bool stop = false;
    [HideInInspector] public bool forceState = false;
    [HideInInspector] public State last;

    [HideInInspector] public AilinBoss Ailin;

    private Ability defaultAbility;

    public virtual void SetupAI() {
        stateTimeElapsed = 0;
        timeElapsed = 0;
        //inRangeTime = 0;
        aiActive = true;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Entity>().animator;
        // player = GameController.instance.player;

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
       // inRangeTime += Time.deltaTime;
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

    //public bool CheckIfTimeTranscurred(float duration)
    //{
    //    return (inRangeTime >= duration);
    //}

    private void OnExitState() {
        stateTimeElapsed = 0;
        timeElapsed = 0;
        GetComponent<Enemy>().weapon.hitting = false;
        GetComponent<Enemy>().thrown = false;
        navMeshAgent.enabled = false;
        if (gameObject.GetComponent<ParticleSystem>()) gameObject.GetComponent<ParticleSystem>().Stop();
        if (stop) {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            stop = false;
        }
        gameObject.GetComponent<Enemy>().weapon.ShrinkCollision();
        // gameObject.GetComponent<Enemy>().ability = defaultAbility;
    }

    public void SetAnimBool(string str) {

        animator.SetBool("RUN", false);
        animator.SetBool("IDLE", false);
        animator.SetBool("ATTACK", false);
        animator.SetBool("REACT", false);
        animator.SetBool("SPELL", false);
        animator.SetBool("TELEPORT", false);
        animator.SetBool("TURN", false);
        animator.SetBool("CHARGESTAB", false);
        animator.SetBool("STAB", false);
        animator.SetBool("ENDSTAB", false);

        animator.SetBool(str, true);

    }

}
