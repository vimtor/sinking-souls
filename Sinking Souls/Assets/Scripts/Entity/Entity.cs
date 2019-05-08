using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Entity Stats
    [Header("Stats")]
    [SerializeField] protected float m_Health;
    public float Health
    {
        set { m_Health = value; }
        get { return m_Health; }
    }

    [SerializeField] protected float m_MaxHealth;
    public float MaxHealth
    {
        set { m_MaxHealth = value; }
        get { return m_MaxHealth; }
    }

    [SerializeField] protected float m_HittedRecovery;
    protected bool m_Hitted;
    public bool Hitted
    {
        get { return m_Hitted; }
    }
    #endregion

    #region Equipment Variables
    [Header("Equipment")]
    [SerializeField] protected Weapon m_Weapon;
    public Weapon Weapon
    {
        get { return m_Weapon; }
        set { m_Weapon = value; }
    }

    [Tooltip("Where the weapon will be instantiated.")]
    [SerializeField] protected GameObject m_WeaponHand;
    public GameObject WeaponHand
    {
        get { return m_WeaponHand; }
    }
    public bool leftHanded;
    [Tooltip("Where the other weapon will be instantiated.")]
    [SerializeField] protected GameObject m_OtherHand;
    public GameObject OtherHand {
        get { return m_OtherHand; }
    }

    protected BoxCollider m_WeaponCollider;

    [SerializeField] protected Ability[] m_Abilities;
    public Ability[] Abilities
    {
        get { return m_Abilities; }
        set { m_Abilities = value; }
    }
    
    protected float m_AbilityCooldown;
    public float AbilityCooldown
    {
        get { return m_AbilityCooldown; }
    }
    #endregion

    #region Particles
    [Header("Particles")]
    public GameObject m_HitParticles;
    public GameObject m_HealParticles;
    #endregion

    #region Default Components
    protected Rigidbody m_Rigidbody;
    public Animator m_Animator;
    public Animator Animator
    {
        get { return m_Animator; }
    }
    private CapsuleCollider m_CapsuleCollider;
    #endregion

    #region Modifier Variables
    public enum ModifierState { FIRE, TOXIC, ELECTRIC, ICE };
    private Dictionary<ModifierState, int> m_CurrentModifierState;
    public Dictionary<ModifierState, int> CurrentModifierState
    {
        get { return m_CurrentModifierState; }
    }
    #endregion
    public bool dead = false;

    public GameObject lockedEnemy = null;

    // THIS NEEDS TO BE REMOVED. USE ANIMATION EVENTS INSTEAD.
    private bool m_AbilityThrown;
    public bool AbilityThrown
    {
        get { return m_AbilityThrown; }
        set { m_AbilityThrown = value; }
    }

    [HideInInspector] public Color originalColor;
    [HideInInspector] public bool gettingDamage = false;

   


    protected void OnStart()
    {
        // Get the necessary components.
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        m_CapsuleCollider = GetComponent<CapsuleCollider>();


        // Setup entity weapon.
        EquipWeapon();
        // Get weapon collider reference to avoid errors when using animation events.
        m_WeaponCollider = m_Weapon.BoxCollider;


        // To avoid different prefabs accesing the same ability.
        if (m_Abilities != null)
            for(int i = 0; i< m_Abilities.Length; i++)m_Abilities[i] = Instantiate(m_Abilities[i]);


        // Other variables.
        originalColor = transform.GetChild(1).GetComponent<Renderer>().material.color;

        m_CurrentModifierState = new Dictionary<ModifierState, int>();
        for (int i = 0; i < 4; i++) m_CurrentModifierState[(ModifierState)i] = 0;

        m_Hitted = false;
        m_HittedRecovery = 0.3f;
    }


    #region Utils Functions
    IEnumerator ResetColor(float time)
    {
        yield return new WaitForSeconds(time);

        try
        {
            gettingDamage = false;
            transform.GetChild(1).GetComponent<Renderer>().material.color = originalColor;
        }
        catch(Exception)
        {
            yield break;
        }   
    }

    public void EquipWeapon()
    {
        if(!leftHanded)m_Weapon.Instantiate(m_WeaponHand, gameObject);
        else m_Weapon.Instantiate(m_OtherHand, gameObject);
    }


    public void Heal(float heal)
    {
        // Add health until the maximun.
        m_Health = m_Health + heal > m_MaxHealth ? m_MaxHealth : m_Health + heal;

        bool noHeal = true;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).tag == "FxTemporaire") noHeal = false;
        }

        if (noHeal)
        {
            GameObject healGo = Instantiate(m_HealParticles);
            healGo.transform.position = gameObject.transform.position;
            healGo.transform.parent = gameObject.transform;
        }
    }

    public void Heal()
    {
        Health = 0;
        Heal(m_MaxHealth);
    }
    #endregion

    #region AnimationEvents Functions
    // These functions are called via animation events.

    protected void AddForce(float force) {
        Debug.Log("Addforce " + force);
        GetComponent<Rigidbody>().velocity = transform.forward.normalized * force;
        Debug.Log("velocity1 " + m_Rigidbody.velocity);
    }
    protected void Stop() {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }


    protected void EnableCollider() { m_WeaponCollider.enabled = true; }
    protected void EnablePerfect()
    {
        transform.GetChild(2).GetComponent<BoxCollider>().enabled = true;

    }
    protected void DisablePerfect()
    {
        transform.GetChild(2).GetComponent<BoxCollider>().enabled = false;

    }
    protected void EnableNormal()
    {
        transform.GetChild(3).GetComponent<BoxCollider>().enabled = true;

    }
    protected void DisableNormal()
    {
        transform.GetChild(3).GetComponent<BoxCollider>().enabled = false;

    }
    protected void DisableCollider() { m_WeaponCollider.enabled = false; }

    protected void UseAbility(int abilityID)
    {
        m_AbilityCooldown = m_Abilities[abilityID].cooldown;
        m_Abilities[abilityID].Use(gameObject);
    }

    protected void PlaySound(string name)  { AudioManager.Instance.PlayEffect(name);  }
    protected void StopSound(string name)  { AudioManager.Instance.Stop(name);  }
    protected void PauseSound(string name) { AudioManager.Instance.Pause(name); }
    #endregion

    #region React Functions

    public void React(Vector3 hitterPosition)
    {

        m_Hitted = true;

        Vector3 hitDirection = hitterPosition - transform.position;
        Vector3 hitPosition = Quaternion.Inverse(transform.rotation) * hitDirection.normalized;

        m_Animator.SetFloat("HitX", hitPosition.x);
        m_Animator.SetFloat("HitY", hitPosition.z);
        m_Animator.SetTrigger("React");
        if (gameObject.tag == "Player") Time.timeScale = 0.1f;
        StartCoroutine(ContiuneGame(0.03f));
        StartCoroutine(ReactCoroutine());
        if(m_WeaponCollider!=null) m_WeaponCollider.enabled = false;
    }

    private IEnumerator ContiuneGame(float t) {
        yield return new WaitForSecondsRealtime(t);
        Time.timeScale = 1f;


    }

    private IEnumerator ReactCoroutine()
    { 
        yield return new WaitForSecondsRealtime(m_HittedRecovery);
        m_Hitted = false;
    }

    #endregion

    #region Damage Functions
    protected void ApplyModifier(Modifier modifier)
    {
        if (modifier != null)
        {
            modifier.Apply(gameObject);
        }
    }

    public void ApplyDamage(float damage)
    {
        m_Health -= damage;
        transform.GetChild(1).GetComponent<Renderer>().material.color = Color.red;
        gettingDamage = true;
        GameController.instance.StartCoroutine(ResetColor(0.1f));
    }

    #endregion

    private void Update() {
    }
    [HideInInspector]public bool deadButWaiting = false;
    private void OnTriggerEnter(Collider other)
    {
        if (m_Hitted || dead || deadButWaiting) return;
        switch (other.tag)
        {
            case "Weapon":
                if (other.GetComponent<WeaponHolder>().owner.tag != tag)
                {
                    ApplyDamage(other.GetComponent<WeaponHolder>().holder.damage);

                    GameObject hitParticles = Instantiate(m_HitParticles);
                    hitParticles.transform.position = other.transform.position + Vector3.Normalize((transform.position + Vector3.up) - (other.transform.position )) *2 ;
                    Destroy(hitParticles, 1);

                    if (gameObject.tag == "Player")
                    {
                        gameObject.GetComponent<Player>().Dodge = Player.DodgeType.NONE;
                        AudioManager.Instance.PlayEffect("PlayerHit");
                    }
                    
                    if (other.GetComponent<WeaponHolder>().owner.tag == "Player") other.GetComponent<WeaponHolder>().owner.GetComponent<Entity>().lockedEnemy = gameObject;
                    React( other.ClosestPointOnBounds(gameObject.transform.position));
                    ApplyModifier(other.GetComponent<WeaponHolder>().holder.modifier);

                    if (other.GetComponent<WeaponHolder>().owner.tag == "Player") {
                        AudioManager.Instance.PlayEffect("Splash");
                        Debug.Log("Fart");
                    }

                    if (other.name.Contains("Dagger")) {
                        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
                        StartCoroutine(UnfreezePosition(0.1f));
                    }
                }
                break;

            case "Ability":
                if (gameObject.tag == other.GetComponent<AbilityHolder>().holder.target)
                {
                   
                    ApplyDamage(other.GetComponent<AbilityHolder>().holder.damage);
                    React(other.transform.position);
                    ApplyModifier(other.gameObject.GetComponent<AbilityHolder>().holder.modifier);
                }

                if (other.GetComponent<WeaponHolder>().owner.tag != "Player")
                {
                    AudioManager.Instance.PlayEffect("PlayerHit");
                }
                break;
        }

       
    }
    
    IEnumerator UnfreezePosition(float t) {
        yield return new WaitForSeconds(t);
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

    }

}
