using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoulsUI : MonoBehaviour {

    [HideInInspector]public enum Color { RED, GREEN, BLUE};
    public Color color;

    private Text text;
    private string lastValue;

    private void Start() {
        text = GetComponent<Text>();
        lastValue = "0";
    }

    public void UpdateText () {
        switch (color) {
            case Color.RED:
                if (lastValue != GameController.instance.redSouls.ToString()) {
                    text.text = lastValue = GameController.instance.redSouls.ToString();
                    text.fontSize += 2;
                    StartCoroutine(ZoomOut(0.3f));
                }
            break;
            case Color.GREEN:
                if (lastValue != GameController.instance.greenSouls.ToString()) {
                    text.text = lastValue = GameController.instance.greenSouls.ToString();
                    text.fontSize += 2;
                    StartCoroutine(ZoomOut(0.3f));
                }
            break;
            case Color.BLUE:
                if (lastValue != GameController.instance.blueSouls.ToString()) {
                    text.text = lastValue = GameController.instance.blueSouls.ToString();
                    text.fontSize += 2;
                    StartCoroutine(ZoomOut(0.3f));
                }
            break;
        }
    }
    private IEnumerator ZoomOut(float time) {
        yield return new WaitForSeconds(time);
        text.fontSize -= 2;

    }
}
