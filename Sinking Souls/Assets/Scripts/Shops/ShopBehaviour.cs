using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Linq;
using TMPro;

public abstract class ShopBehaviour<T> : MonoBehaviour
{
    [Header("Shop Interface")] public GameObject shopPanel;
    public GameObject shopTitle;
    public GameObject shopItem;

    [Space(10)] public TextMeshProUGUI price;
    public TextMeshProUGUI remainingSouls;

    [Header("Configuration")] public int interactRange;
    public float cursorHideTime = 1f;

    [Header("Camera")] public GameObject shopCamera;


    private Vector3 _minimumDistance;
    private GameObject _oldSelection;
    private bool _hiding = false;


    protected GameObject SetupItem(T commodity)
    {
        return Configure(Instantiate(shopItem), commodity);
    }

    private void UpdateShop()
    {
        GameObject selectedItem = EventSystemWrapper.Instance.CurrentSelected();

        Debug.Log(selectedItem.GetComponent<ShopItem>() == null);

        price.text = selectedItem.transform.Find("Price").GetComponent<TextMeshProUGUI>().text;

        int priceDifference = GameController.instance.LobbySouls - selectedItem.GetComponent<ShopItem>().price;
        remainingSouls.text = priceDifference.ToString();

        _oldSelection = EventSystemWrapper.Instance.CurrentSelected();
    }


    private void Update()
    {
        _minimumDistance = GameController.instance.player.GetComponent<Player>().transform.position -
                           transform.position;

        if (!shopPanel.activeSelf)
        {
            // Open the store.
            if (InputManager.GetButtonA() && (_minimumDistance.magnitude < interactRange))
            {
                shopPanel.SetActive(true);
                shopTitle.SetActive(true);
                UpdateShop();

                shopCamera.SetActive(true);

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
                shopPanel.SetActive(false);
                shopTitle.SetActive(false);

                Cursor.visible = false;
                shopCamera.SetActive(false);

                GameController.instance.player.GetComponent<Player>().Resume();
            }

            if (EventSystemWrapper.Instance.CurrentSelected() != _oldSelection)
            {
                UpdateShop();
            }

            if (Math.Abs(InputManager.Mouse.magnitude) > 0.0f)
            {
                _hiding = false;
                Cursor.visible = true;
            }
            else if (!_hiding)
            {
                _hiding = true;
                StartCoroutine(HideMouse(cursorHideTime));
            }

            _oldSelection = EventSystemWrapper.Instance.CurrentSelected();
        }
    }

    //Hide cursor if not use it for certain time
    private IEnumerator HideMouse(float time)
    {
        yield return new WaitForSeconds(time);
        if (_hiding) Cursor.visible = false;
    }


    protected abstract GameObject Configure(GameObject item, T commodity);

    public abstract void FillShop();
}