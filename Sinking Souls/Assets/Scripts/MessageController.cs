using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageController : MonoBehaviour {

    public List<MessageCondition> conditions;
    public List<GameObject> overlayingObjects;
    private GameObject canvas;
    public GameObject message;
    public GameObject dPad;
    private GameObject currentMessage;
    private GameObject currentDPad;
    public float dpadAlphaHolder = 0;
    public float messageAlphaHolder = 0;
    public float displaySpeed;
    public float dPadSpeed;
    public float dPadDuration;
    private MessageCondition waitingCondition;
    private MessageCondition currentCondition; // if weird things happen this can be the problem not restarting to null everiwhere
    private GameObject genericDialogue;
    public GameObject cinematic;
    public GameObject LevelInfo;

    void Start () {
        foreach (MessageCondition condition in conditions) {
            condition.reStart(true);
        }
        canvas = GameObject.Find("Canvas");
    }
    float messageTimer = 0;
    float dPadTimer = 0;
    float duration = 0;

    bool activeOverlayingObject() {
        foreach(GameObject g in overlayingObjects) {
            if (g.activeInHierarchy) return true;
        }
        return false;
    }

    bool checkCinematic() {
        if (cinematic != null) return cinematic.activeInHierarchy;
        return false;
    }

    bool checkLevelInfo() {
        return (LevelInfo != null);
    }

    private bool deactivatedMessage = false;
        // Update is called once per frame
    void Update () {
        if(LevelInfo == null)LevelInfo = GameObject.Find("LevelInfo(Clone)");
        if (!activeOverlayingObject() ) {

            if (checkLevelInfo()) {
                LevelInfo.SetActive(true);
            }
            else {



                if (currentDPad != null) {
                    currentDPad.GetComponent<dPadUI>().onScreen = true;
                    currentDPad.GetComponent<Image>().color = new Color(1, 1, 1, dpadAlphaHolder);
                }
                if (currentMessage != null) {
                    currentMessage.GetComponent<DynamicMessageBhv>().onScreen = true;

                    currentMessage.GetComponent<Image>().color = new Color(1, 1, 1, messageAlphaHolder);
                    currentMessage.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, messageAlphaHolder);
                    currentMessage.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, messageAlphaHolder);
                    currentMessage.transform.GetChild(2).GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, messageAlphaHolder);
                }
                if (deactivatedMessage) {
                    genericDialogue.SetActive(true);
                    deactivatedMessage = false;
                }

                if (currentDPad == null && currentMessage == null) {
                    foreach (MessageCondition condition in conditions) {
                        DisplayMessage(condition);
                    }
                }

                if (currentMessage != null) {

                    if (messageTimer >= duration || Input.GetKey(KeyCode.UpArrow) || InputManager.Dpad.y > 0 || InputManager.ButtonA) {
                        messageTimer = -Time.deltaTime;
                        currentMessage.GetComponent<DynamicMessageBhv>().destroy();
                        currentMessage = null;
                        currentCondition.reStart(false);
                    }

                    messageTimer += Time.deltaTime;
                }

                if (currentDPad != null) {
                    if (dPadTimer >= dPadDuration) {
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

        }
        else {
            if (checkLevelInfo()) {
                LevelInfo.SetActive(false);
            }
            if (currentDPad != null) {
                currentDPad.GetComponent<dPadUI>().onScreen = false;
                currentDPad.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            }
            if (currentMessage != null) {
                currentMessage.GetComponent<DynamicMessageBhv>().onScreen = false;

                currentMessage.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                currentMessage.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
                currentMessage.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 0);
                currentMessage.transform.GetChild(2).GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 0);
            }

            if (GameObject.Find("Generic Dialogue") && !checkCinematic()) {
                genericDialogue = GameObject.Find("Generic Dialogue");
                genericDialogue.SetActive(false);
                deactivatedMessage = true;
            }
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
