using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class AlchemistBehaviour : MonoBehaviour
{
    [Header("Shop Interface")]
    public GameObject m_ShopPanel;
    public GameObject m_Item;

    [Space(10)]

    private Text m_Price;
    private Text m_RemainingSouls;

    [Header("Configuration")]
    public EventSystem m_EventSystem;
    public int m_InteractRange;


    private int m_TotalSouls;
    private Vector3 m_DistPlayer;


    public void FillShop()
    {
        bool firstSelected = false;
        foreach (Ability ability in GameController.instance.abilities)
        {
            GameObject item = Instantiate(m_Item);

            // Configure the ui item.
            item.transform.Find("Icon").GetComponent<Image>().sprite = ability.sprite;
            item.transform.Find("Name").GetComponent<Text>().text = ability.name;
            item.transform.Find("Price").GetComponent<Text>().text = ability.price.ToString();
            item.transform.SetParent(m_ShopPanel.transform.GetChild(0), false);

            // Store values in the ShopItem component for easier acces later on.
            item.GetComponent<ShopItem>().price = ability.price;

            // Select first game object.
            if (!firstSelected)
            {
                m_EventSystem.SetSelectedGameObject(item);
                firstSelected = true;
            }

            Debug.Log("Item added");
        }

        m_TotalSouls = GameController.instance.LobbySouls;
    }

    public void UpdateShop()
    {
        GameObject selectedItem = m_EventSystem.currentSelectedGameObject;

        m_Price.text = selectedItem.transform.Find("Price").GetComponent<Text>().text;
        int remainingSouls = m_TotalSouls - selectedItem.GetComponent<ShopItem>().price;
        m_RemainingSouls.text = remainingSouls.ToString();
    }

    private void Update()
    {
        m_DistPlayer = GameController.instance.player.GetComponent<Player>().transform.position - transform.position;

        if (!m_ShopPanel.activeSelf)
        {
            // Open the store.
            if (InputManager.ButtonA && (m_DistPlayer.magnitude < m_InteractRange))
            {
                m_ShopPanel.SetActive(true);
                m_Price = GameObject.Find("SoulsPrice").GetComponent<Text>();
                m_RemainingSouls = GameObject.Find("SoulsRemaining").GetComponent<Text>();
                UpdateShop();

                // Stop the player.
                GameController.instance.player.GetComponent<Player>().CanMove = false;
            }
        }
        else
        {
            // Close the store.
            if (InputManager.ButtonB)
            {
                InputManager.ButtonB = false;

                m_ShopPanel.SetActive(false);
                GameController.instance.player.GetComponent<Player>().CanMove = true;
            }
        }
    }

}