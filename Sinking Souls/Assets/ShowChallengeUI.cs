using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowChallengeUI : MonoBehaviour {

    public GameObject InAnim;
    public GameObject OutAnim;
    GameObject current;

	// Use this for initialization
	void Start () {
        current = Instantiate(InAnim, GameController.instance.player.transform, false);
        current.transform.localPosition = Vector3.up * 3.5f;
        current.transform.localScale /= 5;
        StartCoroutine(endMessage());
	}
	
    IEnumerator endMessage()
    {
        yield return new WaitForSecondsRealtime(5.5f);
        Destroy(current);
        current = Instantiate(OutAnim, GameController.instance.player.transform, false);
        current.transform.localPosition = Vector3.up * 3.5f;
        current.transform.localScale /= 5;
        Destroy(current, 2);
    }
    // Update is called once per frame
    void Update () {
		if(current != null)
        {
            current.transform.forward = new Vector3((GameObject.Find("Game Camera").transform.position - current.transform.position).x, (GameObject.Find("Game Camera").transform.position - current.transform.position).y, (GameObject.Find("Game Camera").transform.position - current.transform.position).z);
        }
	}
}
