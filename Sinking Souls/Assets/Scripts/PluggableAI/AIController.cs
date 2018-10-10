using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class AIController : MonoBehaviour {

    public State currentState;
    public State remainState;
    public GameObject player;

    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public float stateTimeElapsed;

    private bool aiActive;
    private Animator playerAnimator;

    public void SetupAI() {
        stateTimeElapsed = 0;
        aiActive = true;
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (aiActive) navMeshAgent.enabled = true;
    }

    void Update() {
        if (!aiActive)
            return;
        currentState.UpdateState(this);
    }

    public void TransitionToState(State nextState) {
        if(nextState != remainState) {
            currentState = nextState;
            OnExitState();
        }
        //set all animator bools a false
    }

    public bool CheckIfCountDownElapsed(float duration) {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= duration);
    }

    private void OnExitState() {
        stateTimeElapsed = 0;
    }

    public void SetAnimBool(string str) {
        playerAnimator = player.GetComponent<Enemy>().animator;

        playerAnimator.SetBool("RUN", false);
        playerAnimator.SetBool("IDLE", false);

        playerAnimator.SetBool(str, true);
    }

}
