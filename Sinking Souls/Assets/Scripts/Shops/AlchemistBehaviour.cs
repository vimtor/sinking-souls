using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;


public class AlchemistBehaviour : MonoBehaviour
{
    [Header("Shop Interface")]
    public GameObject m_ShopPanel;
    public GameObject m_Item;

    [Space(10)]

    public Text m_Price;
    public Text m_RemainingSouls;

    [Header("Configuration")]
    public EventSystem m_EventSystem;
    public int m_InteractRange;


    private Vector3 m_DistancePlayer;
    private GameObject m_OldSelection;


    protected GameObject SetupItem(Ability ability)
    {
        return Configure(Instantiate(m_Item), ability);
    }

    protected GameObject Configure(GameObject item, Ability ability)
    {
        // Configure the ui item.
        item.transform.Find("Icon").GetComponent<Image>().sprite = ability.sprite;
        item.transform.Find("Name").GetComponent<Text>().text = ability.name;
        item.transform.Find("Price").GetComponent<Text>().text = ability.price.ToString();
        item.transform.SetParent(m_ShopPanel.transform.GetChild(0), false);

        // Store values in the ShopItem component for easier acces later on.
        item.GetComponent<ShopItem>().price = ability.price;

        return item;
    }

    public void FillShop()
    {
        Ability[] abilities = GameController.instance.abilities;

        // Select first selected game object.
        m_EventSystem.SetSelectedGameObject(SetupItem(abilities[0]));

        // Instantiate the rest of the items unless the first one.
        Array.ForEach(abilities.Skip(1).ToArray(), ability => SetupItem(ability));
    }

    public void UpdateShop()
    {
        GameObject selectedItem = m_EventSystem.currentSelectedGameObject;

        m_Price.text = selectedItem.transform.Find("Price").GetComponent<Text>().text;

        int remainingSouls = GameController.instance.LobbySouls - selectedItem.GetComponent<ShopItem>().price;
        m_RemainingSouls.text = remainingSouls.ToString();

        m_OldSelection = m_EventSystem.currentSelectedGameObject;
    }

    private void Update()
    {
        m_DistancePlayer = GameController.instance.player.GetComponent<Player>().transform.position - transform.position;

        if (!m_ShopPanel.activeSelf)
        {
            // Open the store.
            if (InputManager.GetButtonA() && (m_DistancePlayer.magnitude < m_InteractRange))
            {
                m_ShopPanel.SetActive(true);
                FillShop();
                UpdateShop();

                // Stop the player.
                GameController.instance.player.GetComponent<Player>().Stop();
                GetComponent<Animator>().SetTrigger("Talk");
            }
        }
        else
        {
            // Close the store.
            if (InputManager.GetButtonB())
            {
                InputManager.ButtonB = false;

                m_ShopPanel.SetActive(false);
                GameController.instance.player.GetComponent<Player>().Resume();
            }

            if (m_EventSystem.currentSelectedGameObject != m_OldSelection)
            {
                UpdateShop();
            }

            m_OldSelection = m_EventSystem.currentSelectedGameObject;
        }
    }
}