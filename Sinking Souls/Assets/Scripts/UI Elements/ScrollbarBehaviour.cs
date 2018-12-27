using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollbarBehaviour : MonoBehaviour {

    public Scrollbar scrollbar;

    private void Update() {
        if (InputManager.LeftJoystick.y > 0) scrollbar.value -= 0.05f;
        if (InputManager.LeftJoystick.y < 0) scrollbar.value += 0.05f;
    }

}
