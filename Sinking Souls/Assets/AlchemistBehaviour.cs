using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlchemistBehaviour : MonoBehaviour
{

    public GameObject shopPanel;
    public GameObject UIItem;
    public int range = 5;
    private Vector3 distPlayer;

    private void Start()
    {
        GameController.instance.alchemist = true;
    }

    public void FillShop()
    {
        bool firstSelected = false;
        foreach (Ability ab in GameController.instance.abilities)
        {
            Debug.Log("loading");
            GameObject item = Instantiate(UIItem);
            item.transform.Find("Icon").GetComponent<Image>().sprite = ab.sprite;
            item.transform.Find("Price").GetComponent<Text>().text = ab.price.ToString();
            item.transform.Find("Name").GetComponent<Text>().text = ab.name;
            item.gameObject.transform.SetParent(shopPanel.transform.GetChild(0), false);

            if (!firstSelected)
            {
                GameObject.Find("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem>().firstSelectedGameObject = item;
                firstSelected = true;
            }
        }

    }

    private void Update()
    {
        distPlayer = GameController.instance.player.GetComponent<Player>().transform.position - this.transform.position;
        //Si el alquimista ha sido rescatado
        if (GameController.instance.alchemist)
        {
            if (!shopPanel.activeSelf)
            {
                if (InputHandler.ButtonA() && (distPlayer.magnitude < range))
                {
                    Debug.Log("AlchemistOpened");
                    shopPanel.SetActive(true);
                    GameController.instance.player.GetComponent<Player>().state = Player.State.IDLE;
                    GameController.instance.player.GetComponent<Player>().HandleInput();
                    GameController.instance.player.GetComponent<Player>().move = false;
                }
            }
            else
            {
                if (InputHandler.ButtonB())
                {
                    Debug.Log("AlchemistClosed");
                    shopPanel.SetActive(false);
                    GameController.instance.player.GetComponent<Player>().move = true;
                    if (InputHandler.ButtonA()) Debug.Log("A");
                }
            }
        }
    }
}