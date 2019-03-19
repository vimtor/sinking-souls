using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollbarBehaviour : MonoBehaviour {

    public Scrollbar scrollbar;
    public float increment = 0.05f;

    private void Update() {
        if (InputManager.LeftJoystick.y > 0) scrollbar.value -= increment;
        if (InputManager.LeftJoystick.y < 0) scrollbar.value += increment;
    }

}
