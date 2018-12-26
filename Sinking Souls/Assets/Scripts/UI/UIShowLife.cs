using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIShowLife : MonoBehaviour {

    // Update is called once per frame
    private Text text;

    private void Start() {
        text = GetComponent<Text>();
    }

    void Update () {
        text.text = GameController.instance.player.GetComponent<Player>().Health.ToString();
    }
}
