using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageController : MonoBehaviour {

    public List<MessageCondition> conditions;
    private GameObject canvas;
    public GameObject message;
    private GameObject currentMessage;
	void Start () {
        foreach (MessageCondition condition in conditions) {
            condition.Start();
        }
        canvas = GameObject.Find("Canvas");
    }
    float messageTimer = 0;
    float duration = 0;
        // Update is called once per frame
    void Update () {
		foreach(MessageCondition condition in conditions) {
            if(!condition.completed)
                if (condition.Check()) {
                    currentMessage = Instantiate(message,canvas.transform,false);
                    duration = condition.message.duration;
                    currentMessage.transform.GetChild(0).GetComponent<Image>().sprite = condition.message.face;//face
                    currentMessage.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = condition.message.teller;//name
                    if (InputManager.Xbox_One_Controller > 0) {
                        currentMessage.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = condition.message.messageController; //message
                    }
                    else {
                        currentMessage.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = condition.message.messageKeyboard; //message

                    }
                }
        }

        if(currentMessage != null) {

            if (messageTimer >= duration) {
                messageTimer = -Time.deltaTime;
                Destroy(currentMessage);
                currentMessage = null;
            }

            messageTimer += Time.deltaTime;
        }
	}
}
