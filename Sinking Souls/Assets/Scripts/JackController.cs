using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackController : MonoBehaviour {

    [Header("Game-feel parameters")]
    public float movementDamping = 0.15f;
    public float rotationDamping = 0.15f;
    public float movementSpeed = 100.0f;
    public float dashSpeed = 100.0f;
    public float dashLength = 1;

    [Header("Animator parameters")]
    public float attackLength = 1;
    public float attackSpeed = 1;

    [Header("Character Equipment")]
    public Ability ability;
    public Weapon weapon;

    private Animator animator;
    private Rigidbody rb;

    private bool movable = true;

    private readonly int speedParam = Animator.StringToHash("Speed");
    private readonly int attackParam = Animator.StringToHash("Attack");
    private readonly int dashParam = Animator.StringToHash("Dash");
    private readonly int spellParam = Animator.StringToHash("Spell");
    private readonly int weaponTypeParam = Animator.StringToHash("WeaponType");
    private readonly int spellTypeParam = Animator.StringToHash("SpellType");
    private readonly int attackTypeParam = Animator.StringToHash("AttackType");

    private Vector3 forward;
    private Vector3 side;

    void Start () {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        side = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;

        foreach(var anim in animator.runtimeAnimatorController.animationClips) {
            if (anim.name == "Walking" || anim.name == "Running") { }
        }

    }

    private void FixedUpdate() {

        Vector3 horizontalMovement = side * InputHandler.LeftJoystick.x;
        Vector3 verticalMovement = forward * InputHandler.LeftJoystick.y;
        Vector3 direction = horizontalMovement - verticalMovement;


        float magnitude = Vector3.Magnitude(direction);
        animator.SetFloat(speedParam, magnitude, movementDamping, Time.deltaTime);

        if (!InputHandler.LeftJoystickZero() && movable) {
            Rotate();

            rb.MovePosition(transform.position + transform.forward * movementSpeed * magnitude * Time.deltaTime);
        }
        

        if (InputHandler.ButtonX()) {
            Attack();
        }

        if (InputHandler.ButtonY()) {
            Spell();
        }

        if (InputHandler.ButtonB()) {
            Dash();
        }
    }

    /// <summary>
    /// Rotates the character to the desired position smoothly, using an specified rotation damping.
    /// </summary>
    private void Rotate() {
        Vector3 horizontalMovement = side * InputHandler.LeftJoystick.x;
        Vector3 verticalMovement = forward * InputHandler.LeftJoystick.y;

        Vector3 direction = horizontalMovement - verticalMovement;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationDamping);
    }

    /// <summary>
    /// Moves the character to the desired position and
    /// also changes the basic locomotion parameters of the blend tree.
    /// </summary>
    private void Move() {

        
    }

    private void Attack() {
        movable = false;
        animator.SetTrigger(attackParam);

        // Set via weapon type.
        animator.SetInteger(weaponTypeParam, 0);
        animator.SetInteger(attackParam, 0);

        // Activate weapon.
    }

    private void Spell() {
        animator.SetTrigger(spellParam);
        ability.Use(gameObject);
    }

    private void Dash() {
        movable = false;
        animator.SetTrigger(dashParam);
        rb.AddForce(transform.forward * dashSpeed * Time.fixedDeltaTime, ForceMode.Impulse);
        StartCoroutine(StopDash());
    }
    IEnumerator StopDash() {
        yield return new WaitForSeconds(dashLength);
        rb.velocity = Vector3.zero;
        movable = true;
    }

    // Refactor Entity to play the hit animation.
    private void Hit() {

    }
}
