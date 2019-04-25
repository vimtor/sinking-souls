using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageController : MonoBehaviour {

    public List<MessageCondition> conditions;
    private GameObject canvas;
    public GameObject message;
    public GameObject dPad;
    private GameObject currentMessage;
    private GameObject currentDPad;
    public float displaySpeed;
    public float dPadSpeed;
    public float dPadDuration;
    private MessageCondition waitingCondition;
    private MessageCondition currentCondition; // if weird things happen this can be the problem not restarting to null everiwhere

    void Start () {
        foreach (MessageCondition condition in conditions) {
            condition.reStart(true);
        }
        canvas = GameObject.Find("Canvas");
    }
    float messageTimer = 0;
    float dPadTimer = 0;
    float duration = 0;
        // Update is called once per frame
    void Update () {
        if (currentDPad == null && currentMessage == null) {
            foreach (MessageCondition condition in conditions) {
                DisplayMessage(condition);
            }
        }

        if(currentMessage != null) {

            if (messageTimer >= duration || Input.GetKey(KeyCode.UpArrow) || InputManager.Dpad.y > 0 || InputManager.ButtonA) {
                messageTimer = -Time.deltaTime;
                currentMessage.GetComponent<DynamicMessageBhv>().destroy();
                currentMessage = null;
                currentCondition.reStart(false);
            }

            messageTimer += Time.deltaTime;
        }

        if(currentDPad != null) {
            if(dPadTimer >= dPadDuration ) {
                dPadTimer = -Time.deltaTime;
                currentDPad.GetComponent<dPadUI>().destroy();
                waitingCondition.reStart(false);
                currentDPad = null;
            }
            else {
                if (InputManager.Dpad.y < 0 || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.DownArrow)) {
                    dPadTimer = -Time.deltaTime;
                    currentDPad.GetComponent<dPadUI>().destroy();
                    currentDPad = null;
                    waitingCondition.completed = false;
                    Show(waitingCondition);
                }
            }

            dPadTimer += Time.deltaTime;
        }

	}

    private void DisplayMessage(MessageCondition condition) {
        if (condition.Check()) {
            if (!condition.completed) {
                Show(condition);
            }
            else {
                waitingCondition = condition;
                currentDPad = Instantiate(dPad, canvas.transform, false);
                currentDPad.GetComponent<dPadUI>().apear(dPadSpeed);
            }
        }
    }

    void Show(MessageCondition condition) {
        condition.completed = true;
        currentCondition = condition;
        currentMessage = Instantiate(message, canvas.transform, false);
        duration = condition.message.duration;
        if (InputManager.Xbox_One_Controller > 0) {
            currentMessage.GetComponent<DynamicMessageBhv>().display(
                currentMessage.transform.GetChild(0).GetComponent<Image>().sprite = condition.message.face,//face
                currentMessage.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = condition.message.teller,//name
                currentMessage.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = condition.message.messageController, //message
                displaySpeed
                );
        }
        else {
            currentMessage.GetComponent<DynamicMessageBhv>().display(
                currentMessage.transform.GetChild(0).GetComponent<Image>().sprite = condition.message.face,//face
                currentMessage.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = condition.message.teller,//name
                currentMessage.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = condition.message.messageKeyboard, //message
                displaySpeed
                );
        }
    }

}
