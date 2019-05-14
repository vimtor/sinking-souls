using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullBehaviour : MonoBehaviour {
    public float speed;
    public float lerp;
    public GameObject particles;
    public float particlesLife;
    public float force;
    private float scale;
    private float timeS;
    public float deathTime;
    private float counter = 0;
    // Use this for initialization
    void Start () {
        transform.position = GetComponent<AbilityHolder>().owner.GetComponent<Enemy>().WeaponHand.transform.position;
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * force, ForceMode.Impulse);
        scale = transform.localScale.x;
        transform.localScale = Vector3.zero;
        timeS = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(GetComponent<AbilityHolder>().owner.transform.position, GameController.instance.player.transform.position) > Vector3.Distance(transform.position, GetComponent<AbilityHolder>().owner.transform.position) * 0.8f) lerp = 0.01f;
        GetComponent<Rigidbody>().velocity = Vector3.Lerp(GetComponent<Rigidbody>().velocity, (((GameController.instance.player.transform.position + (Vector3.up)*1.5f) - transform.position).normalized * speed) + new Vector3(0, (Mathf.Sin(Time.time * 5) * 6f), 0), lerp);
        
        if (transform.localScale.x < scale) {
            transform.localScale += new Vector3(1,1,1) * 10 * Time.deltaTime;
        }
        else {
            transform.forward = GameController.instance.player.transform.position + Vector3.up * 2 - transform.position;
        }
        if(counter >= deathTime) {
            Destroy(gameObject);
            GameObject part = Instantiate(particles);
            part.transform.position = transform.position;
            Destroy(part, 0.6f);
            AudioManager.Instance.PlayEffect("MagicBombExplosion");
        }
        counter += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collision) {

        if(collision.gameObject == GetComponent<AbilityHolder>().owner && counter < 1) return;
        if (collision.gameObject.tag == "Weapon") return;
        Destroy(gameObject);
        GameObject part = Instantiate(particles);
        part.transform.position = transform.position;
        Destroy(part, 0.6f);


        AudioManager.Instance.PlayEffect("MagicBombExplosion");
    }
}
