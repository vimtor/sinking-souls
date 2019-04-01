using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BubbleText : MonoBehaviour {


    [Header("Prefabs:")]
    public GameObject image;
    public GameObject text;
    public List<string> texts;

    [Header("Properties:")]
    public float activationDistance;
    public float fadeDistance;
    public int maxCharacterSize;

    private Queue<int> pickedTexts = new Queue<int>();
    private int index;
    private string displayText;

    ///------------- Use this for Testing ---------------
        //public List<int> aux = new List<int>();


    void Start () {
        // Initialization
        displayText = texts[0];
        pickedTexts.Enqueue(0);
        index = 0;
    }
	

	void Update () {

        //Looks at the camera
        gameObject.GetComponent<RectTransform>().forward = -1 * new Vector3((GameObject.Find("Game Camera").transform.position - transform.position).x, (GameObject.Find("Game Camera").transform.position - transform.position).y, (GameObject.Find("Game Camera").transform.position - transform.position).z);

        //Adjust transparency
        float transparency = GameController.instance.player.GetComponent<Player>().map(Vector3.Distance(GameController.instance.player.transform.position, gameObject.GetComponent<RectTransform>().position), activationDistance, fadeDistance, 1, 0);

        //Change Text       
        if (transparency <= 0) {
            //Select new valid text
            if(pickedTexts.Count < texts.Count)
                while (pickedTexts.Contains(index))
                {
                    index = Random.Range(0, texts.Count);
                    displayText = texts[index];
                } 

        }
        else {
            //Update used texts queue
            if (!pickedTexts.Contains(index)) {
                pickedTexts.Enqueue(index);
                if (pickedTexts.Count > (texts.Count / 2)) pickedTexts.Dequeue();
            }
        }

        ///------------- Use this for Testing ---------------
            //aux = new List<int>();
            //for (int i = 0; i < texts.Count; i++)
            //{
            //    if (pickedTexts.Contains(i)) aux.Add(i);
            //}


        //Checks text size 
        text.GetComponent<TextMeshProUGUI>().fontSizeMax = maxCharacterSize;

        //Display text
        image.GetComponent<Image>().color = new Color(image.GetComponent<Image>().color.r, image.GetComponent<Image>().color.g, image.GetComponent<Image>().color.b, transparency);
        text.GetComponent<TextMeshProUGUI>().color = new Color(text.GetComponent<TextMeshProUGUI>().color.r, text.GetComponent<TextMeshProUGUI>().color.g, text.GetComponent<TextMeshProUGUI>().color.b, transparency * 1.1f);
        text.GetComponent<TextMeshProUGUI>().text = displayText;
    }
}
