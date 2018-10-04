using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity{

    enum State {
        IDLE,
        ATTACK_1,
        ATTACK_2,
        ATTACK_3,
        MOVEMENT,
        ABILITY,
        DASH
    };

    private State state;
    private float weaponUseDelay;

    private Dictionary<Enemy.EnemyType, int> inventory;
    public AbilitySO dash;
    public AbilitySO ability;

    private void Start() {
        OnStart();
        state = State.IDLE;
        
        EquipWeapon();
    }

    private void FixedUpdate() {
        UpdateState();
        HandleInput();
    }

    private void UpdateState() {
        // Set the state depending on the animation state we are in.
        if (CurrentState("IDLE"))     state = State.IDLE;
        if (CurrentState("ATTACK_1")) state = State.ATTACK_1;
        if (CurrentState("ATTACK_2")) state = State.ATTACK_2;
        if (CurrentState("ATTACK_3")) state = State.ATTACK_3;
        if (CurrentState("RUN"))      state = State.MOVEMENT;
        if (CurrentState("THROW"))    state = State.ABILITY;
        if (CurrentState("DASH"))     state = State.DASH;
    }

    private bool CurrentState(string stateName) {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }
    private bool CurrentTransition(string stateName) {
        return animator.GetAnimatorTransitionInfo(0).IsUserName(stateName);
    }

    public void HandleInput() {

        // Set rotation of the player to the one on the left joystick.
        if (InputHandler.LeftJoystick.x != 0 || InputHandler.LeftJoystick.y != 0) {
            facingDir = new Vector3(InputHandler.LeftJoystick.y, 0, InputHandler.LeftJoystick.x);
            transform.rotation = Quaternion.LookRotation(-facingDir);
        }

        // Things that needs to be always on false so they can be changed to true if needed.
        weapon.model.GetComponent<BoxCollider>().enabled = false;

        StateMachine();

        weaponUseDelay += Time.deltaTime;
    }

    public void StateMachine() {
        switch (state) {
            #region STATE_IDLE
            case State.IDLE:
                
                if (CurrentTransition("IDLE-RUN")) // Check if i'm transitioning to walk
                    rb.MovePosition(transform.position + (transform.forward * walkSpeed * Time.deltaTime));

                animator.SetBool("STOP_RUN", false);
                if (InputHandler.ButtonX()) {
                    animator.SetBool("ATTACK_1", true);
                    weaponUseDelay = 0;
                }

                if (InputHandler.ButtonRT()) {
                    animator.SetBool("THROW", true);
                }

                if (InputHandler.ButtonB()) {
                    animator.SetBool("DASH", true);
                }

                if (InputHandler.LeftJoystick.x != 0 || InputHandler.LeftJoystick.y != 0) {
                    animator.SetBool("RUN", true);
                }
                break;
            #endregion

            #region STATE_ATTACK_1
            case State.ATTACK_1:
                weapon.Attack();

                // Check if i'm transitioning to walk
                if (CurrentTransition("IDLE-RUN")) rb.MovePosition(transform.position + (transform.forward * walkSpeed * Time.deltaTime));

                animator.SetBool("IDLE", true);
                if (InputHandler.ButtonX() && weaponUseDelay > weapon.useDelay) {
                    animator.SetBool("ATTACK_2", true);
                    weaponUseDelay = 0;
                }
                if (InputHandler.ButtonY()) {
                    animator.SetBool("THROW", true);
                }
                if (InputHandler.LeftJoystick.x != 0 || InputHandler.LeftJoystick.y != 0) {
                    animator.SetBool("RUN", true);
                }
                break;
            #endregion

            #region STATE_ATTACK_2
            case State.ATTACK_2:
                weapon.Attack();

                if (InputHandler.ButtonX() && weaponUseDelay > weapon.useDelay) {
                    animator.SetBool("ATTACK_3", true);
                    weaponUseDelay = 0;
                }
                if (InputHandler.ButtonY()) {
                    animator.SetBool("THROW", true);
                }
                if (InputHandler.LeftJoystick.x != 0 || InputHandler.LeftJoystick.y != 0) {
                    animator.SetBool("RUN", true);
                }
                break;
            #endregion

            #region STATE_ATTACK_3
            case State.ATTACK_3:
                weapon.CriticAttack();

                if (InputHandler.ButtonY()) {
                    animator.SetBool("THROW", true);
                }
                if (InputHandler.LeftJoystick.x != 0 || InputHandler.LeftJoystick.y != 0) {
                    animator.SetBool("RUN", true);
                }
                break;
            #endregion

            #region STATE_MOVEMENT
            case State.MOVEMENT:
                // Check if i'm transitioning from one animation to the other.
                if (!CurrentTransition("RUN-IDLE") && !CurrentTransition("RUN-THROW")) 
                    rb.MovePosition(transform.position + (transform.forward * walkSpeed * Time.deltaTime));

                if (InputHandler.ButtonB()) {
                    animator.SetBool("DASH", true);
                }
                if (InputHandler.LeftJoystick.x == 0 && InputHandler.LeftJoystick.y == 0) {
                    animator.SetBool("STOP_RUN", true);
                }
                if (InputHandler.ButtonY()) {
                    animator.SetBool("THROW", true);
                }
                if (InputHandler.ButtonX()) {
                    animator.SetBool("ATTACK_1", true);
                }
                break;
            #endregion

            case State.ABILITY:
                Ability();
                break;

            case State.DASH:
                Dash();
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

    public void Attack() { }

}
