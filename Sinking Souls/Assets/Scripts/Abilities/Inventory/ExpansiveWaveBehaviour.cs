using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpansiveWaveBehaviour : MonoBehaviour
{
    [Header("Properties")]
    public float range = 15;
    public float offset = 2;
    public float speed = 5;
    public float damage;

    public bool exteriorCollision;
    public bool interiorCollision;
    private SphereCollider exterior;
    private SphereCollider interior;
    public ParticleSystem effect;
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
        effect = Instantiate(effect);
        effect.transform.SetParent(gameObject.transform, false);
        ParticleSystem.MainModule psMain = effect.main;
        psMain.startSpeed = speed * 0.45f;
        effect.transform.position = transform.position;
        effect.Play();
        Destroy(gameObject, range / speed);
    }

    private void Update()
    {
        if (exterior.radius <= range)
        {
            interior.radius += speed * Time.deltaTime;
            exterior.radius = interior.radius + offset;
        }
     

        if (exteriorCollision && !interiorCollision && !hitted)
        {
            hitted = true;
            Debug.Log("Hit");
            GameController.instance.player.GetComponent<Entity>().ApplyDamage(damage);
            GameController.instance.player.GetComponent<Entity>().React(transform.position); ;
        }
    }

}
