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
    private Animator animator;
    private Vector3 facingDir;
    private Rigidbody rb;
    private float weaponUseDelay;

    private Dictionary<Enemy.EnemyType, int> inventory;
    public AbilitySO dash;
    public AbilitySO ability;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        state = State.IDLE;
        facingDir = new Vector3(InputHandler.LeftJoystick.y, 0, InputHandler.LeftJoystick.x); ;
        animator = GetComponent<Animator>();
        weapon.Instantiate(hand);
    }

    private void Update() {
        HandleInput();
    }

    public void HandleInput() {
        // Set rotation of the player to the one on the left joy

        if (InputHandler.LeftJoystick.x != 0 || InputHandler.LeftJoystick.y != 0)
            facingDir = new Vector3(InputHandler.LeftJoystick.y, 0, InputHandler.LeftJoystick.x);
        transform.rotation = Quaternion.LookRotation(-facingDir);
        
        // Set the state depending on the animation state we are in
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("IDLE")) state = State.IDLE;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK_1")) state = State.ATTACK_1;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK_2")) state = State.ATTACK_2;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK_3")) state = State.ATTACK_3;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("RUN")) state = State.MOVEMENT;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("THROW")) state = State.ABILITY;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("DASH")) state = State.DASH;

        // Things that needs to be alwais on fals so they can be changed to true if needed
        weapon.model.GetComponent<BoxCollider>().enabled = false;

        // State machine
        switch (state) {
            case State.IDLE:
                if (animator.GetAnimatorTransitionInfo(0).IsUserName("IDLE-RUN")) // Check if i'm transitioning to walk
                    rb.MovePosition(transform.position + (transform.forward * walkSpeed * Time.deltaTime));

                animator.SetBool("STOP_RUN", false);
                if (InputHandler.Button == InputHandler.ButtonType.BUTTON_X) {
                    animator.SetBool("ATTACK_1", true);
                    weaponUseDelay = 0;
                }

                if (InputHandler.Button == InputHandler.ButtonType.BUTTON_Y) {
                    animator.SetBool("THROW", true);
                }

                if (InputHandler.Button == InputHandler.ButtonType.BUTTON_B) {
                    animator.SetBool("DASH", true);
                }

                if (InputHandler.LeftJoystick.x != 0|| InputHandler.LeftJoystick.y != 0) {
                    animator.SetBool("RUN", true);
                }
                break;

            case State.ATTACK_1:
                weapon.Attack();

                if (InputHandler.Button == InputHandler.ButtonType.BUTTON_X && weaponUseDelay > weapon.useDelay) {
                    animator.SetBool("ATTACK_2", true);
                    weaponUseDelay = 0;
                }
                if (InputHandler.Button == InputHandler.ButtonType.BUTTON_Y) {
                    animator.SetBool("THROW", true);
                }
                if (InputHandler.LeftJoystick.x != 0 || InputHandler.LeftJoystick.y != 0) {
                    animator.SetBool("RUN", true);
                }
                if(InputHandler.Button == InputHandler.ButtonType.NONE) {
                    animator.SetBool("IDLE", true);
                }
                break;

            case State.ATTACK_2:
                weapon.Attack();

                if (InputHandler.Button == InputHandler.ButtonType.BUTTON_X && weaponUseDelay > weapon.useDelay) {
                    animator.SetBool("ATTACK_3", true);
                    weaponUseDelay = 0;
                }
                if (InputHandler.Button == InputHandler.ButtonType.BUTTON_Y) {
                    animator.SetBool("THROW", true);
                }
                if (InputHandler.LeftJoystick.x != 0 || InputHandler.LeftJoystick.y != 0) {
                    animator.SetBool("RUN", true);
                }
                break;

            case State.ATTACK_3:
                weapon.CriticAttack();
                
                if (InputHandler.Button == InputHandler.ButtonType.BUTTON_Y) {
                    animator.SetBool("THROW", true);
                }
                break;

            case State.MOVEMENT:

                if(!animator.GetAnimatorTransitionInfo(0).IsUserName("RUN-IDLE") && !animator.GetAnimatorTransitionInfo(0).IsUserName("RUN-THROW"))  // Check if i'm transitioning from one animation to the other
                    rb.MovePosition(transform.position + (transform.forward * walkSpeed * Time.deltaTime));

                if (InputHandler.Button == InputHandler.ButtonType.BUTTON_B) {
                    animator.SetBool("DASH", true);
                }
                if (InputHandler.LeftJoystick.x == 0 && InputHandler.LeftJoystick.y == 0) {
                    animator.SetBool("STOP_RUN", true);
                }
                if (InputHandler.Button == InputHandler.ButtonType.BUTTON_Y) {
                    animator.SetBool("THROW", true);
                }
                if (InputHandler.Button == InputHandler.ButtonType.BUTTON_X) {
                    animator.SetBool("ATTACK_1", true);
                }
                break;

            case State.ABILITY:
                if (animator.GetAnimatorTransitionInfo(0).IsUserName("THROW-RUN")) // Check if i'm transitioning to walk
                    rb.MovePosition(transform.position + (transform.forward * walkSpeed * Time.deltaTime));
            break;

            case State.DASH:
            
                dash.Use(this.gameObject);
            break;

            default:
                break;
        }
        weaponUseDelay += Time.deltaTime;
        //Debug.Log(state);
        Debug.Log(weapon.model.GetComponent<BoxCollider>().enabled);
    }

    public void Ability() { }
    public void Dash() { }
    public void Attack() { }

}
