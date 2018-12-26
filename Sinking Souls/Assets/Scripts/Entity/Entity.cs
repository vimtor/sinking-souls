using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Entity : MonoBehaviour {

    #region Entity Stats
    [Header("Stats")]
    [SerializeField] protected float m_Health;
    public float Health
    {
        get { return m_Health; }
    }

    [SerializeField] protected float m_MaxHealth;
    public float MaxHealth
    {
        get { return m_MaxHealth; }
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
    [SerializeField] protected Ability m_Ability;
    public Ability Ability
    {
        get { return m_Ability; }
        set { m_Ability = value; }
    }
    [Tooltip("Where the weapon will be instantiated.")]
    public GameObject m_WeaponHand;
    #endregion

    #region Particles
    [Header("Particles")]
    public GameObject m_HitParticles;
    public GameObject m_HealParticles;
    #endregion

    #region Default Components
    protected Rigidbody m_Rigidbody;
    protected Animator m_Animator;
    public Animator Animator
    {
        get { return m_Animator; }
    }
    private CapsuleCollider m_CapsuleCollider;
    #endregion

    [HideInInspector] public bool thrown; 
    [HideInInspector] public bool hit = false;
    [HideInInspector] public enum ModifierState { FIRE, TOXIC, ELECTRIC, ICE };
    [HideInInspector] public Dictionary<ModifierState, int> currentModifierState = new Dictionary<ModifierState, int>();
    [HideInInspector] public Color originalColor;
    [HideInInspector] public bool gettingDamage = false;


    protected void OnStart()
    {
        originalColor = transform.GetChild(1).GetComponent<Renderer>().material.color;

        for (int i = 0; i < 4; i++) {
            currentModifierState[(ModifierState)i] = 0;
        }
        
        // Get the necessary components.
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        m_CapsuleCollider = GetComponent<CapsuleCollider>();

        EquipWeapon();
    }

    protected void Apply(Modifier modifier) {
        if(modifier != null) {
            modifier.Apply(gameObject);
        }
    }

    public void EquipWeapon() {
        m_Weapon.Instantiate(m_WeaponHand, gameObject);
    }

    public void TakeDamage(float damage) {
        m_Health -= damage;
        transform.GetChild(1).GetComponent<Renderer>().material.color = Color.red;
        gettingDamage = true;
        GameController.instance.StartCoroutine(ResetColor(0.1f));
    }

    public void Heal(float heal) {
        m_Health += m_Health + heal > m_MaxHealth ? m_MaxHealth : heal;

        bool noHeal = true;
        for(int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).tag == "FxTemporaire") noHeal = false;
        }

        if (noHeal) {
            GameObject healGo = Instantiate(m_HealParticles);
            healGo.transform.position = gameObject.transform.position;
            healGo.transform.parent = gameObject.transform;
        }
    }

    public void Heal() {
        Heal(m_MaxHealth);
    }

    IEnumerator ResetColor(float time) {
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


    private void OnTriggerEnter(Collider other) {

        if (other.tag == "Weapon" && other.GetComponent<WeaponHolder>().owner.tag != tag)
        {
            if (other.GetComponent<WeaponHolder>().holder.hitting && !hit) {
                hit = true;
                TakeDamage(other.GetComponent<WeaponHolder>().holder.damage);
                Apply(other.GetComponent<WeaponHolder>().holder.modifier);
                
                if(tag == "Enemy") CameraManager.instance.Hit(0.05f, 2.5f);
                GameObject hitO = Instantiate(m_HitParticles);
                hitO.transform.position = other.transform.position;
                Destroy(hitO, 1);
            }
        }
        else if (other.tag == "Ability")
        {
            if(gameObject.tag == other.GetComponent<AbilityHolder>().holder.target) {
                TakeDamage(other.GetComponent<AbilityHolder>().holder.damage);
                Apply(other.gameObject.GetComponent<AbilityHolder>().holder.modifier);
            }
        }
    }


    private void OnTriggerExit(Collider other) {

        if (other.tag == "Weapon") {
            hit = false;
        }
        else if (other.tag == "Ability") {
            if (gameObject.tag == other.GetComponent<AbilityHolder>().holder.target) {
                hit = false;
            }
        }

    }
}
