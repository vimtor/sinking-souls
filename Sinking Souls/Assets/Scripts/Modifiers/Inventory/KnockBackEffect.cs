using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/KnockBack")]
public class KnockBackEffect : Effect {

    public override void Apply(GameObject other) {
        other.GetComponent<Rigidbody>().AddForce((-other.transform.forward) * strenght, ForceMode.Impulse);
    }
}
