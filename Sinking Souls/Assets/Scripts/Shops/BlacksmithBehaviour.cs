using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;
using TMPro;

public class BlacksmithBehaviour : MonoBehaviour
{
    [Header("Shop Interface")]
    public GameObject m_ShopPanel;
    public GameObject m_ShopTitle;
    public GameObject m_Item;

    [Space(10)]

    public Text m_Price;
    public Text m_RemainingSouls;

    [Header("Configuration")]
    public EventSystem m_EventSystem;
    public int m_InteractRange;

    [Header("Camera")]
    public GameObject m_Camera;


    private Vector3 m_DistancePlayer;
    private GameObject m_OldSelection;


    protected GameObject SetupItem(Modifier modifier)
    {
        return Configure(Instantiate(m_Item), modifier);
    }

    protected GameObject Configure(GameObject item, Modifier modifier)
    {
        // Configure the ui item.
        item.transform.Find("Icon").GetComponent<Image>().sprite = modifier.sprite;
        item.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = modifier.name;
        item.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = modifier.description;
        item.transform.Find("Price").GetComponent<TextMeshProUGUI>().text = modifier.price.ToString();
        item.transform.SetParent(m_ShopPanel.transform.GetChild(0), false);

        // Store values in the ShopItem component for easier acces later on.
        item.GetComponent<ShopItem>().price = modifier.price;
        item.GetComponent<ShopItem>().modifier = modifier;

        return item;
    }

    public void FillShop()
    {
        Modifier[] modifiers = Array.FindAll(GameController.instance.modifiers, modifier => modifier.owned);

        // Select first selected game object.
        m_EventSystem.SetSelectedGameObject(SetupItem(modifiers[0]));

        // Instantiate the rest of the items unless the first one.
        Array.ForEach(modifiers.Skip(1).ToArray(), modifier => SetupItem(modifier));
    }

    public void UpdateShop()
    {
        GameObject selectedItem = m_EventSystem.currentSelectedGameObject;

        m_Price.text = selectedItem.transform.Find("Price").GetComponent<TextMeshProUGUI>().text;

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
                m_ShopTitle.SetActive(true);
                UpdateShop();

                m_Camera.SetActive(true);

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
                m_ShopTitle.SetActive(false);

                m_Camera.SetActive(false);

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
