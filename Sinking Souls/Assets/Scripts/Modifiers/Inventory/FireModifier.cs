using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Modifiers/Fire(I)")]
public class FireModifier : Modifier {

    public GameObject particlesPrefab;
    private GameObject particlesObject;

    public override void Apply(GameObject go) {
        if (go.tag == "Enemy" && go.GetComponent<Enemy>().currentModifierState[Entity.ModifierState.FIRE]<4) {
            GameController.instance.StartCoroutine(ApplyFire(duration, go));
            InstantiateParticles(go);
            go.GetComponent<Enemy>().currentModifierState[Entity.ModifierState.FIRE] += 1;

        }
        Debug.Log(go.GetComponent<Enemy>().currentModifierState[Entity.ModifierState.FIRE]);
    }

    private void InstantiateParticles(GameObject go) {
        if (go.GetComponent<Enemy>().currentModifierState[Entity.ModifierState.FIRE] == 0) {
            particlesObject = Instantiate(particlesPrefab);
            particlesObject.transform.position = go.transform.position + new Vector3(0,1,0);
            particlesObject.transform.parent = go.transform;
        }
    }

	IEnumerator ApplyFire(float time, GameObject go) {
        yield return new WaitForSeconds(hitTime);
        go.GetComponent<Enemy>().TakeDamage(damage);
        particlesObject.GetComponent<ParticleSystem>().Play();

        if (time > 0) GameController.instance.StartCoroutine(ApplyFire(time - hitTime, go));
        else {
            if (go.GetComponent<Enemy>().currentModifierState[Entity.ModifierState.FIRE] == 1) {
                GameObject.Destroy(particlesObject.gameObject);
            }
            go.GetComponent<Enemy>().currentModifierState[Entity.ModifierState.FIRE] -= 1;
        }
    }
}
