using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class castedMagicBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    private Vector3 destination;
    private bool go;
    private GameObject parent;

    public float flyingSpeed;
    public float relationAtenuation;
    private Vector2 maxHeightPoint;
    private bool augmented;
    private Vector3 parentPos;

    public void chase(Vector3 pos, GameObject p, bool aug = false)
    {
        destination = pos;
        augmented = aug;
        go = true;
        parent = p;
        parentPos = parent.transform.position;
        maxHeightPoint = new Vector2(((destination - parentPos).magnitude/2f), destination.y + 5);
    }
    // Update is called once per frame
    void Update () {
        if (go)
        {
            GetComponent<Rigidbody>().velocity =
                    (destination - transform.position).normalized *
                    flyingSpeed * ((transform.position - destination).magnitude / relationAtenuation);

            Vector2 b = new Vector2(0, transform.position.y);
            Vector2 end = new Vector2((destination - parentPos).magnitude, destination.y);
            float t = (parentPos - transform.position).magnitude;
            t = GameController.instance.player.GetComponent<Player>().map(t, 0, (destination - parentPos).magnitude, 0, 1);
            float y = ((Mathf.Pow((1 - t), 2) * b) + (2 * (1 - t) * t * maxHeightPoint) + (Mathf.Pow(t, 2) * end)).y;

            transform.position = new Vector3(transform.position.x, y, transform.position.z);

            if(Vector3.Distance(transform.position, destination) < 0.3f)
            {
                if(!augmented)parent.GetComponent<SorcererReviveHelper>().reviveNow(destination);
                Destroy(gameObject);
            }
        }
	}
}
