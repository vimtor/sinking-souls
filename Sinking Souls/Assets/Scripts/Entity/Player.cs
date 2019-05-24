using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class Player : Entity
{
    #region State Logic

    public delegate void StateAction();

    public Vector3 repulsionVector;
    public float repulsionForce;
    public float repulsionTime;
    public enum PlayerState
    {
        DASHING,
        MOVING,
        ATTACKING,
        REACTING,
        SPELLING,
        PULLING,
        REPULSED,
        NONE
    };

    public PlayerState m_PlayerState;

    [HideInInspector]
    public enum DodgeType
    {
        NONE,
        NORMAL,
        PERFECT
    };

    public DodgeType Dodge;

    // To avoid transition overlaping.
    private byte m_TransitionCount;

    #endregion

    #region Game-feel Variables

    [Header("Locked dash degrees")]
    [Tooltip("From 9 oclock to 3 oclock, make shure that all of this parameters add up to 180 only!")]
    public float leftOffset = 5;

    public float leftDegrees = 26.6666f;
    public float topDegrees = 26.6666f;
    public float rightDegrees = 26.6666f;
    public float dashSpawningDistance;

    [Tooltip("how much tho the ceneter does the player spawn when he teleports to left and rigth")]
    public float centered;

    public float dashTime;
    [Header("Movement parameters")] public float dodgeDelay;
    public float m_MovementDamping;
    public float m_MovementSpeed;
    public float lockDistance;
    public float minStepDist = 1.7f;
    public float maxStepDist = 3f;

    public float MovementSpeed
    {
        get { return m_MovementSpeed; }
        set
        {
            if (value > 0) m_MovementSpeed = value;
        }
    }

    public float m_MaxMovementSpeed;
    public float m_MovementRotationDamping;
    public float m_LockDashLength = 0.2f;

    [Space(10)] public float m_DashMovementSpeed;
    public float m_lockedSpeed;
    public float m_DashSpeed;
    public float m_DashRotationDamping;

    [Space(10)] [Tooltip("How much the player will advance which each step of the attack.")] [Range(0.0f, 1.0f)]
    public float m_AttackStepForce;

    public float m_AttackRotationDamping;

    private float m_RotationDamping;
    private float m_OriginalMovementSpeed;
    private float dodgeCounter = 0;

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
        get { return m_CanMove; }
        set { m_CanMove = value; }
    }

    #endregion

    #region Shader Variables

    public enum animateShader
    {
        FORWARD,
        BACKWARDS,
        NONE
    };

    public float effectDuration;
    public Vector3 tpPosition = Vector3.zero;
    public Vector3 enemyPosition = Vector3.zero;
    public animateShader animate = animateShader.FORWARD;
    public float current = 0;

    #endregion

    public float defaultLife = 300.0f;
    private float repulsionCounter = 0;

    public bool lockedDashed = false;
    public void SetupPlayer()
    {
        transform.GetChild(1).GetComponent<Renderer>().material.SetFloat("_duration", effectDuration);
        dodgeCounter = dodgeDelay;
        OnStart();
        // Calculate the forward and side vectors relative to the camera.
        m_Forward = Camera.main.transform.forward;
        m_Forward.y = 0;
        m_Forward = Vector3.Normalize(m_Forward);
        m_Side = Quaternion.Euler(new Vector3(0, 90, 0)) * m_Forward;

        // Get animation lengths.
        var animationClips = m_Animator.runtimeAnimatorController.animationClips;
        //m_AttackLength = Array.Find(animationClips, clip => clip.name == "Klaus_1").length;
        m_AttackLength = 0.53f;

        m_DashLength = Array.Find(animationClips, clip => clip.name == "Dash").length * 0.85f;
        m_SpellLength = Array.Find(animationClips, clip => clip.name == "Throw").length;

        // Initialize private members.
        m_OriginalMovementSpeed = m_MovementSpeed;
        m_PlayerState = PlayerState.MOVING;
        m_AbilityCooldown = 0.0f;
        m_CanMove = true;
        m_WeaponCollider.enabled = false;
        m_RotationDamping = m_MovementRotationDamping;

        // Set max health.
        m_MaxHealth = GameController.instance.maxHealth <= 0 ? defaultLife : GameController.instance.maxHealth;
        m_Health = m_MaxHealth;
    }

    //value, start1, end1, new sratr, new end
    public float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    float stoppingVelocity;

    private void FixedUpdate()
    {
        if (stopping)
        {
            if(SceneManager.GetActiveScene().name == "Lobby")
            {
                {
                    GameController.instance.player.GetComponent<Rigidbody>().velocity -= (GameController.instance.player.GetComponent<Rigidbody>().velocity.normalized * stoppingVelocity) * 0.25f * Time.deltaTime;
                    m_Animator.SetFloat(m_SpeedParam, 0, m_MovementDamping * 6.5f, Time.deltaTime);
                    GameController.instance.player.GetComponent<Rigidbody>().velocity = new Vector3(GameController.instance.player.GetComponent<Rigidbody>().velocity.x, -2f, GameController.instance.player.GetComponent<Rigidbody>().velocity.z);
                    AudioManager.Instance.Stop("Walk");
                }

            }
            else
            {
                GameController.instance.player.GetComponent<Rigidbody>().velocity -= (GameController.instance.player.GetComponent<Rigidbody>().velocity.normalized * stoppingVelocity)*2f * Time.deltaTime;
                m_Animator.SetFloat(m_SpeedParam, 0, m_MovementDamping * 1.25f, Time.deltaTime);
                AudioManager.Instance.Stop("Walk");
            }

        }
        if (transform.position.y < -20) Health = -10;
        if (lockedEnemy != null && lockedEnemy.GetComponent<KlausBossAI>()) lockedEnemy = null;
        switch (Dodge) //change color
        {
            case DodgeType.NONE:
                transform.GetChild(1).GetComponent<Renderer>().material.color = Color.white;
                break;
            case DodgeType.NORMAL:
                transform.GetChild(1).GetComponent<Renderer>().material.color = Color.red;

                break;
            case DodgeType.PERFECT:
                transform.GetChild(1).GetComponent<Renderer>().material.color = Color.blue;

                break;
            default:
                break;
        }

        switch (animate)
        {
            case animateShader.BACKWARDS:
                if (current <= effectDuration)
                {
                    current += Time.deltaTime;
                    transform.GetChild(1).GetComponent<Renderer>().material
                        .SetFloat("_amount", map(current, 0, effectDuration, 0, 1));
                }
                else
                {
                    if (tpPosition != Vector3.zero)
                    {
                        m_Rigidbody.transform.position = tpPosition;
                        m_Rigidbody.velocity = Vector3.zero;

                        transform.rotation = Quaternion.LookRotation(enemyPosition - transform.position);
                    }

                    animate = animateShader.FORWARD;
                    current = 0;
                    tpPosition = Vector3.zero;
                }

                break;
            case animateShader.FORWARD:
                if (current <= effectDuration)
                {
                    current += Time.deltaTime;
                    transform.GetChild(1).GetComponent<Renderer>().material
                        .SetFloat("_amount", map(current, 0, effectDuration, 1, 0));
                }
                else
                {
                    //if(tpPosition != Vector3.zero) { 
                    //    m_Rigidbody.transform.position = tpPosition;
                    //    m_Rigidbody.velocity = Vector3.zero;

                    //    transform.rotation = Quaternion.LookRotation(lockedEnemy.transform.position - transform.position);
                    gameObject.layer = LayerMask.NameToLayer("Player");
                    Debug.Log("CAMBIO LAYER");
                    animate = animateShader.NONE;
                    current = 0;
                    tpPosition = Vector3.zero;
                }

                break;
            case animateShader.NONE:
                break;
        }
        //if (lockedEnemy != null) DrawAngles();

        if (!m_CanMove) return;
        m_Animator.SetBool("LoockedEnemy", (lockedEnemy != null));
        CheckDead();
        if (m_Abilities[0].IsPassive) m_Abilities[0].Passive(gameObject);

        m_HorizontalMovement = m_Side * InputManager.LeftJoystick.x;
        m_VerticalMovement = m_Forward * InputManager.LeftJoystick.y;
        m_Direction = m_HorizontalMovement - m_VerticalMovement;

        if (lockedEnemy != null && (lockedEnemy.transform.position - transform.position).magnitude > lockDistance)
            lockedEnemy = null;
        if (lockedEnemy != null) m_MovementSpeed = m_lockedSpeed;
        else m_MovementSpeed = m_OriginalMovementSpeed;


        switch (m_PlayerState)
        {
            case PlayerState.DASHING:
                if (lockedEnemy != null)
                {
                    transform.GetChild(1).GetComponent<Renderer>().material.color = Color.green;
                    //Rotate();
                    if (InputManager.ButtonX)
                        ChangeState(Attack, m_AttackLength, PlayerState.ATTACKING, PlayerState.MOVING);
                }
                else
                {
                    Move();
                    Rotate();
                    
                    if (InputManager.ButtonX)
                        ChangeState(Attack, m_AttackLength, PlayerState.ATTACKING, PlayerState.MOVING);
                }

                break;

            case PlayerState.REACTING:
                if (!m_Hitted) m_PlayerState = PlayerState.MOVING;
                break;

            case PlayerState.MOVING:

                if (m_Rigidbody.velocity.magnitude > 2.0f) AudioManager.Instance.PlayEffect("Walk");
                if (m_Rigidbody.velocity.magnitude < 2.0f) AudioManager.Instance.Stop("Walk");


                if (InputManager.ButtonY) gameObject.GetComponent<Hook>().Throw();

                if (lockedEnemy != null)
                {
                    Vector3 LocalSpeed = transform.InverseTransformDirection(m_Rigidbody.velocity);
                    m_Animator.SetFloat("JoystickX", map(LocalSpeed.x, -MovementSpeed, MovementSpeed, -1, 1));
                    m_Animator.SetFloat("JoystickY", map(LocalSpeed.z, -MovementSpeed, MovementSpeed, -1, 1));
                    ChangeLock();
                    CombatRotation();
                    CombatMove();

                    if (InputManager.ButtonB)
                    {
                        ChangeState(LockDash, m_LockDashLength, PlayerState.DASHING, PlayerState.MOVING, false);
                    }
                }
                else
                {
                    m_Animator.SetFloat(m_SpeedParam, m_Direction.magnitude, m_MovementDamping, Time.deltaTime);
                    Rotate();
                    Move();
                    if (InputManager.ButtonB && m_Direction.magnitude!=0)
                        ChangeState(Dash, m_DashLength, PlayerState.DASHING, PlayerState.MOVING, false);
                }

                if (InputManager.ButtonX)
                    ChangeState(Attack, m_AttackLength, PlayerState.ATTACKING, PlayerState.MOVING);
                if (InputManager.ButtonRT)
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
                if (InputManager.ButtonX)
                    ChangeState(Attack, m_AttackLength, PlayerState.ATTACKING,
                        PlayerState.MOVING); // Enable combo strings.
                if (lockedEnemy == null)
                    Rotate(); // Rotate with attacking rotation damping.
                if (lockedEnemy != null)
                {
                    
                    if (InputManager.ButtonB)
                    {
                        ChangeState(LockDash, m_LockDashLength, PlayerState.DASHING, PlayerState.MOVING, false);
                    }
                }
                else
                {
                    if (InputManager.ButtonB)
                        ChangeState(Dash, m_DashLength, PlayerState.DASHING, PlayerState.MOVING, false);
                }

                if (InputManager.ButtonY) gameObject.GetComponent<Hook>().Throw();

            break;

            case PlayerState.SPELLING:
                if (lockedEnemy != null)
                {
                    if (InputManager.ButtonB)
                    {
                        ChangeState(LockDash, m_LockDashLength, PlayerState.DASHING, PlayerState.MOVING, false);
                    }
                }
                else
                {
                    if (InputManager.ButtonB)
                        ChangeState(Dash, m_DashLength, PlayerState.DASHING, PlayerState.MOVING, false);
                }

                break;
            case PlayerState.PULLING:               
                break;

            case PlayerState.REPULSED:
            m_Rigidbody.velocity = repulsionVector.normalized * repulsionForce;
            if (repulsionCounter >= repulsionTime) {
                m_PlayerState = PlayerState.MOVING;
                repulsionCounter = 0;
            }
            else {
                repulsionCounter += Time.deltaTime;
            }
            break;

            default:
                break;
        }

        if (lockedEnemy == null && InputManager.ButtonRJ)
        {
            float dist = lockDistance + 1;
            foreach (GameObject target in GameController.instance.roomEnemies)
            {
                if (target != null)
                    if (dist > Vector3.Distance(target.transform.position, transform.position))
                    {
                        dist = Vector3.Distance(target.transform.position, transform.position);
                        lockedEnemy = target;
                        AudioManager.Instance.PlayEffect("Lock");
                    }
            }
        }
        else if (lockedEnemy != null && InputManager.ButtonRJ)
        {
            lockedEnemy = null;
            AudioManager.Instance.PlayEffect("Unlock");
        }

        if (m_AbilityCooldown > 0) m_AbilityCooldown -= Time.deltaTime;
        dodgeCounter += Time.deltaTime;
    }

    #region ChangeState Functions

    public void ChangeState(StateAction action, float delay, PlayerState newState, PlayerState returnState,
        bool resetVelocity = true)
    {
        StartCoroutine(ChangeStateCoroutine(action, delay, newState, returnState, resetVelocity));
    }

    private IEnumerator ChangeStateCoroutine(StateAction action, float delay, PlayerState newState,
        PlayerState returnState, bool resetVelocity)
    {
        m_TransitionCount++;

        m_PlayerState = newState;
        action();

        yield return new WaitForSecondsRealtime(delay);

        if (m_TransitionCount <= 1)
        {
            if (resetVelocity) m_Rigidbody.velocity = Vector3.zero;
            m_PlayerState = returnState;
            if (m_PlayerState == PlayerState.MOVING && lockedEnemy == null) ResetMovement();
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
        
        Vector3 aux = transform.forward.normalized * InputManager.LeftJoystick.magnitude * m_MovementSpeed;
        m_Rigidbody.velocity = new Vector3(aux.x, -2, aux.z);
        
        //if(velocity.magnitude < m_MaxMovementSpeed)
        //{
        //    m_Rigidbody.AddForce(transform.forward.normalized * InputManager.LeftJoystick.magnitude * m_MovementSpeed * 20);
        //}

    }

    private void CombatMove()
    {
        // In the future change this to add force and put materials with friction in them.
        // m_Rigidbody.MovePosition(transform.position + transform.forward * m_MovementSpeed * m_Direction.magnitude * Time.fixedDeltaTime);
        Vector3 velocity = m_Rigidbody.velocity;
        velocity.y = 0;

        Vector3 direction = Camera.main.transform.forward.normalized * InputManager.LeftJoystick.y * -1 +
                            (Quaternion.Euler(new Vector3(0, 90, 0)) * Camera.main.transform.forward.normalized) *
                            InputManager.LeftJoystick.x;


        m_Rigidbody.velocity = new Vector3(direction.x, 0, direction.z) * m_MovementSpeed;
    }


    private void ChangeLock()
    {
        if (InputManager.RightJoystick.magnitude > 0.5f)
        {
            Vector3 direction = Camera.main.transform.forward.normalized * InputManager.RightJoystick.y * -1 +
                                (Quaternion.Euler(new Vector3(0, 90, 0)) * Camera.main.transform.forward.normalized) *
                                InputManager.RightJoystick.x;
            float closests = 180;
            Vector2 direction2 = new Vector2(direction.x, direction.z);

            foreach (GameObject target in GameController.instance.roomEnemies)
            {
                if (target != null)
                {
                    float distance = Vector3.Distance(target.transform.position, transform.position);
                    float angle = Vector2.Angle(direction2,
                        new Vector2(target.transform.position.x, target.transform.position.z) -
                        new Vector2(gameObject.transform.position.x, gameObject.transform.position.z));


                    if (distance <= lockDistance && angle < closests)
                    {
                        lockedEnemy = target;
                        closests = angle;

                        AudioManager.Instance.PlayEffect("Lock");
                    }
                }
            }
        }
    }

    private void Rotate()
    {
        if (InputManager.LeftJoystickZero()) return;

        Quaternion rotation = Quaternion.LookRotation(m_Direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * m_RotationDamping);
    }

    private void CombatRotation()
    {
        //Movement
        Quaternion fakeRotation = Quaternion.LookRotation(m_Direction);
        fake_Forward = Quaternion.Lerp(fake_Forward, fakeRotation, Time.deltaTime * m_RotationDamping);

        //Rotation round the enemy
        Vector3 direction = lockedEnemy.transform.position - transform.position;
        direction.y = 0;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * m_RotationDamping);
    }

    public void ResetMovement()
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

        if (lockedEnemy != null &&
            Vector3.Distance(lockedEnemy.transform.position, gameObject.transform.position) > minStepDist &&
            Vector3.Distance(lockedEnemy.transform.position, gameObject.transform.position) < maxStepDist)
        {
            transform.rotation = Quaternion.LookRotation(lockedEnemy.transform.position - transform.position);
            // m_Rigidbody.MovePosition(transform.position + ((lockedEnemy.transform.position - gameObject.transform.position) * 0.45f));
            m_Rigidbody.AddForce((lockedEnemy.transform.position - gameObject.transform.position).normalized * 10,
                ForceMode.Impulse);
        }

        // Activate weapon.
        m_Weapon.Attack();
    }

    private void CritickAttack()
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

        if (lockedEnemy != null &&
            Vector3.Distance(lockedEnemy.transform.position, gameObject.transform.position) > minStepDist &&
            Vector3.Distance(lockedEnemy.transform.position, gameObject.transform.position) < maxStepDist)
        {
            transform.rotation = Quaternion.LookRotation(lockedEnemy.transform.position - transform.position);
            // m_Rigidbody.MovePosition(transform.position + ((lockedEnemy.transform.position - gameObject.transform.position) * 0.45f));
            m_Rigidbody.AddForce((lockedEnemy.transform.position - gameObject.transform.position).normalized * 10,
                ForceMode.Impulse);
        }

        // Activate weapon.
        m_Weapon.CriticAttack();
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

    private void DrawAngles()
    {
        Vector3 direction = Camera.main.transform.forward.normalized * InputManager.LeftJoystick.y * -1 +
                            (Quaternion.Euler(new Vector3(0, 90, 0)) * Camera.main.transform.forward.normalized) *
                            InputManager.LeftJoystick.x;
        Debug.DrawRay(gameObject.transform.position, new Vector3(direction.x, 0, direction.z) * 100, Color.blue);
        foreach (GameObject target in GameController.instance.roomEnemies)
        {
            Debug.DrawRay(transform.position, target.transform.position - transform.position, Color.green);

            Vector3 enemyProjection =
                Vector3.Project(
                    new Vector3(target.transform.position.x, 0, target.transform.position.z) -
                    new Vector3(transform.position.x, 0, transform.position.z),
                    new Vector3(direction.x, 0, direction.z).normalized);

            Debug.DrawRay(transform.position, new Vector3(enemyProjection.x, 0, enemyProjection.z), Color.red);

            Vector3 porjectionDirection = (transform.position + new Vector3(enemyProjection.x, 0, enemyProjection.z)) -
                                          new Vector3(target.transform.position.x, 0, target.transform.position.z);

            Debug.DrawRay(target.transform.position, new Vector3(porjectionDirection.x, 0, porjectionDirection.z),
                Color.magenta);
        }

        //Vector3 fForward = lockedEnemy.transform.position - gameObject.transform.position;
        //Vector3 aux = Quaternion.Euler(new Vector3(0, -90 + leftOffset, 0)) * fForward;
        //Vector2 start = new Vector2(aux.x, aux.z);

        //Debug.DrawRay(gameObject.transform.position, new Vector3(start.x, 0, start.y), Color.red);

        //aux = Quaternion.Euler(new Vector3(0, leftDegrees, 0)) * new Vector3(start.x,0, start.y);
        //start = new Vector2(aux.x, aux.z);
        //Debug.DrawRay(gameObject.transform.position, new Vector3(start.x, 0, start.y), Color.green);

        //aux = Quaternion.Euler(new Vector3(0, topDegrees, 0)) * new Vector3(start.x, 0, start.y);
        //start = new Vector2(aux.x, aux.z);
        //Debug.DrawRay(gameObject.transform.position, new Vector3(start.x, 0, start.y), Color.cyan);

        //aux = Quaternion.Euler(new Vector3(0, rightDegrees, 0)) * new Vector3(start.x, 0, start.y);
        //start = new Vector2(aux.x, aux.z);
        //Debug.DrawRay(gameObject.transform.position, new Vector3(start.x, 0, start.y), Color.yellow);
    }


    private void LockDash()
    {
        lockedDashed = true;
        switch (Dodge)
        {
            case DodgeType.NONE:
                Debug.Log("NONE");
                break;

            case DodgeType.NORMAL:
                Debug.Log("NORMAL");
                gameObject.layer = LayerMask.NameToLayer("Dash");
                StartCoroutine(GetBulnerable(dashTime));
                break;

            case DodgeType.PERFECT:
                Vector3 tpDirection = lockedEnemy.transform.position - transform.position;
                tpDirection = new Vector3(tpDirection.x, 0, tpDirection.z);
                tpDirection.Normalize();

                m_Rigidbody.transform.position = (lockedEnemy.transform.position + tpDirection * dashSpawningDistance);
                transform.rotation = Quaternion.LookRotation(lockedEnemy.transform.position - transform.position);
                m_Rigidbody.velocity = Vector3.zero;
                ChangeState(CritickAttack, m_AttackLength, PlayerState.ATTACKING, PlayerState.MOVING);
                gameObject.layer = LayerMask.NameToLayer("Dash");
                StartCoroutine(GetBulnerable(dashTime));

                animate = animateShader.BACKWARDS;
                AudioManager.Instance.PlayEffect("Teleport");
                return;


            default:
                break;
        }


        Vector3 direction = Camera.main.transform.forward.normalized * InputManager.LeftJoystick.y * -1 +
                            (Quaternion.Euler(new Vector3(0, 90, 0)) * Camera.main.transform.forward.normalized) *
                            InputManager.LeftJoystick.x;
        Vector2 input = new Vector2(direction.x, direction.z);
        Vector3 fForward = lockedEnemy.transform.position - gameObject.transform.position;


        if (InputManager.LeftJoystick == Vector2.zero)
        {
            ///in place
            if (Dodge != DodgeType.NONE)
            {
                animate = animateShader.BACKWARDS;
                AudioManager.Instance.PlayEffect("Teleport");
                enemyPosition = lockedEnemy.transform.position;
                tpPosition = Vector3.zero;
                gameObject.layer = LayerMask.NameToLayer("Dash");
            }

            return;
        }
        else if (Vector2.Angle(new Vector2(fForward.x, fForward.z), input) > 90 - leftOffset)
        {
            ///back
            transform.rotation =
                Quaternion.LookRotation(new Vector3(InputManager.LeftJoystick.x, 0, InputManager.LeftJoystick.y) -
                                        transform.position);
            lockedEnemy = null;

            ChangeState(Dash, m_DashLength, PlayerState.DASHING, PlayerState.MOVING, false);
            gameObject.layer = LayerMask.NameToLayer("Dash");
            return;
        }
        else
        {
            Vector3 aux = Quaternion.Euler(new Vector3(0, -90 + leftOffset, 0)) * fForward;
            Vector2 start = new Vector2(aux.x, aux.z);

            aux = Quaternion.Euler(new Vector3(0, leftDegrees, 0)) * new Vector3(start.x, 0, start.y);
            if (Vector2.Angle(start, input) < leftDegrees)
            {
                ///left
                Vector3 spawnDirection = aux + (new Vector3(start.x, 0, start.y) / centered);
                Vector3 spawnPosition =
                    lockedEnemy.transform.position + spawnDirection.normalized * dashSpawningDistance;


                animate = animateShader.BACKWARDS;
                AudioManager.Instance.PlayEffect("Teleport");
                enemyPosition = lockedEnemy.transform.position;
                tpPosition = spawnPosition;
                gameObject.layer = LayerMask.NameToLayer("Dash");
                return;
            }
            else
            {
                start = new Vector2(aux.x, aux.z);
                aux = Quaternion.Euler(new Vector3(0, topDegrees, 0)) * new Vector3(start.x, 0, start.y);
                if (Vector2.Angle(start, input) < topDegrees)
                {
                    ///top
                    Vector3 spawnDirection = aux + new Vector3(start.x, 0, start.y);
                    Vector3 spawnPosition = lockedEnemy.transform.position +
                                            spawnDirection.normalized * dashSpawningDistance;

                    animate = animateShader.BACKWARDS;
                    AudioManager.Instance.PlayEffect("Teleport");
                    enemyPosition = lockedEnemy.transform.position;
                    tpPosition = spawnPosition;
                    gameObject.layer = LayerMask.NameToLayer("Dash");

                    return;
                }
                else
                {
                    start = new Vector2(aux.x, aux.z);
                    aux = Quaternion.Euler(new Vector3(0, rightDegrees, 0)) * new Vector3(start.x, 0, start.y);
                    if (Vector2.Angle(start, input) < rightDegrees)
                    {
                        ///right
                        Vector3 spawnDirection = (aux / centered) + new Vector3(start.x, 0, start.y);
                        Vector3 spawnPosition = lockedEnemy.transform.position +
                                                spawnDirection.normalized * dashSpawningDistance;

                        animate = animateShader.BACKWARDS;
                        AudioManager.Instance.PlayEffect("Teleport");
                        enemyPosition = lockedEnemy.transform.position;
                        tpPosition = spawnPosition;
                        gameObject.layer = LayerMask.NameToLayer("Dash");

                        return;
                    }
                }
            }
        }

        Dodge = DodgeType.NONE;
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

    public float DeathLength = 1.5f;

    private float timeSpeed = 1;
    public void CheckDead()
    {
        if (!(Health <= 0) || GameController.instance.godMode) return;

        if (!deadButWaiting) {
            StartCoroutine(WaitToRestart(DeathLength));
            m_Animator.SetTrigger("Die");
            m_PlayerState = PlayerState.PULLING; // if this does somethinf change to a empty State
            GameObject.Find("Fade Plane").GetComponent<FadeEffect>().FadeOut(DeathLength - 0.15f);
        }
        Time.timeScale = 0.6f;
        deadButWaiting = true;

    }

    IEnumerator WaitToRestart(float t) {
        yield return new WaitForSecondsRealtime(t);
        GameController.instance.died = true;
        GameController.instance.roomEnemies = new List<GameObject>();
        if (ApplicationManager.Instance.state == ApplicationManager.GameState.TUTORIAL) ApplicationManager.Instance.ChangeScene(ApplicationManager.GameState.TUTORIAL);
        else ApplicationManager.Instance.ChangeScene(ApplicationManager.GameState.LOBBY);
        Time.timeScale = 1f;


    }

    public void Resume()
    {
        m_CanMove = true;
    }

    public void Stop()
    {
        m_CanMove = false;
        m_Animator.SetFloat(m_SpeedParam, 0);
        AudioManager.Instance.Stop("Walk");
    }
    bool stopping;
    public void StopForward()
    {
        m_CanMove = false;
        stopping = true;
        if (SceneManager.GetActiveScene().name == "Lobby") GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * 12;
        stoppingVelocity = GetComponent<Rigidbody>().velocity.magnitude;
    }

    public void EquipModifier(Modifier modifier)
    {
        m_Weapon.modifier = modifier;
    }

    IEnumerator GetBulnerable(float time)
    {
        Debug.Log("Entra");
        yield return new WaitForSeconds(time);
        Debug.Log("Sale");
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
}