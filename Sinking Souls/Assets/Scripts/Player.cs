using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity{
    enum State {
        STATE_IDLE,
        STATE_ATTACK,
        STATE_ATTACK2,
        STATE_ATTACK3,
        STATE_MOVEMENT,
        STATE_ABILITY,
        STATE_DASH
    };

    public float dashSpeed;
    private State state;
    private Animator animator;

    private Dictionary<Enemy.EnemyType, int> inventory;
    public AbilitySO dash;
    public AbilitySO ability;

    public void HandleInput() {

        switch (state) {
            case State.STATE_IDLE:
            if (InputHandler.InputInfo.Button == InputHandler.ButtonType.BUTTON_X) {
                Debug.Log("X");
                state = State.STATE_ATTACK;
                animator.SetBool("ATTACK_1", true);
            }
            break;
            case State.STATE_ATTACK:
            break;
            case State.STATE_ATTACK2:
            break;
            case State.STATE_ATTACK3:
            break;
            case State.STATE_MOVEMENT:
            break;
            case State.STATE_ABILITY:
            break;
            case State.STATE_DASH:
            break;
            default:
            break;
        }
    }
    public void Ability() { }
    public void Dash() { }
    public void Attack() { }

    private void Start() {
        state = State.STATE_IDLE;
        animator = GetComponent<Animator>();
    }
    private void Update() {
        HandleInput();
    }
}
