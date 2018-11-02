using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {

    public GameObject shopUI;
    public GameObject UIItem;
    public GameObject content;


    public void FillShop() {
        foreach( Modifier modifier in GameController.instance.modifiers) {
            if (modifier.unlocked) {
                GameObject item = Instantiate(UIItem);
                item.transform.Find("Icon").GetComponent<Image>().sprite = modifier.sprite;
                item.transform.Find("Price").GetComponent<Text>().text = modifier.price.ToString();
                item.transform.Find("Name").GetComponent<Text>().text = modifier.name;
                item.gameObject.transform.SetParent(content.transform, false);
            }
        }

    }

    private void OnTriggerStay(Collider other) {
        if (GameController.instance.blacksmith) {
            if (!shopUI.activeSelf) {
                if (InputHandler.ButtonA()) {
                    Debug.Log("activating");
                    shopUI.SetActive(true);
                    GameController.instance.player.GetComponent<Player>().state = Player.State.IDLE;
                    GameController.instance.player.GetComponent<Player>().HandleInput();
                    GameController.instance.player.GetComponent<Player>().move = false;
                }
            }
            else {
                if (InputHandler.ButtonB()) {
                    Debug.Log("de-activating");
                    shopUI.SetActive(false);
                    GameController.instance.player.GetComponent<Player>().move = true;
                    if (InputHandler.ButtonA()) Debug.Log("A");
                }
            }
        }
    }
}
