using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{

    [Header("Ability Information")]
    new public string name;
    public string description;
    public int price;
    public Sprite sprite;

    [Header("General Properties")]
    public int cooldown;
    public float damage;
    public bool passive = false;
    public Modifier modifier;
    public GameObject prefab;

    [Header("Specific Properties")]

    [HideInInspector] public string target;
    [HideInInspector] public Entity entity;

    protected GameObject parent;

    public void Use(GameObject newParent) {
        if (!passive) {
            SetParent(newParent);
            SetEntity();
            if (CheckThrown()) {
                Configure(SetPrefab());
            }
        }else {
            Activate();
        }
    }

    public virtual void Passive(GameObject go) { }

    public virtual void Activate() {}

    protected abstract void Configure(GameObject prefab);

    protected GameObject SetPrefab()
    {
        GameObject instantiated = Instantiate(prefab);
        instantiated.transform.position = entity.hand.transform.position;

        target = parent.gameObject.tag == "Player" ? "Enemy" : "Player";
        instantiated.AddComponent<AbilityHolder>().holder = this;

        return instantiated;
    }

    protected bool CheckThrown()
    {
        if (!entity.thrown)
        {
            entity.thrown = true;
            return true;
        }

        return false;
    }

    protected void SetParent(GameObject newParent)
    {
        if (parent == null) parent = newParent;
    }

    protected void SetEntity()
    {
        if (entity == null) entity = parent.GetComponent<Entity>();
    }
}
