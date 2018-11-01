using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutodestructableBehaviour : MonoBehaviour {

    private void Start() {
        Destroy(gameObject);
    }

}
