using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropdownBehaviour : MonoBehaviour
{
    public TextMeshProUGUI label;
    public TMP_Dropdown dropdown;

    public Color highlightedColor;
    public Color defaultColor;

    private void Update()
    {
        label.color = EventSystemWrapper.Instance.IsSelected(gameObject) ? highlightedColor : defaultColor;
    }
}
