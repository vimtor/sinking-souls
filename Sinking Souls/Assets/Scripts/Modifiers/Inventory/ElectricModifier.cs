using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Modifiers/Electric(I)")]
public class ElectricModifier : Modifier {

    public GameObject particlesPrefab;
    private GameObject particlesObject;
    public float stuntProv = 20;

    public override void Apply(GameObject go) {
        if (go.tag == "Enemy") {
            go.GetComponent<Enemy>().TakeDamage(damage);
            if (Random.value < stuntProv/100 && go.GetComponent<Enemy>().currentModifierState[Entity.ModifierState.ELECTRIC] == 0) {
                Debug.Log("Stuned moderfucker");
                GameController.instance.StartCoroutine(StopStun(go));
                go.GetComponent<Enemy>().currentModifierState[Entity.ModifierState.ELECTRIC] += 1;
                go.GetComponent<AIController>().externalTime = duration;
                go.GetComponent<AIController>().forceState = true;
            }
        }
    }

    IEnumerator StopStun(GameObject go) {
        yield return new WaitForSeconds(duration);
        go.GetComponent<Enemy>().currentModifierState[Entity.ModifierState.ELECTRIC] = 0;
        Debug.Log("Unestuned");
    }

    private void InstantiateParticles(GameObject go) {
        particlesObject = Instantiate(particlesPrefab);
        particlesObject.transform.position = go.transform.position + new Vector3(0, 1, 0);
        particlesObject.transform.parent = go.transform;
    }
}
