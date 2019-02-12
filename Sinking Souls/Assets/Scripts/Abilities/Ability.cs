using UnityEngine;

public abstract class Ability : ScriptableObject
{
    #region Ability Information
    [Header("Ability Information")]
    new public string name;
    public string description;
    public int price;
    public Sprite sprite;
    #endregion

    #region Ability Properties
    [Header("General Properties")]
    public int cooldown;
    public float damage;

    [SerializeField] protected bool m_IsPassive;
    public bool IsPassive
    {
        get { return m_IsPassive; }
    }

    public Modifier modifier;
    public GameObject prefab;
    #endregion

    #region Upgrade Variables
    [System.Serializable]
    public class Upgrade
    {
        [SerializeField] private int m_CooldownReduction;
        public int CooldownReduction
        {
            get { return m_CooldownReduction; }
        }

        [SerializeField] private float m_DamageMultiplier;
        public float DamageMultiplier
        {
            get { return m_DamageMultiplier; }
        }

        [SerializeField] private int m_PriceMultiplier;
        public int PriceMultiplier
        {
            get { return m_PriceMultiplier; }
        }
    }

    public Upgrade[] m_Upgrades;
    public int m_UpgradeNumber = 0;
    #endregion

    [HideInInspector] public string target;
    [HideInInspector] public Entity entity;

    protected GameObject parent;


    public void Use(GameObject newParent)
    {
        SetParent(newParent);
        SetEntity();

        if (m_IsPassive) Activate();
        else Configure(SetPrefab(entity.WeaponHand.transform));
    }

    // For prefab ability type.
    protected virtual void Configure(GameObject prefab) { }

    // Passive behaviour.
    public virtual void Passive(GameObject go) { }

    // For passive and use ability type.
    public virtual void Activate() { }

    public void UpgradeAbility()
    {
        if (CanUpgrade())
        {
            damage *= m_Upgrades[m_UpgradeNumber].DamageMultiplier;
            cooldown -= m_Upgrades[m_UpgradeNumber].CooldownReduction;

            price *= m_Upgrades[m_UpgradeNumber].PriceMultiplier;
        }
        
        m_UpgradeNumber++;
    }

    public bool CanUpgrade()
    {
        return m_Upgrades.Length >= m_UpgradeNumber;
    }

    #region Configure Functions
    protected GameObject SetPrefab(Transform position)
    {
        GameObject instantiated = Instantiate(prefab);
        instantiated.transform.position = position.position;

        target = parent.gameObject.tag == "Player" ? "Enemy" : "Player";
        instantiated.AddComponent<AbilityHolder>();
        instantiated.GetComponent<AbilityHolder>().holder = this;
        instantiated.GetComponent<AbilityHolder>().owner = parent;

        return instantiated;
    }

    protected void SetParent(GameObject newParent)
    {
        parent = newParent;
    }

    protected void SetEntity()
    {
        if (entity == null && parent.GetComponent<Entity>()) entity = parent.GetComponent<Entity>();
    }
    #endregion
}
