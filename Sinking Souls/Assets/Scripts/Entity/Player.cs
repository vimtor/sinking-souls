using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : Entity {

    public enum State {
        IDLE,
        ATTACK_1,
        ATTACK_2,
        ATTACK_3,
        MOVEMENT,
        ABILITY,
        DASH
    };

    public float attackOffset;
    public float rotationSpeed;
    public float actionRotationSpeed;
    [HideInInspector] public bool move;
    [HideInInspector] public State state;


    public Dictionary<string, float> clipLength = new Dictionary<string, float>();

    public Ability dash;
    public Ability ability;

    private State lastState;
    private float time;
    private float dashCooldown;
    public float abilityCooldown;
    private Dictionary<Enemy.EnemyType, int> inventory;
    private Vector3 forward, right;



    public void SetupPlayer() {
        OnStart();
        move = true;
        state = State.IDLE;
        dashCooldown = abilityCooldown = 0;
        forward = new Vector3 (Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) ;
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;

        #region Setup Animations
        for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++) {
            var animationClip = animator.runtimeAnimatorController.animationClips[i];
            clipLength.Add(animationClip.name, animationClip.length);
        }

        clipLength["Attack1Anim"] = clipLength["Attack1Anim"] / 3;
        clipLength["Attack2Anim"] = clipLength["Attack2Anim"] / 3;
        clipLength["Attack3Anim"] = clipLength["Attack3Anim"] / 3;
        clipLength["DashAnim"] = clipLength["DashAnim"] / 2;
        #endregion

        EquipWeapon();
    }

    private void Update() {
        CheckDead();
        if (ability.passive) {
            ability.Passive(gameObject);
        }
    }

    private void FixedUpdate() {
        if(move) HandleInput();
    }

    private void SetRotation(float _speed) {
        if (InputHandler.LeftJoystick.x != 0 || InputHandler.LeftJoystick.y != 0) {
            Vector3 horizontal = right * InputHandler.LeftJoystick.x;
            Vector3 vertical = forward * InputHandler.LeftJoystick.y;
            facingDir = horizontal - vertical;

            Vector3 newDir = Vector3.RotateTowards(transform.forward, facingDir, _speed * Time.deltaTime, 0);

            transform.rotation = Quaternion.LookRotation(newDir);
            
        }
    }

    public void HandleInput() {
        // Things that needs to be always on false so they can be changed to true if needed.
        weapon.hitting = false;
        gameObject.layer = LayerMask.NameToLayer("Player");

        StateMachine();


        time += Time.deltaTime;
        if(dashCooldown > 0) dashCooldown -= Time.deltaTime;
        if(abilityCooldown > 0) abilityCooldown -= Time.deltaTime;
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
                    AudioManager.instance.Play("Attack1");
                }   

                if (InputHandler.LeftJoystick.x != 0 || InputHandler.LeftJoystick.y != 0) {
                    state = State.MOVEMENT;
                    lastState = State.IDLE;
                    AudioManager.instance.Play("Walk");
                }

                if (InputHandler.ButtonB()) {
                    if(dashCooldown <= 0) { 
                        state = State.DASH;
                        lastState = State.IDLE;
                        dashCooldown = dash.cooldown;
                        time = 0;

                        AudioManager.instance.Play("Dash");
                    }
                } 

                if (InputHandler.ButtonY()) {
                    if (abilityCooldown <= 0) {
                        state = State.ABILITY;
                        lastState = State.IDLE;
                        abilityCooldown = ability.cooldown;
                        time = 0;
                    }
                } 
            break;
            #endregion

            #region STATE_ATTACK_1
            case State.ATTACK_1:

                SetAnimBool("ATTACK_1");
                if(time > (clipLength["Attack1Anim"] / 3)) weapon.Attack();
                SetRotation(actionRotationSpeed);

                                                                            // you can substraca little offset to makit more fluid
                if (InputHandler.ButtonX() && time > (clipLength["Attack1Anim"]/3)*2 && time < clipLength["Attack1Anim"] + attackOffset) {
                    lastState = State.ATTACK_1;
                    state = State.ATTACK_2;
                    time = 0;
                    AudioManager.instance.Play("Attack2");
                }

                if (InputHandler.ButtonB()) {
                    if (time > clipLength["Attack1Anim"]/2) { //if this seems like a good idea turn rotation on after 2/3 of the attack
                        if (dashCooldown <= 0) {
                            state = State.DASH;
                            lastState = State.ATTACK_1;
                            dashCooldown = dash.cooldown;
                            time = 0;
                            AudioManager.instance.Play("Dash");
                        }
                    }
                }

                if (InputHandler.ButtonY()) {
                    if (time > clipLength["Attack1Anim"] / 2) { //if this seems like a good idea turn rotation on after 2/3 of the attack
                        if (abilityCooldown <= 0) {
                            state = State.ABILITY;
                            lastState = State.ATTACK_1;
                            abilityCooldown = ability.cooldown;
                            time = 0;
                        }
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
                if(time > (clipLength["Attack2Anim"] / 3)) weapon.Attack();
                SetRotation(actionRotationSpeed);

                if (InputHandler.ButtonX() && time > (clipLength["Attack2Anim"] / 3) * 2 && time < clipLength["Attack2Anim"] + attackOffset) {
                    lastState = State.ATTACK_2;
                    state = State.ATTACK_3;
                    time = 0;

                    AudioManager.instance.Play("Attack1");
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
                if (InputHandler.ButtonB()) {
                    if (time > clipLength["Attack2Anim"]/2) { //if this seems like a good idea turn rotation on after 2/3 of the attack
                        if (dashCooldown <= 0) {
                            state = State.DASH;
                            lastState = State.ATTACK_2;
                            dashCooldown = dash.cooldown;
                            time = 0;

                            AudioManager.instance.Play("Dash");
                        }
                    }
                }
                if (InputHandler.ButtonY()) {
                    if (time > clipLength["Attack2Anim"] / 2) { //if this seems like a good idea turn rotation on after 2/3 of the attack
                        if (abilityCooldown <= 0) {
                            state = State.ABILITY;
                            lastState = State.ATTACK_2;
                            abilityCooldown = ability.cooldown;
                            time = 0;
                        }
                    }
                }
            break;
            #endregion

            #region STATE_ATTACK_3
            case State.ATTACK_3:

                SetAnimBool("ATTACK_3");
                if(time > (clipLength["Attack3Anim"] / 3)) weapon.Attack();
                SetRotation(actionRotationSpeed);

                if (InputHandler.LeftJoystick.x != 0 || InputHandler.LeftJoystick.y != 0) {
                    if(time > clipLength["Attack3Anim"]) { 
                        lastState = State.ATTACK_3;
                        state = State.MOVEMENT;
                    }
                }

                if (InputHandler.ButtonX() && time > (clipLength["Attack3Anim"] / 3) * 2 && time < clipLength["Attack3Anim"] + attackOffset) {
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
                if (InputHandler.ButtonB()) {
                    if (time > clipLength["Attack3Anim"]/2) { //if this seems like a good idea turn rotation on after 2/3 of the attack
                        //SetRotation(30);
                        if (dashCooldown <= 0) {
                            state = State.DASH;
                            lastState = State.ATTACK_3;
                            dashCooldown = dash.cooldown;
                            time = 0;

                            AudioManager.instance.Play("Dash");
                        }
                    }
                }
            break;
            #endregion

            #region STATE_MOVEMENT
            case State.MOVEMENT:
                SetAnimBool("RUN");
                SetRotation(rotationSpeed);

                rb.MovePosition(transform.position + (transform.forward * walkSpeed * Time.deltaTime));

                if (InputHandler.LeftJoystick.x == 0 && InputHandler.LeftJoystick.y == 0) {
                    state = State.IDLE;
                    lastState = State.MOVEMENT;
                    AudioManager.instance.Pause("Walk");
                }

                if (InputHandler.ButtonX()) {
                    if (lastState == State.ATTACK_1 && time < clipLength["Attack1Anim"] + attackOffset) {
                            lastState = State.MOVEMENT;
                            state = State.ATTACK_2;
                            time = 0;
                            AudioManager.instance.Play("Attack2");
                    }
                    else if (lastState == State.ATTACK_2 && time < clipLength["Attack2Anim"] + attackOffset) {
                            lastState = State.MOVEMENT;
                            state = State.ATTACK_3;
                            time = 0;
                            AudioManager.instance.Play("Attack1");
                    }
                    else {
                        state = State.ATTACK_1;
                        lastState = State.MOVEMENT;
                        time = 0;
                        AudioManager.instance.Play("Attack1");
                    }
                }
                if (InputHandler.ButtonB()) {
                    if (dashCooldown <= 0) {
                        state = State.DASH;
                        lastState = State.IDLE;
                        dashCooldown = dash.cooldown;
                        time = 0;

                        AudioManager.instance.Pause("Walk");
                        AudioManager.instance.Play("Dash");
                    }
                } 
                if (InputHandler.ButtonY()) {
                    if (abilityCooldown <= 0) {
                        state = State.ABILITY;
                        lastState = State.IDLE;
                        abilityCooldown = ability.cooldown;
                        time = 0;
                        AudioManager.instance.Pause("Walk");
                }else{
                    Debug.Log("wait for cooldown");
                }
                }
                break;
            #endregion

            #region STATE_ABILITY
            case State.ABILITY:
                SetAnimBool("THROW");
                if (time > clipLength["ThrowAnim"]/2) {
                    Ability();
                }   

                if (InputHandler.LeftJoystick.x != 0 || InputHandler.LeftJoystick.y != 0) {
                    if (time > clipLength["ThrowAnim"]) {
                        lastState = State.ABILITY;
                        state = State.MOVEMENT;
                        time = 0;
                        thrown = false;
                        AudioManager.instance.Play("Walk");
                    }
                }
                else {
                    if (time > clipLength["ThrowAnim"]) {
                        lastState = State.ABILITY;
                        state = State.IDLE;
                        time = 0;
                        thrown = false;
                    }
                }
                if (InputHandler.ButtonX()) { 
                    if (time > (clipLength["ThrowAnim"] / 3) * 2) { //if 2/3 of the animation has passed
                        lastState = State.ABILITY;
                        state = State.ATTACK_1;
                        time = 0;
                        thrown = false;
                    }
                }   
            break;
            #endregion

            #region STATE_DASH
            case State.DASH:
                SetAnimBool("DASH");
                gameObject.layer = LayerMask.NameToLayer("PlayerDash");

                Dash();
                if (InputHandler.LeftJoystick.x != 0 || InputHandler.LeftJoystick.y != 0) {
                    if (time > clipLength["DashAnim"]/2) {
                        rb.velocity = GetComponent<Transform>().forward * 0;
                        lastState = State.DASH;
                        state = State.MOVEMENT;
                        time = 0;
                        AudioManager.instance.Play("Walk");
                    }
                }
                else {
                    if (time > clipLength["DashAnim"] / 2) {
                        rb.velocity = GetComponent<Transform>().forward * 0;
                        lastState = State.DASH;
                        state = State.IDLE;
                        time = 0;
                    }
                }
                break;
            #endregion

            default:
                break;
        }
    }


    public void Ability() {
        if (ability.passive) {
            ability.Activate();
        }else ability.Use(gameObject);
    }

    public void Dash() {
        // Dash C# should be implemented in the future when the dash possible behaviours are defined. 
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

    public void CheckDead() {
        if (health <= 0 && !GameController.instance.godMode) {
            GameController.instance.died = true;
            GameController.instance.scene = GameController.GameState.LOBBY;
            SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
        }
    }

}
