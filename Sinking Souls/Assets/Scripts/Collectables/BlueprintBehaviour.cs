using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintBehaviour : MonoBehaviour {

    public Modifier modifier;
    public float floatOffset;
    [Range(0.0f, 0.1f)] public float floatingSpeed;
    [Range(0.0f, 0.1f)] public float rotationSpeed;

    private Vector3 originalPos;
    private int direction = 1;


	void Start () {
        originalPos = transform.position;
	}

	void Update () {
        if (transform.position.y >= originalPos.y + floatOffset || transform.position.y <= originalPos.y - floatOffset) direction *= -1;
        transform.position += new Vector3(0, floatingSpeed * direction, 0 ) ;
	}

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            if(!GameController.instance.pickedModifiers.Contains(modifier)) GameController.instance.pickedModifiers.Add(modifier);

            Destroy(gameObject);
        }
    }

}
