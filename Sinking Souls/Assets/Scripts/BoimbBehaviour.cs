using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoimbBehaviour : MonoBehaviour {

    private Rigidbody rb;
    private List<Collider> others = new List<Collider>();
    public float explotionForce;

	void Start () {
	}
	
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision) {
        Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.tag != "Player"){
            Explode();
        }
    }

    private void Explode() {
        Vector3 otherPos, forceDir, position = gameObject.transform.position;
        foreach (Collider other in others) {
            if(other.gameObject.tag == "Enemy") { 
                otherPos = other.GetComponent<Transform>().position;
                forceDir = new Vector3(otherPos.x - position.x, otherPos.y - position.y, otherPos.z - position.z);
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
