using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mouseCursor : MonoBehaviour {


	private Image image;
	public Sprite mouseDown;
    public Sprite normalCursor;
    private Vector2 offset = new Vector2(-13,13);

    public bool visible = false;
    public float apearingSpeed;

	public GameObject clickEffect;

	void Awake(){
		Cursor.visible = false;
		image = GetComponent<Image>();
	}

	void Update () {

        if (Input.GetKeyDown(KeyCode.Y)) visible = !visible;
        if (visible)
        {

            image.color = new Vector4(image.color.r, image.color.g, image.color.b, image.color.a + apearingSpeed * Time.unscaledDeltaTime);
            if (image.color.a >= 1) image.color = new Vector4(image.color.r, image.color.g, image.color.b, 1);

            Vector2 cursorPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - offset;

            transform.position = cursorPos;

            if (Input.GetMouseButtonDown(0))
            {
                image.sprite = mouseDown;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                image.sprite = normalCursor;

                GameObject eff = Instantiate(clickEffect);
                eff.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Destroy(eff, 3);
            }
        }
        else
        {
            image.color = new Vector4(image.color.r, image.color.g, image.color.b, image.color.a - apearingSpeed * Time.unscaledDeltaTime);
            if (image.color.a <= 0) image.color = new Vector4(image.color.r, image.color.g, image.color.b, 0);
        }
	}

    public void Hide()
    {
        visible = false;
    }

    public void Show()
    {
        visible = true;
    }

    public void InstaHide()
    {
        visible = false;
        image.color = new Vector4(image.color.r, image.color.g, image.color.b, 0);
    }
    public void InstaShow()
    {
        visible = true;
        image.color = new Vector4(image.color.r, image.color.g, image.color.b, 1);
    }

}
