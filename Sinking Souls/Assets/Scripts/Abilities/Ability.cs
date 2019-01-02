using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public enum AbilityType { PASSIVE, PREFAB, USE };

    [Header("Ability Information")]
    new public string name;
    public string description;
    public int price;
    public Sprite sprite;

    [Header("General Properties")]
    public int cooldown;
    public float damage;

    public AbilityType abilityType;
    public Modifier modifier;
    public GameObject prefab;
    

    [HideInInspector] public string target;
    [HideInInspector] public Entity entity;

    protected GameObject parent;


    public void Use(GameObject newParent, Transform position = null)
    {
        SetParent(newParent);
        SetEntity();

        switch (abilityType)
        {
            case AbilityType.PASSIVE:
            case AbilityType.USE:
                Activate();
                break;

            case AbilityType.PREFAB:
                if (position == null)
                {
                    Configure(SetPrefab(entity.m_WeaponHand.transform));
                }
                else
                {
                    Configure(SetPrefab(position));
                }
                break;

            default:
                break;
        }
    }

    // For prefab ability type.
    protected virtual void Configure(GameObject prefab) { }

    // For passive ability type.
    public virtual void Passive(GameObject go) { }

    // For passive and use ability type.
    public virtual void Activate() {}


    public bool IsPassive()
    {
        return abilityType == AbilityType.PASSIVE;
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
        if (parent == null) parent = newParent;
    }

    protected void SetEntity()
    {
        if (entity == null && parent.GetComponent<Entity>()) entity = parent.GetComponent<Entity>();
    }
    #endregion
}
