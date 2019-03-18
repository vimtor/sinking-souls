using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullBehaviour : MonoBehaviour {
    public float speed;
    public float lerp;
    public GameObject particles;
    public float particlesLife;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Rigidbody>().velocity = Vector3.Lerp(GetComponent<Rigidbody>().velocity, (((GameController.instance.player.transform.position + (Vector3.up)*1.5f) - transform.position).normalized * speed) + new Vector3(0, (Mathf.Sin(Time.time * 5) * 6f), 0), lerp);
    }

    private void OnCollisionEnter(Collision collision) {
        Destroy(gameObject);
        GameObject part = Instantiate(particles);
        part.transform.position = transform.position;
        Destroy(part, 0.6f);
    }
}
