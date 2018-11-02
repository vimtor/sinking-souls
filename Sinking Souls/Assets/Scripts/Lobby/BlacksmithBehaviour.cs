using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlacksmithBehaviour : MonoBehaviour {

    public GameObject shopPanel;

	private void Start () {
        GameController.instance.blacksmith = true;
	}
}
