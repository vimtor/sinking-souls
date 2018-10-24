using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class AIController : MonoBehaviour {

    public State currentState;
    public State remainState;
    public GameObject player;

    [HideInInspector] public Animator animator;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public float stateTimeElapsed;


    private bool aiActive = false;

    public void SetupAI(GameObject _player) {
        stateTimeElapsed = 0;
        aiActive = true;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Enemy>().animator;
        player = _player;

        if (aiActive) navMeshAgent.enabled = true;

        Debug.Log("setupAI");
    }

    void Update() {
        if (!aiActive)
            return;
        currentState.UpdateState(this);
        //Debug.Log(currentState.name);
    }

    public void TransitionToState(State nextState) {
        if(nextState != remainState) {
            currentState = nextState;
            OnExitState();
        }

        //animator.SetBool("RUN", false);
        //animator.SetBool("IDLE", false);
        //animator.SetBool("ATTACK", false);

    }

    public bool CheckIfCountDownElapsed(float duration) {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= duration);
    }

    private void OnExitState() {
        stateTimeElapsed = 0;
        GetComponent<Enemy>().weapon.hitting = false;
        navMeshAgent.enabled = false;
    }

    public void SetAnimBool(string str) {

        animator.SetBool("RUN", false);
        animator.SetBool("IDLE", false);
        animator.SetBool("ATTACK", false);
        animator.SetBool("REACT", false);

        animator.SetBool(str, true);

    }

}
