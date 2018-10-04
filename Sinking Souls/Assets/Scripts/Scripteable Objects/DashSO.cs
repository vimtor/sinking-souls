using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Ability", menuName = "Dash/NormalDash")]
public class DashSO : AbilitySO {
    public float dashSpeed;

    public override void Use(GameObject player) {
        if (!player.GetComponent<Animator>().GetBool("DASHED")) {
            player.GetComponent<Rigidbody>().AddForce(player.GetComponent<Transform>().forward.normalized * dashSpeed, ForceMode.Impulse);
            //MonoB.instance.StartCoroutine(StopDash(0.05f, player));
            player.GetComponent<Animator>().SetBool("DASHED", true);
        }
    }
    public IEnumerator StopDash(float time, GameObject player) {
        Debug.Log("before uield return");
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        yield return new WaitForSeconds(time);
        Debug.Log("uield return");
    }

}
