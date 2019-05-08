using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public Sprite[] possibleImages;
    private Image image;

	void Start ()
    {
        image = GetComponent<Image>();
        image.sprite = possibleImages[Random.Range(0, possibleImages.Length)];
    }
    private void Update() {
        Debug.Log( "Im On "+ SceneManager.GetActiveScene().name);
    }

}
