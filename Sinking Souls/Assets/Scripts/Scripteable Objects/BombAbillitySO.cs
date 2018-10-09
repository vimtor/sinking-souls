using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BombSO", menuName = "BombAbility")]
public class BombAbillitySO : AbilitySO {
	// Use this for initialization
	void Start () {
		
	}
	
	void Update () {
		
	}

    public override void Use(GameObject parent) {
        Player pScript = parent.GetComponent<Player>();
        if (!pScript.thrown) { 
            GameObject bomb = Instantiate(prefab);
            bomb.GetComponent<Transform>().position = pScript.hand.transform.position;
            bomb.GetComponent<Rigidbody>().AddForce((parent.GetComponent<Transform>().forward * 50));
            bomb.AddComponent<AbilityHolder>().holder = this;
            if (parent.gameObject.tag == "Player") bomb.GetComponent<AbilityHolder>().holder.targget = "Enemy";
            else bomb.GetComponent<AbilityHolder>().holder.targget = "Player";
            pScript.thrown = true;
        }
    }
}
