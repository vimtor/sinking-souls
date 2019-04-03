using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DialogueButtonBehaviour : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    public TextMeshProUGUI textMesh;

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
        textMesh.text = '~' + textMesh.text.Remove(0, 1);
    }

    private void DeselectButton()
    {
        textMesh.text = ' ' + textMesh.text.Remove(0, 1);
    }

    private void SubmitButton()
    {
        AudioManager.Instance.PlayEffect("MenuButton_Click");
    }
}
