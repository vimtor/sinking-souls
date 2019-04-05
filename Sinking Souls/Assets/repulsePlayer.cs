using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class repulsePlayer : MonoBehaviour {

    public float strength = 10;
    float time = 0;
    bool count = false;
    Quaternion rotation;

	// Use this for initialization
	void Start () {
        rotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = new Vector3(0, 1.36f, 0);
        transform.forward = transform.parent.forward;
        GetComponent<Renderer>().material.SetFloat("_dt",time);
        if( count )time += Time.deltaTime;
        else {
            time = 0;
        }
        if (time >= 1) count = false;
    }
    private void OnCollisionEnter(Collision collision) {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Player") {
            collision.gameObject.GetComponent<Player>().m_PlayerState = Player.PlayerState.REPULSED;
            collision.gameObject.GetComponent<Player>().m_Animator.SetTrigger("React");

            collision.gameObject.GetComponent<Player>().repulsionVector = (collision.transform.position - transform.position).normalized * strength;
            GetComponent<Renderer>().material.SetVector("_Position", collision.contacts[0].point);
            time = 0;
            count = true;
        }
    }
}
