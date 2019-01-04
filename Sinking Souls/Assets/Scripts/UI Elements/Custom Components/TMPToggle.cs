using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TMPToggle : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [Header("Text properties")]
    public Color32 m_NormalColor;
    public Color32 m_HighlightedColor;

    private TextMeshProUGUI m_TextMesh;
    private Toggle m_Toggle;

	void Awake ()
    {
        m_Toggle = GetComponent<Toggle>();
        m_TextMesh = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
	}

    public void OnSelect(BaseEventData eventData) { Select(); }
    public void OnDeselect(BaseEventData eventData) { Deselect(); }

    private void Select()
    {
        m_TextMesh.faceColor = m_HighlightedColor;
    }

    private void Deselect()
    {
        m_TextMesh.faceColor = m_NormalColor;
    }
}
