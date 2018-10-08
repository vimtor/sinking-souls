using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Ability", menuName = "Dash/NormalDash")]
public class DashSO : AbilitySO {
    public float dashSpeed;

    public override void Use(GameObject player) {
            player.GetComponent<Rigidbody>().velocity = player.GetComponent<Transform>().forward.normalized * dashSpeed;
    }
}
