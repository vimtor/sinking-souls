using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseCursor : MonoBehaviour {


	private SpriteRenderer rend;
	public Sprite mouseDown;
	public Sprite normalCursor;

	public GameObject clickEffect;

	void Sart(){
		Cursor.visible = false;
		rend =  GetComponent<SpriteRenderer>();
	}

	void Update () {
		Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.position = cursorPos;

		if(Input.GetMouseButtonDown(0)){
			rend.sprite = mouseDown;
		} else if(Input.GetMouseButtonUp(0)){
			rend.sprite = normalCursor;
			Instantiate(clickEffect, transform.position, Quaternion.identity);
		}
	}
}
