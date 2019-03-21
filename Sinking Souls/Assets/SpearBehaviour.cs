using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearBehaviour : MonoBehaviour
{

    public Transform Target;
    public GameObject fire;
    public float fireDuration;
    public float firingAngle = 45.0f;
    private float gravity = 9.8f;

    private Vector3 startingPos;
    private float Vy;
    private float Vx;
    private float elapse_time = 0;

    // Use this for initialization
    void Start()
    {
        transform.position += Vector3.up;

        Target = GameController.instance.player.gameObject.transform;

        startingPos = transform.position;

        float target_Distance = Vector3.Distance(transform.position, Target.position);

        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        // Extract the X  Y componenent of the velocity
        Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        transform.rotation = Quaternion.LookRotation(Target.position - transform.position);

        // Calculate flight time.
        float flightDuration = target_Distance / Vx;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);
        elapse_time += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag != "Enemy")
        {
            if (other.tag != "Player")
            {
                GameObject aux = Instantiate(fire);
                aux.transform.position = transform.position;
                Destroy(aux, fireDuration);
            }

            Destroy(gameObject);
        }
    }


}