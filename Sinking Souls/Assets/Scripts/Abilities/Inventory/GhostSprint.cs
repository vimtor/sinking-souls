using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Ghost Sprint")]
public class GhostSprint : Ability {

    [Header("Specific Properties")]
    public float newSpeed;
    public float orignalSpeed;
    public float godModeLasting = 2;
    public bool active = false;
    private bool thrown = false;

    public override void Passive(GameObject go) {
        go.GetComponent<Player>().MovementSpeed = newSpeed;
        if (active && go.GetComponent<Entity>().Weapon.hitting == true) {
            GameController.instance.godMode = false;
            GameController.instance.player.transform.GetChild(1).GetComponent<Renderer>().material.color = GameController.instance.player.GetComponent<Entity>().originalColor;
            newSpeed = orignalSpeed;
            active = thrown = false;
            Debug.Log("Returned to normal attack");
        }
    }

    public override void Activate() {
        if (!thrown) {
            active = true;
            thrown = true;
            GameController.instance.godMode = true;
            orignalSpeed = newSpeed;
            newSpeed = newSpeed * 2;
            GameController.instance.player.transform.GetChild(1).GetComponent<Renderer>().material.color = Color.white;
            GameController.instance.StartCoroutine(ReturnToNormal());
        }
    }

    IEnumerator ReturnToNormal() {
        yield return new WaitForSeconds(godModeLasting);
        newSpeed = orignalSpeed;
        GameController.instance.godMode = false;
        GameController.instance.player.transform.GetChild(1).GetComponent<Renderer>().material.color = GameController.instance.player.GetComponent<Entity>().originalColor;
        active = thrown = false;
        Debug.Log("Returned to normal");
    }

    protected override void Configure(GameObject player) {}
}
