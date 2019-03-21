using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Modifiers/Fire(I)")]
public class FireModifier : Modifier {

    public GameObject particlesPrefab;
    private GameObject particlesObject;

    public override void Apply(GameObject go) {
        if (go.GetComponent<Entity>().CurrentModifierState[Entity.ModifierState.FIRE]<4) {
            GameController.instance.StartCoroutine(ApplyFire(duration, go));
            InstantiateParticles(go);
            go.GetComponent<Entity>().CurrentModifierState[Entity.ModifierState.FIRE] += 1;

        }
    }

    private void InstantiateParticles(GameObject go) {
        if (go.GetComponent<Entity>().CurrentModifierState[Entity.ModifierState.FIRE] == 0) {
            particlesObject = Instantiate(particlesPrefab);
            particlesObject.transform.position = go.transform.position + new Vector3(0,1,0);
            particlesObject.transform.parent = go.transform;
        }
    }

	IEnumerator ApplyFire(float time, GameObject go) {
        yield return new WaitForSeconds(hitTime);

        try
        {
            go.GetComponent<Entity>().ApplyDamage(damage);
            particlesObject.GetComponent<ParticleSystem>().Play();

            if (time > 0) GameController.instance.StartCoroutine(ApplyFire(time - hitTime, go));
            else {
                if (go.GetComponent<Entity>().CurrentModifierState[Entity.ModifierState.FIRE] == 1) {
                    Destroy(particlesObject.gameObject);
                }
                go.GetComponent<Entity>().CurrentModifierState[Entity.ModifierState.FIRE] -= 1;
            }
        }
        catch(Exception)
        {
            yield break;
        }
        
    }
}
