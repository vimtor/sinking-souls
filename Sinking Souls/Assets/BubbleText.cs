using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BubbleText : MonoBehaviour {
    public float activationDistance;
    public float fadeDistance;

    public GameObject image;
    public GameObject text;
    public int maxCharacterSize;
    public List<string> texts;
    public Queue<int> pickedTexts = new Queue<int>();

    private string displayText;

	// Use this for initialization
	void Start () {
        displayText = texts[0];
        pickedTexts.Enqueue(0);
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.GetComponent<RectTransform>().forward = -1 * new Vector3((GameObject.Find("Game Camera").transform.position - transform.position).x, (GameObject.Find("Game Camera").transform.position - transform.position).y, (GameObject.Find("Game Camera").transform.position - transform.position).z);

        float transparency = GameController.instance.player.GetComponent<Player>().map(Vector3.Distance(GameController.instance.player.transform.position, gameObject.GetComponent<RectTransform>().position), activationDistance, fadeDistance, 1, 0);
        int i = 0 ;
        if (transparency <= 0) {
            do {
                i = Random.Range(0, texts.Count);
                displayText = texts[Random.Range(0, texts.Count)];
            } while(pickedTexts.Contains(i));

        }
        else {
            if (!pickedTexts.Contains(i)) {
                pickedTexts.Enqueue(i);
                if (pickedTexts.Count > texts.Count / 2) pickedTexts.Dequeue();
            }
        }    
        image.GetComponent<Image>().color = new Color(image.GetComponent<Image>().color.r, image.GetComponent<Image>().color.g, image.GetComponent<Image>().color.b, transparency);
        text.GetComponent<TextMeshProUGUI>().color = new Color(text.GetComponent<TextMeshProUGUI>().color.r, text.GetComponent<TextMeshProUGUI>().color.g, text.GetComponent<TextMeshProUGUI>().color.b, transparency * 1.1f);
        text.GetComponent<TextMeshProUGUI>().text = displayText;
    }
}
