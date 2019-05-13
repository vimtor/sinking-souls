using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderBehaviour : MonoBehaviour
{

    private Image sliderArea;
    private Image pointArea;
    private Image backGround;

    [Header("Colors:")]
    [Header("Slider")]
    public Color normalSliderColor;
    public Color highlightedSliderColor;
    [Header("Background")]
    public Color normalBackgroundColor;
    public Color highlitedBackgroundColor;
    [Header("Point")]
    public Color normalPointColor;
    public Color highlitedPointColor;
    public Color PressedPointColor;
    public Color hidedColor;

    [Header("Properties")]
    public float initialValue;
    public bool hided = false;
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

        backGround = gameObject.transform.GetChild(0).GetComponent<Image>();
        sliderArea = gameObject.transform.GetChild(1).GetComponentInChildren<Image>();
        pointArea = gameObject.transform.GetChild(2).GetComponentInChildren<Image>();
        gameObject.GetComponent<Slider>().value = barValue;

    }


    private void Update()
    {
        //Block default unity slider movement
        //gameObject.GetComponent<Slider>().value = barValue;
        if (hided)
        {
            sliderArea.color = hidedColor;
            pointArea.color = hidedColor;
            backGround.color = hidedColor;
        }
            else
        {
            if (EventSystemWrapper.Instance.IsSelected(gameObject))
            {
                sliderArea.color = highlightedSliderColor;
                pointArea.color = highlitedPointColor;
                backGround.color = highlitedBackgroundColor;
            }
            else
            {
                sliderArea.color = normalSliderColor;
                pointArea.color = normalPointColor;
                backGround.color = normalBackgroundColor;
            }
            if (leftInput() && EventSystemWrapper.Instance.IsSelected(gameObject)) moveSlider(-offset);
            else if (rightInput() && EventSystemWrapper.Instance.IsSelected(gameObject)) moveSlider(offset);

        }

        timer += Time.unscaledDeltaTime;
    }

    private void moveSlider(float _value)
    {
        //Joystick case
        if (Mathf.Abs(InputManager.LeftJoystick.x) > 0.01f && !(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))) {
            Debug.Log("Joystick");
            if (timer >= 0.04f)
            {
                timer = 0;
                barValue += ((_value / 4) * (Mathf.Abs(InputManager.LeftJoystick.x)*4));
            }
            gameObject.GetComponent<Slider>().value = barValue;

        }
        //Key case
        else if(!Input.GetMouseButton(0))
        {
            Debug.Log("Key");
            if (timer >= delay)
            {
                timer = 0;
                barValue += _value;               
            }
            gameObject.GetComponent<Slider>().value = barValue;
        }
        //Mouse
        else
        {
            Debug.Log("Pulsacion");
            pointArea.color = PressedPointColor;
            float distance = (Input.mousePosition.x - gameObject.transform.position.x);
            barValue = GameController.instance.Map(distance, -113, 108, -80, 10, true);
            Debug.Log(distance);
            gameObject.GetComponent<Slider>().value = barValue;
        }
    }

    private bool leftInput()
    {
        return (InputManager.LeftJoystick.x < -0.1f || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || InputManager.Dpad.x < -0.1f || Input.GetMouseButton(0));
    }

    private bool rightInput()
    {
        return (InputManager.LeftJoystick.x > 0.1f || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || InputManager.Dpad.x > 0.1f || Input.GetMouseButton(0));
    }

}
