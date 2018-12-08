using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpansiveWaveBehaviour : MonoBehaviour
{
    [Header("Properties")]
    public float range = 15;
    public float offset = 2;
    public float speed = 5;
    public int damage;

    [HideInInspector] public bool exteriorCollision;
    [HideInInspector] public bool interiorCollision;
    private SphereCollider exterior;
    private SphereCollider interior;
    private bool hitted;

    private void Start()
    {
        hitted = false;
        interiorCollision = false;
        exteriorCollision = false;
        interior = transform.GetChild(0).GetComponent<SphereCollider>();
        exterior = transform.GetChild(1).GetComponent<SphereCollider>();
        interior.radius = 0;
        exterior.radius = interior.radius + offset;
    }

    private void Update()
    {
        if (exterior.radius <= range)
        {
            interior.radius += speed * Time.deltaTime;
            exterior.radius = interior.radius + offset;
        }
        else Destroy(gameObject);
     

        if (exteriorCollision && !interiorCollision && !hitted)
        {
            hitted = true;
            GameController.instance.player.GetComponent<Entity>().TakeDamage(damage);
        }
    }

}
