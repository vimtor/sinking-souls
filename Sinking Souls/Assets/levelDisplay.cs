using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class levelDisplay : MonoBehaviour {
    public GameObject display;
    private GameObject sDisplay;
    public Sprite BG1, BG2;
    public Sprite[] levels;
    public float minimumDuration;
    public float fadingSpeed;
    private bool fade = false;

	void Start () {

        sDisplay = Instantiate(display, GameObject.Find("Canvas").transform, false);
        if(GameController.instance.level1.name == GameController.instance.gameObject.GetComponent<LevelGenerator>().level.name)sDisplay.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = BG1;
        else if(GameController.instance.level2.name == GameController.instance.gameObject.GetComponent<LevelGenerator>().level.name)sDisplay.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = BG2;
        else Destroy(sDisplay);


        if(sDisplay != null) {
            sDisplay.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = levels[GameController.instance.gameObject.GetComponent<LevelGenerator>().currentLevel];
        }

    }

    private float time = 0;
	// Update is called once per frame
	void Update () {

        if(time >= minimumDuration) {
            if(GameController.instance.player.GetComponent<Rigidbody>().velocity.magnitude > 0.01f) {

                fade = true;
            }
        }

        if (fade) {
            
            sDisplay.transform.GetChild(0).gameObject.GetComponent<Image>().color = sDisplay.transform.GetChild(0).gameObject.GetComponent<Image>().color - new Color(0, 0, 0, 1) * fadingSpeed * Time.deltaTime;
            sDisplay.transform.GetChild(1).gameObject.GetComponent<Image>().color = sDisplay.transform.GetChild(0).gameObject.GetComponent<Image>().color - new Color(0, 0, 0, 1) * fadingSpeed * Time.deltaTime;
            if (sDisplay.transform.GetChild(1).gameObject.GetComponent<Image>().color.a <= 0) {
                Destroy(sDisplay);
                fade = false;
            }
        }

        time += Time.deltaTime;
	}



}
