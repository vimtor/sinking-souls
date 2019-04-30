using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Linq;
using TMPro;

public abstract class ShopBehaviour<T> : MonoBehaviour
{
    [Header("Shop Interface")]
    public GameObject shopPanel;
    public GameObject shopTitle;
    public GameObject shopItem;
    public Color cantPayTextColor;
    public Color normalTextColor;

    [Space(10)] public TextMeshProUGUI price;
    public TextMeshProUGUI remainingSouls;

    [Header("Configuration")] public int interactRange;

    [Header("Camera")] public GameObject shopCamera;

    public bool isOpen;
    private GameObject oldSelection;
    private bool hiding;

    private Animator animator;
    private static readonly int kTalkParam = Animator.StringToHash("Talk");

    [Header("Dialogue")]
    public bool dialogable;
    public GameObject dialogueObject;
    public GameObject firstOption;
    private bool dialoguing;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    protected GameObject SetupItem(T commodity)
    {
        return Configure(Instantiate(shopItem), commodity);
    }

    private void UpdateShop()
    {
        var selectedItem = EventSystemWrapper.Instance.CurrentSelected();
        if (selectedItem == null) return;

        price.text = selectedItem.transform.Find("Price").GetComponent<TextMeshProUGUI>().text;

        int priceDiff = GameController.instance.lobbySouls - selectedItem.GetComponent<ShopItem>().price;
        if (priceDiff < 0) remainingSouls.color = cantPayTextColor;             //Too expensive
        else remainingSouls.color = normalTextColor;                            // Can buy
        remainingSouls.text = priceDiff.ToString();

        oldSelection = EventSystemWrapper.Instance.CurrentSelected();
    }


    private void Update()
    {
        if (dialoguing)
        {
            if (InputManager.GetButtonB() || Input.GetKeyDown(KeyCode.Escape))
            {
                InputManager.ButtonB = false;
                dialoguing = false;

                GameController.instance.player.GetComponent<Player>().Resume();
                dialogueObject.SetActive(false);
            }
        }

        if (!isOpen)
        {
            // Open the store.
            if (InputManager.GetButtonA() || Input.GetKeyDown(KeyCode.Return))
            {
                if (isOpen) return;
                InputManager.ButtonA = false;

                var minimumDistance = GameController.instance.player.GetComponent<Player>().transform.position - transform.position;
                if (minimumDistance.magnitude > interactRange) return;

                if (dialogable)
                {
                    dialogueObject.SetActive(true);
                    EventSystemWrapper.Instance.SelectFirst(firstOption);
                    GameController.instance.player.GetComponent<Player>().Stop();
                    dialoguing = true;
                }
                else
                {
                    OpenShop();
                }
            }
        }
        else
        {
            //UpdateShop();
            // Close the store.
            if (InputManager.GetButtonB() || Input.GetKeyDown(KeyCode.Escape))
            {
                InputManager.ButtonB = false;
                CloseShop();
            }

            if (shopPanel.transform.Find("Content").childCount > 0 && EventSystemWrapper.Instance.CurrentSelected() != oldSelection)
            {
                UpdateShop();
            }

            oldSelection = EventSystemWrapper.Instance.CurrentSelected();
        }
    }

    private void CloseShop()
    {
        shopPanel.SetActive(false);
        shopTitle.SetActive(false);
        shopCamera.SetActive(false);

        Cursor.visible = false;
  
        GameController.instance.player.GetComponent<Player>().Resume();
        animator.SetTrigger(kTalkParam);

        isOpen = false;
    }

    public void OpenShop()
    {
        shopPanel.SetActive(true);
        shopTitle.SetActive(true);
        shopCamera.SetActive(true);

        if (dialogable) dialogueObject.SetActive(false);

        var content = shopPanel.transform.Find("Content");
        if(content.transform.childCount > 0)
        {
            var firstItem = content.transform.GetChild(0).gameObject;
            EventSystemWrapper.Instance.Select(firstItem);

            UpdateShop();
        }
       

        // Stop the player.
        GameController.instance.player.GetComponent<Player>().Stop();
        animator.SetTrigger(kTalkParam);

        isOpen = true;
    }


    protected abstract GameObject Configure(GameObject item, T commodity);

    public abstract void FillShop();
}