using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderBehaviour : MonoBehaviour
{
    public Image fillArea;

    public Color normalColor;
    public Color highlightedColor;

    [Header("Properties")]
    public float initialValue;
    private float barValue;
    public float offset = 4;
    public float delay = 0.01f;
    public float timer = 0;
    public bool waiting = false;
    private int leftCounter = 0;
    private int rughtCounter = 0;

    private void Start()
    {
        barValue = initialValue;
    }


    private void Update()
    {
        gameObject.GetComponent<Slider>().value = barValue;
        if (leftInput() && EventSystemWrapper.Instance.IsSelected(gameObject)) moveSlider(-offset);
        else if (rightInput() && EventSystemWrapper.Instance.IsSelected(gameObject)) moveSlider(offset);
        fillArea.color = EventSystemWrapper.Instance.IsSelected(gameObject) ? highlightedColor : normalColor;
        timer += Time.deltaTime;
    }

    private void moveSlider(float _value)
    {
        if (timer >= delay)
        {
            timer = 0;
            barValue += _value;
        }
    }

    private bool leftInput()
    {
        return (InputManager.LeftJoystick.x < -0.1f || Input.GetKeyDown(KeyCode.A) || Input.GetKey("left"));
    }

    private bool rightInput()
    {
        return (InputManager.LeftJoystick.x > 0.1f || Input.GetKeyDown(KeyCode.D) || Input.GetKey("right"));
    }

}
