using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackController : MonoBehaviour {

    [Header("Game-feel parameters")]
    public float movementDamping = 0.15f;
    public float rotationDamping = 0.15f;
    public float movementSpeed = 100.0f;
    public float dashSpeed = 100.0f;

    [Header("Character Equipment")]
    public Ability ability;
    public Weapon weapon;

    private Animator animator;
    private Rigidbody rb;

    private readonly int horizontalParam = Animator.StringToHash("Horizontal");
    private readonly int verticalParam = Animator.StringToHash("Vertical");
    private readonly int attackParam = Animator.StringToHash("Attack");
    private readonly int spellParam = Animator.StringToHash("Spell");

    private readonly int weaponTypeParam = Animator.StringToHash("WeaponType");
    private readonly int spellTypeParam = Animator.StringToHash("SpellType");
    private readonly int attackTypeParam = Animator.StringToHash("AttackType");

    void Start () {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
	}

    private void FixedUpdate() {
        Rotate();
        Move();

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
        Vector3 desiredPosition = new Vector3(InputHandler.LeftJoystick.x, 0, InputHandler.LeftJoystick.y);
        Vector3 direction = transform.position + desiredPosition;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationDamping * Time.deltaTime);
    }

    /// <summary>
    /// Moves the character to the desired position and
    /// also changes the basic locomotion parameters of the blend tree.
    /// </summary>
    private void Move() {
        // Map character velocity respect forward instead of joystick.
        animator.SetFloat(horizontalParam, InputHandler.LeftJoystick.x, movementDamping, Time.deltaTime);
        animator.SetFloat(verticalParam, InputHandler.LeftJoystick.y, movementDamping, Time.deltaTime);

        
        Vector3 direction = new Vector3(InputHandler.LeftJoystick.x, 0, InputHandler.LeftJoystick.y);
        rb.MovePosition(transform.position + direction * movementSpeed * Time.deltaTime);
    }

    private void Attack() {
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
        rb.AddForce(transform.forward * dashSpeed * Time.deltaTime, ForceMode.Impulse);
    }

    // Refactor Entity to play the hit animation.
    private void Hit() {

    }
}
