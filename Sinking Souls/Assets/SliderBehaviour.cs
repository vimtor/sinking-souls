using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderBehaviour : MonoBehaviour
{
    public Image fillArea;

    public Color normalColor;
    public Color highlightedColor;

    private void Update()
    {
        fillArea.color = EventSystemWrapper.Instance.IsSelected(gameObject) ? highlightedColor : normalColor;
    }
}
