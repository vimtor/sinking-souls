using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopButtonDisplayBehaviour : MonoBehaviour {

    bool isImage;
    Image image;
    TextMeshPro text;
    public float apearingSpeed = 30;
    bool fadeOut = false;
    // Use this for initialization
    void Start()
    {
        if (GetComponent<Image>())
        {
            image = GetComponent<Image>();
            isImage = true;
        }
        else
        {
            text = GetComponent<TextMeshPro>();
            isImage = false;
        }
        if (isImage)
            image.color = image.color * new Vector4(1, 1, 1, 0);
        else
        {
            text.color = text.color * new Vector4(1, 1, 1, 0);

        }
    }

    bool openShop()
    {
        
        return (GameObject.Find("Blacksmith Shop") != null || GameObject.Find("Alchemist Shop") != null || GameObject.Find("Alchemist Dialogue") != null || GameObject.Find("Innkeeper Shop") != null);
    }

    // Update is called once per frame
    void Update()
    {
        if (isImage)
        {
            if (Vector3.Distance(GameController.instance.player.transform.position, transform.position) < 4)
            {
                image.color = new Vector4(image.color.r, image.color.g, image.color.b, image.color.a - apearingSpeed * Time.deltaTime);
                if (image.color.a <= 0) image.color = new Vector4(image.color.r, image.color.g, image.color.b, 0);
            }
            else
            {
                image.color = new Vector4(image.color.r, image.color.g, image.color.b, image.color.a + apearingSpeed * Time.deltaTime);
                if (image.color.a >= 1) image.color = new Vector4(text.color.r, text.color.g, text.color.b, 1);
            }
        }
        else
        {
            if (Vector3.Distance(GameController.instance.player.transform.position, transform.position) > 4 || openShop())
            {
                text.color = new Vector4(text.color.r, text.color.g, text.color.b, text.color.a - apearingSpeed * Time.deltaTime);
                if (text.color.a <= 0) text.color = new Vector4(text.color.r, text.color.g, text.color.b, 0);
                
            }
            else
            {
                text.color = new Vector4(text.color.r, text.color.g, text.color.b, text.color.a + apearingSpeed * Time.deltaTime);
                if (text.color.a >= 1) text.color = new Vector4(text.color.r, text.color.g, text.color.b, 1);
            }
        }
    }

    public void destroy()
    {
        fadeOut = true;
    }
}
