using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour {

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
