﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BombBehaviour : MonoBehaviour {

    [HideInInspector] public float explotionForce;
    [HideInInspector] public GameObject effect;

    private Rigidbody rb;
    private List<Collider> others = new List<Collider>();


    private void OnCollisionEnter(Collision collision) {
        if(GetComponent<AbilityHolder>().holder.target == "Enemy" && collision.gameObject.tag != "Player") {
            Explode();
        }
        else if (GetComponent<AbilityHolder>().holder.target == "Player" && collision.gameObject.tag != "Enemy") {
            Explode();
        }
    }

    private void Explode() {
        GetComponent<CapsuleCollider>().enabled = true;

        Vector3 otherPos, forceDir, position = gameObject.transform.position;
        foreach (Collider other in others) {
            if(other.gameObject.tag == GetComponent<AbilityHolder>().holder.target) { 
                otherPos = other.GetComponent<Transform>().position;
                forceDir = otherPos - position;
                forceDir = forceDir + new Vector3(0, 0.5f, 0);
                other.GetComponent<Rigidbody>().AddForce(forceDir.normalized * explotionForce);
            }
        }

        Instantiate(effect, transform.position, Quaternion.identity).transform.localEulerAngles += new Vector3(90, 0, 0);
        CameraManager.instance.Shake(0.1f, 0.08f);
        AudioManager.Instance.PlayEffect("BombExplosion");
        StartCoroutine(DestroyBomb());
    }

    private IEnumerator DestroyBomb() {
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        if (!others.Contains(other)) others.Add(other);
    }

    private void OnTriggerExit(Collider other) {
        if (others.Contains(other)) others.Remove(other);
    }
}
