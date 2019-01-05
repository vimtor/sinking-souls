using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;
using TMPro;

public abstract class ShopBehaviour<T> : MonoBehaviour
{
    [Header("Configuration")]
    public EventSystem m_EventSystem;
    public int m_InteractRange;

    protected Vector3 m_DistancePlayer;
    protected GameObject m_OldSelection;
    protected GameObject m_SelectedItem;

    [Header("Shop Interface")]
    public GameObject m_ShopPanel;
    public GameObject m_Item;

    [Space(10)]

    public Text m_Price;
    public Text m_RemainingSouls;


    protected void Update()
    {
        m_DistancePlayer = GameController.instance.player.GetComponent<Player>().transform.position - transform.position;

        if (!m_ShopPanel.activeSelf)
        {
            // Open the store.
            if (InputManager.GetButtonA() && (m_DistancePlayer.magnitude < m_InteractRange))
            {
                m_ShopPanel.SetActive(true);
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


            // Check if selected item was updated.
            if (m_EventSystem.currentSelectedGameObject != m_OldSelection)
            {
                UpdateShop();
            }

            m_OldSelection = m_EventSystem.currentSelectedGameObject;
        }
    }


    protected GameObject SetupItem(T commodity)
    {
        return Configure(Instantiate(m_Item), commodity);
    }

    protected virtual void UpdateShop()
    {
        m_SelectedItem = m_EventSystem.currentSelectedGameObject;

        m_Price.text = m_SelectedItem.transform.Find("Price").GetComponent<TextMeshProUGUI>().text;

        int remainingSouls = GameController.instance.LobbySouls - m_SelectedItem.GetComponent<ShopItem>().price;
        m_RemainingSouls.text = remainingSouls.ToString();

        m_OldSelection = m_EventSystem.currentSelectedGameObject;
    }


    protected abstract GameObject Configure(GameObject item, T commodity);

    public abstract void FillShop();

}