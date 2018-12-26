using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class Player : Entity {

    delegate void StateAction();

    private enum PlayerState { DASHING, MOVING, ATTACKING, HITTED, NONE };
    private PlayerState m_PlayerState;

    [Header("Movement parameters")]
    public float m_MovementDamping;
    public float m_RotationDamping;

    [Space(5)]
    public float m_MovementSpeed;
    public float MovementSpeed
    {
        get { return m_MovementSpeed; }
        set { if (value > 0) m_MovementSpeed = value; }
    }
    public float m_DashSpeed;

    private readonly int m_SpeedParam = Animator.StringToHash("Speed");
    private readonly int m_AttackParam = Animator.StringToHash("Attack");
    private readonly int m_DashParam = Animator.StringToHash("Dash");
    private readonly int m_SpellParam = Animator.StringToHash("Spell");
    private readonly int m_WeaponTypeParam = Animator.StringToHash("WeaponType");
    private readonly int m_SpellTypeParam = Animator.StringToHash("SpellType");

    private float m_AttackLength;
    private float m_DashLength;

    private Vector3 m_Forward;
    private Vector3 m_Side;
    private Vector3 m_HorizontalMovement;
    private Vector3 m_VerticalMovement;
    private Vector3 m_Direction;

    private bool m_CanMove;
    public bool CanMove
    {
        get { return m_CanMove;  }
        set { m_CanMove = value; }
    }

    private float m_AbilityCooldown;
    public float AbilityCooldown
    {
        get { return m_AbilityCooldown; }
    }

    // To avoid transition overlaping.
    private byte m_TransitionCount;

    public void SetupPlayer() {
        OnStart();

        // Calculate the forward and side vectors relative to the camera.
        m_Forward = Camera.main.transform.forward;
        m_Forward.y = 0;
        m_Forward = Vector3.Normalize(m_Forward);
        m_Side = Quaternion.Euler(new Vector3(0, 90, 0)) * m_Forward;

        // Get animation lengths.
        var animationClips = m_Animator.runtimeAnimatorController.animationClips;
        m_AttackLength = Array.Find(animationClips, clip => clip.name == "Klaus_1").length;
        m_DashLength = Array.Find(animationClips, clip => clip.name == "Forward Roll").length * 0.9f;

        // Initialize private members.
        m_PlayerState = PlayerState.MOVING;
        m_AbilityCooldown = 0.0f;
        m_CanMove = true;
    }

    private void FixedUpdate()
    {
        if (!m_CanMove) return;

        CheckDead();
        if (m_Ability.passive) m_Ability.Passive(gameObject);

        // Things that needs to be always on false so they can be changed to true if needed.
        m_Weapon.hitting = false;
        gameObject.layer = LayerMask.NameToLayer("Player");


        m_HorizontalMovement = m_Side * InputHandler.LeftJoystick.x;
        m_VerticalMovement = m_Forward * InputHandler.LeftJoystick.y;
        m_Direction = m_HorizontalMovement - m_VerticalMovement;

        switch (m_PlayerState)
        {
            case PlayerState.DASHING:
                break;

            case PlayerState.MOVING:
                m_Animator.SetFloat(m_SpeedParam, m_Direction.magnitude, m_MovementDamping, Time.deltaTime);

                Rotate();
                Move();

                if (InputHandler.ButtonB()) ChangeState(Dash, m_DashLength, PlayerState.DASHING, PlayerState.MOVING);

                if (InputHandler.ButtonX()) ChangeState(Attack, m_AttackLength, PlayerState.ATTACKING, PlayerState.MOVING);

                break;

            case PlayerState.ATTACKING:
                // For combos purposes.
                if (InputHandler.ButtonX()) ChangeState(Attack, m_AttackLength, PlayerState.ATTACKING, PlayerState.MOVING);
                break;

            default:
                break;
        }
    }

    #region ChangeState Functions

    private void ChangeState(StateAction action, float delay, PlayerState newState, PlayerState returnState)
    {
        StartCoroutine(ChangeStateCoroutine(action, delay, newState, returnState));
    }

    private IEnumerator ChangeStateCoroutine(StateAction action, float delay, PlayerState newState, PlayerState returnState)
    {
        m_TransitionCount++;

        m_PlayerState = newState;
        action();

        yield return new WaitForSecondsRealtime(delay);
        m_Rigidbody.velocity = Vector3.zero;
        if (m_TransitionCount <= 1) m_PlayerState = returnState;

        m_TransitionCount--;
    }

    #endregion

    #region Movement Functions

    private void Move()
    {
        m_Rigidbody.MovePosition(transform.position + transform.forward * m_MovementSpeed * m_Direction.magnitude * Time.deltaTime);
    }

    private void Rotate()
    {
        if (InputHandler.LeftJoystickZero()) return;

        Quaternion rotation = Quaternion.LookRotation(m_Direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * m_RotationDamping);
    }

    #endregion

    #region Action Functions

    private void Attack()
    {
        m_Animator.SetFloat(m_SpeedParam, 0);

        // Set weapon type and attack type.
        m_Animator.SetInteger(m_WeaponTypeParam, 1);
        m_Animator.SetTrigger(m_AttackParam);

        // Activate weapon.

    }

    private void Dash()
    {
        m_Animator.SetTrigger(m_DashParam);
        m_Rigidbody.AddForce(transform.forward * m_DashSpeed * Time.fixedDeltaTime, ForceMode.Impulse);
    }

    public void Spell() {
        m_Animator.SetTrigger(m_SpellParam);
        if (m_Ability.passive) m_Ability.Activate();
        else m_Ability.Use(gameObject);
    }

    #endregion


    public void CheckDead() {
        if (Health <= 0 && !GameController.instance.godMode) {
            GameController.instance.died = true;
            GameController.instance.scene = GameController.GameState.LOBBY;
            SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
        }
    }

}
