using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BombSO", menuName = "BombAbility")]
public class BombAbillitySO : AbilitySO {

    public override void Use(GameObject parent) {
        Player playerScript = parent.GetComponent<Player>();
        if (!playerScript.thrown) { 
            GameObject bomb = Instantiate(prefab);
            bomb.GetComponent<Transform>().position = playerScript.hand.transform.position;
            bomb.GetComponent<Rigidbody>().AddForce((parent.GetComponent<Transform>().forward * 50));
            bomb.AddComponent<AbilityHolder>().holder = this;

            if (parent.gameObject.tag == "Player") bomb.GetComponent<AbilityHolder>().holder.target = "Enemy";
            else bomb.GetComponent<AbilityHolder>().holder.target = "Player";

            playerScript.thrown = true;
        }
    }
}
