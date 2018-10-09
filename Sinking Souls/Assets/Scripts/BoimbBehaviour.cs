using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoimbBehaviour : MonoBehaviour {

    private Rigidbody rb;
    [HideInInspector]
    public bool exploding = false;
    private List<Collider> others = new List<Collider>();
    public float explotionForce;

	void Start () {
	}
	
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision) {
        Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.tag != "Player"){ //&& collision.gameObject.tag != "Weapon") { 
            exploding = true;
            explode();
        }
    }

    private void explode() {
        foreach(Collider other in others) {
            if(other.gameObject.tag == "Enemy") { 
                Vector3 otherPos = other.GetComponent<Transform>().position;
                Vector3 position = gameObject.transform.position;
                Vector3 forceDir = new Vector3(otherPos.x - position.x, otherPos.y - position.y, otherPos.z - position.z);
                other.GetComponent<Rigidbody>().AddForce(forceDir.normalized * explotionForce);
            }
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        if (!others.Contains(other)) others.Add(other);

    }
    private void OnTriggerExit(Collider other) {
        if (others.Contains(other)) others.Remove(other);
    }
}
