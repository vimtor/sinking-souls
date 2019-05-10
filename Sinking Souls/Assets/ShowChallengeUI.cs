using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowChallengeUI : MonoBehaviour {

    public GameObject InAnim;
    public GameObject OutAnim;
    GameObject current;

    // Use this for initialization
    //private void Start() {
    //    Show("Test to string!");
    //}

    public void Show (string name) {
        current = Instantiate(InAnim, GameController.instance.player.transform, false);
        current.transform.localPosition = Vector3.up * 3.5f;
        current.transform.localScale /= 5;
        StartCoroutine(ShowText());
        StartCoroutine(endMessage());
        current.transform.Find("Name").gameObject.GetComponent<TextMeshPro>().text = "<color=#ffa500ff>" + name + "</color>";

    }
	IEnumerator ShowText() {
        yield return new WaitForSecondsRealtime(1.5f);
        current.transform.Find("Name").gameObject.GetComponent<NameDisplay>().Show();
    }
    IEnumerator endMessage()
    {
        yield return new WaitForSecondsRealtime(5.5f);
        Quaternion lastRot = current.transform.rotation;
        Destroy(current);
        current = Instantiate(OutAnim, GameController.instance.player.transform, false);
        current.transform.localPosition = Vector3.up * 3.5f;
        current.transform.localScale /= 5;
        current.transform.rotation = lastRot;
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
