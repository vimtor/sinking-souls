using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Modifiers/Toxic(I)")]
public class ToxicModifier : Modifier {

    public GameObject particlesPrefab;
    private GameObject particlesObject;

    public override void Apply(GameObject go) {
        if (go.tag == "Enemy" && go.GetComponent<Enemy>().CurrentModifierState[Entity.ModifierState.TOXIC] < 1) {
            GameController.instance.StartCoroutine(ApplyToxic(go));
            go.GetComponent<Enemy>().CurrentModifierState[Entity.ModifierState.TOXIC] += 1;
            go.AddComponent<tryChangeColor>();

        }
    }

    private void InstantiateParticles(GameObject go) {
        particlesObject = Instantiate(particlesPrefab);
        particlesObject.transform.position = go.transform.position + new Vector3(0, 1, 0);
        particlesObject.transform.parent = go.transform;
    }

    IEnumerator ApplyToxic(GameObject go) {
        yield return new WaitForSeconds(hitTime);
        go.GetComponent<Enemy>().ApplyDamage(damage);
        

        GameController.instance.StartCoroutine(ApplyToxic(go));
    }
}