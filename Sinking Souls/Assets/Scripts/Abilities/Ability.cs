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

     public string target;
     public Entity entity;

     public GameObject parent;


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
