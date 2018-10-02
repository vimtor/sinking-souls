using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity{
    enum State {
        STATE_IDLE,
        STATE_ATTACK_1,
        STATE_ATTACK_2,
        STATE_ATTACK_3,
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

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) state = State.STATE_IDLE;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_1")) state = State.STATE_ATTACK_1;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_2")) state = State.STATE_ATTACK_2;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_3")) state = State.STATE_ATTACK_3;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("RUN")) state = State.STATE_MOVEMENT;
        Debug.Log(InputHandler.InputInfo.LeftJoystick.x);

        switch (state) {
            case State.STATE_IDLE:

            animator.SetBool("STOP_RUN", false);
            if (InputHandler.InputInfo.Button == InputHandler.ButtonType.BUTTON_X) {
                animator.SetBool("ATTACK_1", true);
            }     
            if(InputHandler.InputInfo.LeftJoystick.x != 0|| InputHandler.InputInfo.LeftJoystick.y != 0) {
                animator.SetBool("RUN", true);
            }
            break;
            case State.STATE_ATTACK_1:

            if (InputHandler.InputInfo.Button == InputHandler.ButtonType.BUTTON_X) {
                Debug.Log("click2");
                animator.SetBool("ATTACK_2", true);
                
            }
            if (InputHandler.InputInfo.LeftJoystick.x != 0 || InputHandler.InputInfo.LeftJoystick.y != 0) {
                animator.SetBool("RUN", true);
            }
            if(InputHandler.InputInfo.Button == InputHandler.ButtonType.NONE) {
                animator.SetBool("IDLE", true);
            }
            break;
            case State.STATE_ATTACK_2:

            if (InputHandler.InputInfo.Button == InputHandler.ButtonType.BUTTON_X) {
                Debug.Log("click3");
                animator.SetBool("ATTACK_3", true);
            }
            if (InputHandler.InputInfo.LeftJoystick.x != 0 || InputHandler.InputInfo.LeftJoystick.y != 0) {
                animator.SetBool("RUN", true);
            }
            break;
            case State.STATE_ATTACK_3:
            break;
            case State.STATE_MOVEMENT:

            if (InputHandler.InputInfo.LeftJoystick.x == 0) {
                animator.SetBool("STOP_RUN", true);
            }
            if(InputHandler.InputInfo.Button == InputHandler.ButtonType.BUTTON_X) {
                animator.SetBool("ATTACK_1", true);
            }
            break;
            case State.STATE_ABILITY:
            break;
            case State.STATE_DASH:
            break;
            default:
            break;
        }
        Debug.Log(state);
        //switch (state) {
        //case State.STATE_IDLE:
        //    if (InputHandler.InputInfo.Button == InputHandler.ButtonType.BUTTON_X) {
        //        animator.SetBool("ATTACK_1", true);
        //        state = State.STATE_ATTACK_1;
        //    }
        //break;
        //case State.STATE_ATTACK_1:

        //    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
        //        state = State.STATE_IDLE;
        //    }
        //break;
        //case State.STATE_ATTACK_2:
        //break;
        //case State.STATE_ATTACK_3:
        //break;
        //case State.STATE_MOVEMENT:
        //break;
        //case State.STATE_ABILITY:
        //break;
        //case State.STATE_DASH:
        //break;
        //default:
        //break;
        //}
        //if(state != State.STATE_IDLE)
        //Debug.Log(state);
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
