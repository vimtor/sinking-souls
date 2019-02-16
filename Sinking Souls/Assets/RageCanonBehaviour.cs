using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageCanonBehaviour : MonoBehaviour {

    
    public int restartTime;
    public float duration;
    public float RotationSpeed;
    private Vector3 center;
    private Vector3 playerVelocity;
    public float initialDegrees;
    private float direction;
    private Vector3 initial;
    public GameObject canon;
    [HideInInspector]public GameObject position;
    private GameObject instantiatedCanon;
    private bool start = false;

    private float time = 0;

	// Use this for initialization
	void Start () {
        direction = -1;
        center = new Vector3(GameController.instance.currentRoom.transform.position.x, transform.position.y, GameController.instance.currentRoom.transform.position.z);
        playerVelocity = GameController.instance.player.GetComponent<Rigidbody>().velocity;
        direction = Vector3.SignedAngle((GameController.instance.player.transform.position - transform.position), ((GameController.instance.player.transform.position + playerVelocity) - transform.position),new Vector3(0,1,0));
        direction = Mathf.Sign(direction);

        initial = center - transform.position;
        initial = Quaternion.Euler(new Vector3(0, initialDegrees * -direction, 0)) * initial;



    }

    // Update is called once per frame
    void Update () {
        if (!start) {
            GetComponent<AbilityHolder>().owner.transform.rotation = Quaternion.Lerp(GetComponent<AbilityHolder>().owner.transform.rotation, Quaternion.LookRotation(initial), Time.deltaTime * RotationSpeed);
        }
        if(!start && GetComponent<AbilityHolder>().owner.transform.rotation == Quaternion.LookRotation(initial)) {
            start = true;
            instantiatedCanon = Instantiate(canon);

            instantiatedCanon.AddComponent<AbilityHolder>().owner = GetComponent<AbilityHolder>().owner;
            instantiatedCanon.GetComponent<AbilityHolder>().holder = GetComponent<AbilityHolder>().holder;
            instantiatedCanon.transform.position = transform.position;
            instantiatedCanon.transform.rotation = Quaternion.LookRotation(initial);
            Destroy(instantiatedCanon, duration);
        }

        if ( start && instantiatedCanon != null) {
            GetComponent<AbilityHolder>().owner.transform.rotation = Quaternion.Euler(new Vector3(0, ((initialDegrees*2)/duration * Time.deltaTime) * direction, 0)) * instantiatedCanon.transform.rotation;
        }

        time += Time.deltaTime;
        Debug.DrawRay(gameObject.transform.position, initial * 100, Color.blue);
    }
}
