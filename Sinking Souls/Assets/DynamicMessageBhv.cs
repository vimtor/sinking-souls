using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DynamicMessageBhv : MonoBehaviour {

    Image background;
    Image picture;
    TextMeshProUGUI nameText;
    TextMeshProUGUI text;
    [HideInInspector] public float speed;
    bool apear;
    bool desapear;
    [HideInInspector] public bool onScreen = true;

    // Use this for initialization
    void Start () {
        background = GetComponent<Image>();
        picture = transform.GetChild(0).gameObject.GetComponent<Image>();
        nameText = transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        text = transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();

        background.color = new Color(background.color.r, background.color.g, background.color.b, 0);
        picture.color = new Color(picture.color.r, picture.color.g, picture.color.b, 0);
        nameText.color = new Color(nameText.color.r, nameText.color.g, nameText.color.b, 0);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
	}
	
	// Update is called once per frame
	void Update () {
        if (apear){
            if (background.color.a < 1) {
                background.color = new Color(background.color.r, background.color.g, background.color.b, background.color.a + speed * Time.deltaTime);
                picture.color = new Color(picture.color.r, picture.color.g, picture.color.b, picture.color.a + speed * Time.deltaTime);
                nameText.color = new Color(nameText.color.r, nameText.color.g, nameText.color.b, nameText.color.a + speed * Time.deltaTime);
                text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + speed * Time.deltaTime);
            }
            else {
                apear = false;
            }
        }
        if (desapear) {
            if (background.color.a > 0) {
                background.color = new Color(background.color.r, background.color.g, background.color.b, background.color.a - speed * Time.deltaTime);
                picture.color = new Color(picture.color.r, picture.color.g, picture.color.b, picture.color.a - speed * Time.deltaTime);
                nameText.color = new Color(nameText.color.r, nameText.color.g, nameText.color.b, nameText.color.a - speed * Time.deltaTime);
                text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - speed * Time.deltaTime);
            }
            else {
                desapear = false;
                Destroy(gameObject);
            }
        }

        if(onScreen)
        GameObject.Find("MessageController").GetComponent<MessageController>().messageAlphaHolder = GetComponent<Image>().color.a;
    }

    public void display(Sprite face, string name, string message, float _speed) {
        apear = true;
        desapear = false;

        transform.GetChild(0).GetComponent<Image>().sprite = face;//face
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = name;//name
        transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = message; //message
        speed = _speed;
    }

    public void destroy() {
        apear = false;
        desapear = true;
    }
}
