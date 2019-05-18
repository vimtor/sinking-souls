using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerIconRescaler : MonoBehaviour {

    public float DeathIslandScale;

	// Use this for initialization
	void Start () {
        DeathIslandScale *= (transform.localScale.magnitude / transform.lossyScale.magnitude);

        if(GameObject.Find("Post Processing").gameObject.GetComponent<PostProcessVolume>().profile.name == "Dead Island Profile") gameObject.transform.localScale = new Vector3(DeathIslandScale, 1, DeathIslandScale);
	}

}
