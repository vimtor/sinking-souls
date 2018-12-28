using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Modifiers/Ice(I)")]
public class IceModifier : Modifier {

    private float originalSpeed;
    public float reduction;

    public override void Apply(GameObject go) {
        if(go.tag == "Enemy" && go.GetComponent<Enemy>().CurrentModifierState[Entity.ModifierState.ICE] == 0) originalSpeed = go.GetComponent<AIController>().navMeshAgent.speed;
        if (go.tag == "Enemy" && go.GetComponent<Enemy>().CurrentModifierState[Entity.ModifierState.ICE] < 4) {
            float speed = go.GetComponent<AIController>().navMeshAgent.speed;
            speed = speed * reduction;
            go.GetComponent<AIController>().navMeshAgent.speed = speed;
            GameController.instance.StartCoroutine(MeltIce(duration, originalSpeed, go));
            go.GetComponent<Enemy>().CurrentModifierState[Entity.ModifierState.ICE] += 1;
            Debug.Log(go.GetComponent<AIController>().navMeshAgent.speed);
        }

    }

    IEnumerator MeltIce(float time, float _speed, GameObject go) {
        yield return new WaitForSeconds(time);
        Debug.Log("Returned");
        go.GetComponent<AIController>().navMeshAgent.speed = _speed;
        go.GetComponent<Enemy>().CurrentModifierState[Entity.ModifierState.ICE] -= 1;

    }
}
