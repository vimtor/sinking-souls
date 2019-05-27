using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ChestIconRescaler : MonoBehaviour {

    public float TritonIslandScale;
    public float DeathIslandScale;

    public bool TritonIsland;


    // Use this for initialization
    void Start()
    {
        TritonIsland = true;
        if (GameObject.Find("Post Processing").gameObject.GetComponent<PostProcessVolume>().profile.name == "Dead Island Profile") TritonIsland = false;
    }

    // Update is called once per frame
    void Update () {

        float scale;
		if(TritonIsland)
        {
            scale = TritonIslandScale;
            scale *= (transform.localScale.magnitude / transform.lossyScale.magnitude);
            gameObject.transform.localScale = new Vector3(scale, scale, scale);
        }
        else
        {
            scale = DeathIslandScale;
            scale *= (transform.localScale.magnitude / transform.lossyScale.magnitude);
            gameObject.transform.localScale = new Vector3(scale, scale, scale);
        }
	}
}
