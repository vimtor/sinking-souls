using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChestBehaviour : MonoBehaviour {

    public GameObject Button;
    private GameObject canvas;
    private GameObject instantiatedButton;
    public AnimationClip Open;
    private bool isOpened = false;
    public GameObject ChestContentUI;
    private GameObject UIHandler;
    public int SoulsRecived = 50;
    public int SoulsRecivedLight = 10;
    public bool show = false;
    private float apearingSpeed = 10;
    public float messageDuration = 6;
    public bool LightChest = false;

    // Use this for initialization
    void Start () {
        canvas = transform.Find("ChestCanvas").gameObject;
        isOpened = false;
        ChestContentUI = GameController.instance.GetComponent<GameController>().ChestContentUI;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") if (!isOpened) instantiatedButton = Instantiate(Button, canvas.transform, false);
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player") {
            if (!isOpened) {
                if (Input.GetButtonDown("BUTTON_A") || Input.GetKey(KeyCode.E)) {
                    isOpened = true;
                    Destroy(instantiatedButton);
                    GetComponent<Animator>().SetTrigger("Open");

                    StartCoroutine(ShowContent(1.8f));
                    //GiveContent();

                }
            }
        }
    }

    public void Update()
    {
        if (!show)
        {
            UIHandler.GetComponent<Image>().color = new Vector4(UIHandler.GetComponent<Image>().color.r, UIHandler.GetComponent<Image>().color.g, UIHandler.GetComponent<Image>().color.b, UIHandler.GetComponent<Image>().color.a - apearingSpeed * Time.deltaTime);
            UIHandler.GetComponentInChildren<TextMeshProUGUI>().color = new Vector4(UIHandler.GetComponentInChildren<TextMeshProUGUI>().color.r, UIHandler.GetComponentInChildren<TextMeshProUGUI>().color.g, UIHandler.GetComponentInChildren<TextMeshProUGUI>().color.b, UIHandler.GetComponentInChildren<TextMeshProUGUI>().color.a - apearingSpeed * Time.deltaTime);
            if (UIHandler.GetComponent<Image>().color.a <= 0)
            {
                UIHandler.GetComponent<Image>().color = new Vector4(UIHandler.GetComponent<Image>().color.r, UIHandler.GetComponent<Image>().color.g, UIHandler.GetComponent<Image>().color.b, 0);
                UIHandler.GetComponentInChildren<TextMeshPro>().color = new Vector4(UIHandler.GetComponentInChildren<TextMeshPro>().color.r, UIHandler.GetComponentInChildren<TextMeshPro>().color.g, UIHandler.GetComponentInChildren<TextMeshPro>().color.b, 0);
            }
        }
        else
        {
            UIHandler.GetComponent<Image>().color = new Vector4(UIHandler.GetComponent<Image>().color.r, UIHandler.GetComponent<Image>().color.g, UIHandler.GetComponent<Image>().color.b, UIHandler.GetComponent<Image>().color.a + apearingSpeed * Time.deltaTime);
            UIHandler.GetComponentInChildren<TextMeshProUGUI>().color = new Vector4(UIHandler.GetComponentInChildren<TextMeshProUGUI>().color.r, UIHandler.GetComponentInChildren<TextMeshProUGUI>().color.g, UIHandler.GetComponentInChildren<TextMeshProUGUI>().color.b, UIHandler.GetComponentInChildren<TextMeshProUGUI>().color.a + apearingSpeed * Time.deltaTime);

            if (UIHandler.GetComponent<Image>().color.a >= 1)
            {
                UIHandler.GetComponent<Image>().color = new Vector4(UIHandler.GetComponent<Image>().color.r, UIHandler.GetComponent<Image>().color.g, UIHandler.GetComponent<Image>().color.b, 1);
                UIHandler.GetComponentInChildren<TextMeshProUGUI>().color = new Vector4(UIHandler.GetComponentInChildren<TextMeshProUGUI>().color.r, UIHandler.GetComponentInChildren<TextMeshProUGUI>().color.g, UIHandler.GetComponentInChildren<TextMeshProUGUI>().color.b, 1);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player") if(instantiatedButton != null) instantiatedButton.GetComponent<popUpEffect>().destroy();
    }
    
 
    public void GiveContent() {
        int rand = Random.Range(0, 2);
        UIHandler = Instantiate(ChestContentUI, GameObject.Find("Canvas").transform, false);
        ShowChest();
        StartCoroutine(HideChest(messageDuration));

        if (!LightChest) {
            if (rand == 0) {// give a modifier
                foreach (Modifier mod in GameController.instance.modifiers) {
                    if (!mod.owned) {
                        UIHandler.GetComponentInChildren<TextMeshProUGUI>().text = "<color=#ffa500ff><size=40>" + mod.name + "</size></color> unlocked";
                        //Debug.Log("You Got: " + mod.name);
                        mod.owned = true;
                        return;
                    }
                }
            }
            //give a ability
            foreach (Ability ab in GameController.instance.abilities) {
                if (!ab.owned) {
                    //Debug.Log("You Got: " + ab.name);
                    UIHandler.GetComponentInChildren<TextMeshProUGUI>().text = "<color=#ffa500ff><size=40>" + ab.name + "</size></color> unlocked";
                    ab.owned = true;
                    return;
                }
            }
            if (rand == 1) {
                foreach (Modifier mod in GameController.instance.modifiers) {
                    if (!mod.owned) {
                        UIHandler.GetComponentInChildren<TextMeshProUGUI>().text = "<color=#ffa500ff><size=40>" + mod.name + "</size></color> unlocked";
                        mod.owned = true;
                        return;
                    }
                }
            }
            //if all unlocked give 50 souls
            UIHandler.GetComponentInChildren<TextMeshProUGUI>().text = "<color=#00ff48><size=40>" + SoulsRecived + "</size></color> souls added";

            GameController.instance.AddSouls(SoulsRecived);
        }
        else {
            //if all unlocked give 50 souls
            UIHandler.GetComponentInChildren<TextMeshProUGUI>().text = "<color=#00ff48><size=40>" + SoulsRecivedLight + "</size></color> souls added";

            GameController.instance.AddSouls(SoulsRecivedLight);
        }

       
    }

    public void ShowChest()
    {
        show = true;
    }

    IEnumerator HideChest(float messageDuration)
    {
        yield return new WaitForSecondsRealtime(messageDuration);

        show = false;
        Destroy(UIHandler, 3f);

    }

    IEnumerator ShowContent(float t)
    {
        yield return new WaitForSeconds(t);

        GiveContent();
    }

}
