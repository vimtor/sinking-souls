using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderBehaviour : MonoBehaviour
{
    public Image sliderArea;
    public Image pointArea;
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
        //Block default unity slider movement
        gameObject.GetComponent<Slider>().value = barValue;
        if (EventSystemWrapper.Instance.IsSelected(gameObject))
        {
            sliderArea.color = highlightedColor;
        }
        else
        {
            sliderArea.color = normalColor;
            pointArea.color = normalColor;
        }
        if (leftInput() && EventSystemWrapper.Instance.IsSelected(gameObject)) moveSlider(-offset);
        else if (rightInput() && EventSystemWrapper.Instance.IsSelected(gameObject)) moveSlider(offset);
        //fillArea.color = EventSystemWrapper.Instance.IsSelected(gameObject) ? highlightedColor : normalColor;
        timer += Time.unscaledDeltaTime;
    }

    private void moveSlider(float _value)
    {
        //Joystick case
        if (Mathf.Abs(InputManager.LeftJoystick.x) > 0.01f) {
            Debug.Log("Joystick");
            if (timer >= 0.04f)
            {
                timer = 0;
                barValue += ((_value / 4) * (Mathf.Abs(InputManager.LeftJoystick.x)*4));
            }

        }
        //Key case
        else {
            Debug.Log("Key");
            if (timer >= delay)
            {
                timer = 0;
                barValue += _value;               
            }
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
