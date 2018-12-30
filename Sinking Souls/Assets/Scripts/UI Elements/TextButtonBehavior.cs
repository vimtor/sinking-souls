using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TextButtonBehavior : MonoBehaviour,
                                  IPointerEnterHandler, IPointerExitHandler,
                                  ISelectHandler, IDeselectHandler,
                                  ISubmitHandler, IPointerClickHandler
{
    private TextMeshProUGUI m_TextMesh;
    private Button m_Button;

    private string m_TextBackup;

	void Start ()
    {
        m_TextMesh = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        m_TextBackup = m_TextMesh.text;

        m_Button = GetComponent<Button>();
	}

    public void OnPointerEnter(PointerEventData eventData) { SelectButton(); }
    public void OnPointerExit(PointerEventData eventData) { DeselectButton(); }

    public void OnSelect(BaseEventData eventData) { SelectButton(); }
    public void OnDeselect(BaseEventData eventData) { DeselectButton(); }

    public void OnPointerClick(PointerEventData eventData) { SubmitButton(); }
    public void OnSubmit(BaseEventData eventData) { SubmitButton(); }

    private void SelectButton()
    {
        if (Random.value < 0.5f) AudioManager.instance.Play("MenuButton_1");
        else AudioManager.instance.Play("MenuButton_2");

        m_TextMesh.text = "- " + m_TextBackup + " -";
    }

    private void DeselectButton()
    {
        m_TextMesh.text = m_TextBackup;
    }

    private void SubmitButton()
    {
        AudioManager.instance.Play("MenuButton_Click");
    }

}
