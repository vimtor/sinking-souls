using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class Player : Entity
{
    

    #region State Logic
    delegate void StateAction();

    private enum PlayerState { DASHING, MOVING, ATTACKING, REACTING, SPELLING, NONE };
    private PlayerState m_PlayerState;

    // To avoid transition overlaping.
    private byte m_TransitionCount;
    #endregion

    #region Game-feel Variables
    [Header("Movement parameters")]
    public float m_MovementDamping;
    public float m_MovementSpeed;
    public float lockDistance;
    private float minDist = 1.7f;
    private float maxDist = 3f;
    public float MovementSpeed
    {
        get { return m_MovementSpeed; }
        set { if (value > 0) m_MovementSpeed = value; }
    }
    public float m_MaxMovementSpeed;
    public float m_MovementRotationDamping;

    [Space(10)]

    public float m_DashMovementSpeed;
    public float m_DashSpeed;
    public float m_DashRotationDamping;

    [Space(10)]

    [Tooltip("How much the player will advance which each step of the attack.")]
    [Range(0.0f, 1.0f)] public float m_AttackStepForce;
    public float m_AttackRotationDamping;

    private float m_RotationDamping;
    private float m_OriginalMovementSpeed;
    #endregion

    #region Animator Variables
    private readonly int m_SpeedParam = Animator.StringToHash("Speed");
    private readonly int m_AttackParam = Animator.StringToHash("Attack");
    private readonly int m_DashParam = Animator.StringToHash("Dash");
    private readonly int m_SpellParam = Animator.StringToHash("Spell");
    private readonly int m_WeaponTypeParam = Animator.StringToHash("WeaponType");
    private readonly int m_SpellTypeParam = Animator.StringToHash("SpellType");

    private float m_AttackLength;
    private float m_DashLength;
    private float m_SpellLength;
    #endregion

    #region Movement Variables
    private Vector3 m_Forward;
    private Vector3 m_Side;
    private Vector3 m_HorizontalMovement;
    private Vector3 m_VerticalMovement;
    private Vector3 m_Direction;
    private Quaternion fake_Forward;
    

    private bool m_CanMove;
    public bool CanMove
    {
        get { return m_CanMove;  }
        set { m_CanMove = value; }
    }
    #endregion


    public void SetupPlayer()
    {
        OnStart();

        // Calculate the forward and side vectors relative to the camera.
        m_Forward = Camera.main.transform.forward;
        m_Forward.y = 0;
        m_Forward = Vector3.Normalize(m_Forward);
        m_Side = Quaternion.Euler(new Vector3(0, 90, 0)) * m_Forward;

        // Get animation lengths.
        var animationClips = m_Animator.runtimeAnimatorController.animationClips;
        m_AttackLength = Array.Find(animationClips, clip => clip.name == "Klaus_1").length;
        m_DashLength = Array.Find(animationClips, clip => clip.name == "Dash").length * 0.85f;
        m_SpellLength = Array.Find(animationClips, clip => clip.name == "Throw").length;

        // Initialize private members.
        m_OriginalMovementSpeed = m_MovementSpeed;
        m_PlayerState = PlayerState.MOVING;
        m_AbilityCooldown = 0.0f;
        m_CanMove = true;
        m_WeaponCollider.enabled = false;
        m_RotationDamping = m_MovementRotationDamping;
    }
    //value, start1, end1, new sratr, new end
    float map(float s, float a1, float a2, float b1, float b2) {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    private void FixedUpdate()
    {
        if (!m_CanMove) return;
        m_Animator.SetBool("LoockedEnemy", (lockedEnemy != null));
        CheckDead();
        if (m_Abilities[0].IsPassive) m_Abilities[0].Passive(gameObject);

        m_HorizontalMovement = m_Side * InputManager.LeftJoystick.x;
        m_VerticalMovement = m_Forward * InputManager.LeftJoystick.y;
        m_Direction = m_HorizontalMovement - m_VerticalMovement;

        if(lockedEnemy != null && (lockedEnemy.transform.position - transform.position).magnitude > lockDistance) lockedEnemy = null;

        switch (m_PlayerState)
        {
            case PlayerState.DASHING:
                Move();
                Rotate();

                if (InputManager.ButtonX) ChangeState(Attack, m_AttackLength, PlayerState.ATTACKING, PlayerState.MOVING);
                break;

            case PlayerState.REACTING:
                if (!m_Hitted) m_PlayerState = PlayerState.MOVING;
                break;

            case PlayerState.MOVING:
                if (lockedEnemy != null)
                {
                    Vector3 LocalSpeed = transform.InverseTransformDirection(m_Rigidbody.velocity);
                    
                    Debug.Log(LocalSpeed);
                    m_Animator.SetFloat("JoystickX", map(LocalSpeed.x, -MovementSpeed, MovementSpeed, -1, 1));
                    m_Animator.SetFloat("JoystickY", map(LocalSpeed.z, -MovementSpeed, MovementSpeed, -1, 1));
                    CombatRotation();
                    CombatMove();
                }
                else
                {
                    m_Animator.SetFloat(m_SpeedParam, m_Direction.magnitude, m_MovementDamping, Time.deltaTime);
                    Rotate();
                    Move();
                }

                if (InputManager.ButtonB) ChangeState(Dash, m_DashLength, PlayerState.DASHING, PlayerState.MOVING, false);
                if (InputManager.ButtonX) ChangeState(Attack, m_AttackLength, PlayerState.ATTACKING, PlayerState.MOVING);
                if (InputManager.ButtonY)
                {
                    if (m_AbilityCooldown <= 0.0f)
                    {
                        // Ability cooldwon is reset in the UseAbility method.
                        ChangeState(Spell, m_SpellLength, PlayerState.SPELLING, PlayerState.MOVING);
                    }
                }

                if (m_Hitted) m_PlayerState = PlayerState.REACTING;
                break;

            case PlayerState.ATTACKING:
                if (m_Hitted) m_PlayerState = PlayerState.REACTING;

                // Behaviour while attacking.
                if (InputManager.ButtonX) ChangeState(Attack, m_AttackLength, PlayerState.ATTACKING, PlayerState.MOVING); // Enable combo strings.
                
                Rotate(); // Rotate with attacking rotation damping.

                if (InputManager.ButtonB) ChangeState(Dash, m_DashLength, PlayerState.DASHING, PlayerState.MOVING, false);
                if (InputManager.ButtonY)
                {
                    if (m_AbilityCooldown <= 0.0f)
                    {
                        ChangeState(Spell, m_SpellLength, PlayerState.SPELLING, PlayerState.MOVING);
                    }
                }
                break;

            case PlayerState.SPELLING:
                if (InputManager.ButtonB) ChangeState(Dash, m_DashLength, PlayerState.DASHING, PlayerState.MOVING, false);

                break;

            default:
                break;
        }

        if (m_AbilityCooldown > 0) m_AbilityCooldown -= Time.deltaTime;
    }

    #region ChangeState Functions

    private void ChangeState(StateAction action, float delay, PlayerState newState, PlayerState returnState, bool resetVelocity = true)
    {
        StartCoroutine(ChangeStateCoroutine(action, delay, newState, returnState, resetVelocity));
    }

    private IEnumerator ChangeStateCoroutine(StateAction action, float delay, PlayerState newState, PlayerState returnState, bool resetVelocity)
    {
        m_TransitionCount++;

        m_PlayerState = newState;
        action();

        yield return new WaitForSecondsRealtime(delay);
        
        if (m_TransitionCount <= 1)
        {
            if (resetVelocity) m_Rigidbody.velocity = Vector3.zero;
            m_PlayerState = returnState;
            if (m_PlayerState == PlayerState.MOVING) ResetMovement();
        } 

        m_TransitionCount--;
    }

    #endregion

    #region Movement Functions

    private void Move()
    {
        // In the future change this to add force and put materials with friction in them.
        // m_Rigidbody.MovePosition(transform.position + transform.forward * m_MovementSpeed * m_Direction.magnitude * Time.fixedDeltaTime);
        Vector3 velocity = m_Rigidbody.velocity;
        velocity.y = 0;

        m_Rigidbody.velocity = transform.forward.normalized * InputManager.LeftJoystick.magnitude * m_MovementSpeed;

        //if (velocity.magnitude < m_MaxMovementSpeed)
        //{
        //    m_Rigidbody.AddForce(m_Direction * m_MovementSpeed * Time.fixedDeltaTime);
        //}
    }

    private void CombatMove()
    {

        // In the future change this to add force and put materials with friction in them.
        // m_Rigidbody.MovePosition(transform.position + transform.forward * m_MovementSpeed * m_Direction.magnitude * Time.fixedDeltaTime);
        Vector3 velocity = m_Rigidbody.velocity;
        velocity.y = 0;
        
       Vector3 direction = Camera.main.transform.forward.normalized * InputManager.LeftJoystick.y*-1 + (Quaternion.Euler(new Vector3(0, 90, 0)) * Camera.main.transform.forward.normalized) * InputManager.LeftJoystick.x;
        

        m_Rigidbody.velocity = new Vector3(direction.x, 0, direction.z)*m_MovementSpeed;

        //if (velocity.magnitude < m_MaxMovementSpeed)
        //{
        //    m_Rigidbody.AddForce(m_Direction * m_MovementSpeed * Time.fixedDeltaTime);
        //}
    }

    private void Rotate()
    {
        if (InputManager.LeftJoystickZero()) return;

        Quaternion rotation = Quaternion.LookRotation(m_Direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * m_RotationDamping);
    }

    private void CombatRotation()
    {
        Debug.Log("Entra");

        //Movement
        Quaternion fakeRotation = Quaternion.LookRotation(m_Direction);
        fake_Forward = Quaternion.Lerp(fake_Forward, fakeRotation, Time.deltaTime * m_RotationDamping);

        //Rotation round the enemy
        Quaternion rotation = Quaternion.LookRotation(lockedEnemy.transform.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * m_RotationDamping);
    }

    private void ResetMovement()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
        m_RotationDamping = m_MovementRotationDamping;
        m_MovementSpeed = m_OriginalMovementSpeed;
        m_WeaponCollider.enabled = false;
    }

    #endregion

    #region Action Functions

    private void Attack()
    {
        m_Rigidbody.velocity = Vector3.zero;
        // Reset the movement animator parameters.
        m_Animator.SetFloat(m_SpeedParam, 0);

        // Set weapon type and attack type.
        m_Animator.SetInteger(m_WeaponTypeParam, 1);
        m_Animator.SetTrigger(m_AttackParam);

        // Set attack game-feel parameters.
        m_RotationDamping = m_AttackRotationDamping;
        m_Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        
        if (lockedEnemy != null && Vector3.Distance(lockedEnemy.transform.position, gameObject.transform.position) > minDist && Vector3.Distance(lockedEnemy.transform.position, gameObject.transform.position) < maxDist)
        {
            Debug.Log("Ahora");
            transform.rotation = Quaternion.LookRotation(lockedEnemy.transform.position - transform.position);
            m_Rigidbody.MovePosition(transform.position + ((lockedEnemy.transform.position - gameObject.transform.position) * 0.45f));
        }
           
        // Activate weapon.
        m_Weapon.Attack();
    }

    private void Dash()
    {
        m_RotationDamping = m_DashRotationDamping;
        m_MovementSpeed = m_DashMovementSpeed;

        gameObject.layer = LayerMask.NameToLayer("Dash");
        m_WeaponCollider.enabled = false;

        m_Animator.SetTrigger(m_DashParam);
        m_Rigidbody.AddForce(transform.forward * m_DashSpeed * Time.fixedDeltaTime, ForceMode.Impulse);
    }

    private void Spell()
    {
        m_Rigidbody.velocity = Vector3.zero;
        m_WeaponCollider.enabled = false;

        // Reset the movement animator parameters.
        m_Animator.SetFloat(m_SpeedParam, 0);

        // Set weapon type and attack type.
        m_Animator.SetInteger(m_SpellTypeParam, 1);

        // The animation is the one who calls UseAbility() via animation events.
        m_Animator.SetTrigger(m_SpellParam);
    }

    #endregion


    public void CheckDead()
    {
        if (Health <= 0 && !GameController.instance.godMode)
        {
            GameController.instance.died = true;
            GameController.instance.scene = GameController.GameState.LOBBY;
            SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
        }
    }

    public void Resume()
    {
        m_CanMove = true;
    }

    public void Stop()
    {
        m_CanMove = false;
        m_Animator.SetFloat(m_SpeedParam, 0);
    }

    public void EquipModifier(Modifier modifier)
    {
        m_Weapon.modifier = modifier;
    }

}
