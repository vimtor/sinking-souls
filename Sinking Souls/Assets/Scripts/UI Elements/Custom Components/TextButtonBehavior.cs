using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TextButtonBehavior : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    public TextMeshProUGUI textMesh;

    private Button _button;
    private string _textBackup;

    private void Awake()
    {
        _textBackup = textMesh.text;
        _button = GetComponent<Button>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        SelectButton();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        DeselectButton();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        SubmitButton();
    }

    private void SelectButton()
    {
        AudioManager.Instance.PlayEffect(Random.value < 0.5f ? "MenuButton_1" : "MenuButton_2");
        textMesh.text = "~ " + _textBackup + " ~";
    }

    private void DeselectButton()
    {
        textMesh.text = _textBackup;
    }

    private void SubmitButton()
    {
        AudioManager.Instance.PlayEffect("MenuButton_Click");
    }
}