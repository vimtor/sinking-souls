using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ArrowBehaviour : MonoBehaviour {

    [HideInInspector] public Vector3 direction;
     public float arrowForce;

    private void Start() {

        transform.localEulerAngles += new Vector3(0, 90, 0);
        GetComponent<Rigidbody>().AddForce(direction * arrowForce);

    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == GetComponent<AbilityHolder>().holder.target) {
            StartCoroutine(DestroyArrow());
        }
    }

    private IEnumerator DestroyArrow() {
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }

}
