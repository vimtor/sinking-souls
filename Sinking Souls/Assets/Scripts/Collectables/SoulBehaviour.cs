using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulBehaviour : MonoBehaviour {

    [HideInInspector]
    public int value;

    [HideInInspector]
    public GameObject player;

    [HideInInspector]
    public string soulColor;

    private void Start () {
        player = GameController.instance.player;
        soulColor = GetComponent<Renderer>().material.name;
	}
	
	private void Update () {
        float speed = Vector3.Magnitude(player.transform.position - transform.position);
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position + new Vector3(0,1,0), speed * 0.05f);
	}

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {

            Destroy(gameObject);

            switch (soulColor) {

                case "BlueSoul" + " (Instance)":
                    GameController.instance.blueSouls++;
                    break;

                case "RedSoul" + " (Instance)":
                    GameController.instance.redSouls++;
                    break;

                case "GreenSoul" + " (Instance)":
                    GameController.instance.greenSouls++;
                    break;

                default:
                    Debug.LogError("Material name not considered. Check if the material is new or wrong");
                    break;

            }
        }
    }
}
