using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enhancer : ScriptableObject
{

    [Header("Enhancer Information")]
    new public string name;
    public string description;
    public int basePrice;
    public int baseEnhancer;
    public float priceMultiplier;
    public float enhancerMultiplier;
    public Sprite sprite;

    [Header("General Properties")]
    public bool life;
    public bool damage;
    //public bool passive = false;
    //public Modifier modifier;
    //public GameObject prefab;

    [Header("Specific Properties")]

    [HideInInspector] public string target;
    [HideInInspector] public Entity entity;

    protected GameObject parent;

    public void Use(GameObject newParent, Transform position = null)
    {
        {
            SetParent(newParent);
            SetEntity();

        }
    }

    protected void SetParent(GameObject newParent)
    {
        if (parent == null) parent = newParent;
    }

    protected void SetEntity()
    {
        if (entity == null && parent.GetComponent<Entity>()) entity = parent.GetComponent<Entity>();
    }
}