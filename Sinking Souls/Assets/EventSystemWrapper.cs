using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemWrapper : MonoBehaviour
{
    private EventSystem _eventSystem;

    public static EventSystemWrapper Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        _eventSystem = GetComponent<EventSystem>();
    }

    public void SelectFirst(GameObject selectable)
    {
        StartCoroutine(SelectFirstCoroutine(selectable));
    }

    // Needed to highlight the button when selecting it.
    private IEnumerator SelectFirstCoroutine(GameObject selectable)
    {
        _eventSystem.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        _eventSystem.SetSelectedGameObject(selectable);
    }

    public GameObject CurrentSelected()
    {
        return _eventSystem.currentSelectedGameObject;
    }

    public void Select(GameObject selectable)
    {
        Debug.Log(selectable);
        _eventSystem.SetSelectedGameObject(selectable);
    }

    public bool IsSelected(GameObject selectable)
    {
        return selectable == CurrentSelected();
    }
}