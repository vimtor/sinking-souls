using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleProp : MonoBehaviour {

	public GameObject destroyedVersion;

    private void OnTriggerEnter(Collider other) {
        if(other.GetComponent<WeaponHolder>()) Destruct();

    }
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player" && other.gameObject.layer == 10) Destruct();
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject.layer == 10) Destruct();
    }

    void Destruct()
    {
        AudioManager.Instance.Play(Random.value < 0.5 ? "BreakingJar01" : "BreakingJar02");
        Instantiate(destroyedVersion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
