using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity{

    public enum State {
        IDLE,
        ATTACK_1,
        ATTACK_2,
        ATTACK_3,
        MOVEMENT,
        ABILITY,
        DASH
    };

    private State state;
    private State lastState;
    private float time;

    public float attackOffset;

    Dictionary<string, float> clipLength = new Dictionary<string, float>();

    private Dictionary<Enemy.EnemyType, int> inventory;
    public AbilitySO dash;
    public AbilitySO ability;

    private void Start() {
        OnStart();
        state = State.IDLE;

        for(int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++) {
            var animationClip = animator.runtimeAnimatorController.animationClips[i];
            clipLength.Add(animationClip.name, animationClip.length);
        }
        clipLength["Attack1Anim"] = clipLength["Attack1Anim"] / 2;
        clipLength["Attack2Anim"] = clipLength["Attack2Anim"] / 2;
        clipLength["Attack3Anim"] = clipLength["Attack3Anim"] / 2;
        clipLength["DashAnim"] = clipLength["DashAnim"] / 2;

        EquipWeapon();
    }

    private void FixedUpdate() {

        HandleInput();
    }

    private bool CurrentState(string stateName) {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }
    private bool CurrentTransition(string stateName) {
        return animator.GetAnimatorTransitionInfo(0).IsUserName(stateName);
    }

    public void HandleInput() {

        // Things that needs to be always on false so they can be changed to true if needed.
        weapon.hitting = false;
        gameObject.layer = LayerMask.NameToLayer("Player");

        StateMachine();


        time += Time.deltaTime;
    }

    public void StateMachine() {
        switch (state) {
            #region STATE_IDLE
            case State.IDLE:
                SetAnimBool("IDLE");

                if (InputHandler.ButtonX()) {
                    lastState = State.IDLE;
                    state = State.ATTACK_1;
                    time = 0;
                }   

                if (InputHandler.LeftJoystick.x != 0 || InputHandler.LeftJoystick.y != 0) {
                    state = State.MOVEMENT;
                    lastState = State.IDLE;
                }

                if (InputHandler.ButtonB()) {
                    state = State.DASH;
                    lastState = State.IDLE;
                    time = 0;
                } 
            break;
            #endregion

            #region STATE_ATTACK_1
            case State.ATTACK_1:

                SetAnimBool("ATTACK_1");
                weapon.Attack();
                                                                            // you can substraca little offset to makit more fluid
                if (InputHandler.ButtonX() && time > clipLength["Attack1Anim"] - 0.2 && time < clipLength["Attack1Anim"] + attackOffset) {
                    lastState = State.ATTACK_1;
                    state = State.ATTACK_2;
                    time = 0;
                }

                if (InputHandler.ButtonB()) {
                    if (time > clipLength["Attack1Anim"]/2) { //if this seems like a good idea turn rotation on after 2/3 of the attack
                        state = State.DASH;
                        lastState = State.IDLE;
                        time = 0;
                    }
                } 

                else if(time > clipLength["Attack1Anim"]) {//change frome attack1time to exact length of the animation
                    lastState = State.ATTACK_1;
                    state = State.IDLE;
                }
                if(InputHandler.LeftJoystick.x != 0 || InputHandler.LeftJoystick.y != 0) {
                    if(time > clipLength["Attack1Anim"]) { 
                        lastState = State.ATTACK_1;
                        state = State.MOVEMENT;
                    }
                }

                break;
            #endregion

            #region STATE_ATTACK_2
            case State.ATTACK_2:
                SetAnimBool("ATTACK_2");
                weapon.Attack();

                if (InputHandler.ButtonX() && time > clipLength["Attack2Anim"] - 0.2 && time < clipLength["Attack2Anim"] + attackOffset) {
                    lastState = State.ATTACK_2;
                    state = State.ATTACK_3;
                    time = 0;
                }
                else if (time > clipLength["Attack2Anim"]) {
                    lastState = State.ATTACK_2;
                    state = State.IDLE;
                }
                if (InputHandler.LeftJoystick.x != 0 || InputHandler.LeftJoystick.y != 0) {
                    if (time > clipLength["Attack2Anim"]) {
                        lastState = State.ATTACK_2;
                        state = State.MOVEMENT;
                    }
                }
            break;
            #endregion

            #region STATE_ATTACK_3
            case State.ATTACK_3:
                SetAnimBool("ATTACK_3");
                weapon.CriticAttack();

                if (InputHandler.LeftJoystick.x != 0 || InputHandler.LeftJoystick.y != 0) {
                    if(time > clipLength["Attack3Anim"]) { 
                        lastState = State.ATTACK_3;
                        state = State.MOVEMENT;
                    }
                }

                if (InputHandler.ButtonX() && time > clipLength["Attack3Anim"] - 0.2 && time < clipLength["Attack3Anim"] + attackOffset) {
                    lastState = State.ATTACK_3;
                    state = State.ATTACK_1;
                    time = 0;
                }
                else if (time > clipLength["Attack3Anim"]) {
                    lastState = State.ATTACK_3;
                    state = State.IDLE;
                }
                if (InputHandler.LeftJoystick.x != 0 || InputHandler.LeftJoystick.y != 0) {
                    if (time > clipLength["Attack3Anim"]) {
                        lastState = State.ATTACK_3;
                        state = State.MOVEMENT;
                    }
                }
            break;
            #endregion

            #region STATE_MOVEMENT
            case State.MOVEMENT:
                SetAnimBool("RUN");
                if (InputHandler.LeftJoystick.x != 0 || InputHandler.LeftJoystick.y != 0) {
                    facingDir = new Vector3(InputHandler.LeftJoystick.y, 0, InputHandler.LeftJoystick.x);
                    transform.rotation = Quaternion.LookRotation(-facingDir);
                }
                rb.MovePosition(transform.position + (transform.forward * walkSpeed * Time.deltaTime));

                if (InputHandler.LeftJoystick.x == 0 && InputHandler.LeftJoystick.y == 0) {
                    state = State.IDLE;
                    lastState = State.MOVEMENT;
                }

                if (InputHandler.ButtonX()) {
                    if (lastState == State.ATTACK_1 && time < clipLength["Attack1Anim"] + attackOffset) {
                            lastState = State.MOVEMENT;
                            state = State.ATTACK_2;
                            time = 0;
                    }
                    else if (lastState == State.ATTACK_2 && time < clipLength["Attack2Anim"] + attackOffset) {
                            lastState = State.MOVEMENT;
                            state = State.ATTACK_3;
                            time = 0;
                    }
                    else {
                        state = State.ATTACK_1;
                        lastState = State.MOVEMENT;
                        time = 0;
                    }
                }
                if (InputHandler.ButtonB()) {
                    state = State.DASH;
                    lastState = State.IDLE;
                    time = 0;
                } 
                break;
            #endregion

            case State.ABILITY:
                Ability();
                break;

            case State.DASH:
                SetAnimBool("DASH");
                gameObject.layer = LayerMask.NameToLayer("PlayerDash");

                Dash();
                if (InputHandler.LeftJoystick.x != 0 || InputHandler.LeftJoystick.y != 0) {
                    if (time > clipLength["DashAnim"]) {
                        rb.velocity = GetComponent<Transform>().forward * 0;
                        lastState = State.DASH;
                        state = State.MOVEMENT;
                        time = 0;
                    }
                }
                else {
                    if (time > clipLength["DashAnim"]) {
                        rb.velocity = GetComponent<Transform>().forward * 0;
                        lastState = State.DASH;
                        state = State.IDLE;
                        time = 0;
                    }
                }
                break;

            default:
                break;
        }
    }


    public void Ability() {
        if (CurrentTransition("THROW-RUN")) // Check if i'm transitioning to walk
            rb.MovePosition(transform.position + (transform.forward * walkSpeed * Time.deltaTime));
    }

    public void Dash() {
        dash.Use(gameObject);
    }

    public void SetAnimBool(string str) {
        animator.SetBool("ATTACK_1", false);
        animator.SetBool("ATTACK_2", false);
        animator.SetBool("ATTACK_3", false);
        animator.SetBool("RUN", false);
        animator.SetBool("IDLE", false);
        animator.SetBool("THROW", false);
        animator.SetBool("DASH", false);

        animator.SetBool(str, true);
    }

    public void Attack() {
    }

}
