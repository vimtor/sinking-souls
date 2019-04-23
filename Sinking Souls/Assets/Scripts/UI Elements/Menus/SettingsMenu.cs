using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// This class is thought to be an adapter or interface between the UI and OptionsManager.
/// It has different functions that use OptionsManager functions.
/// </summary>
public class SettingsMenu : MonoBehaviour
{
   

    [Header("Configuration:")]
    public float delay;
    public Color normalColor;
    public Color highlitedColor;
    public Color hidedColor;

    private GameObject ESys;
    private bool hided = false;

    public float time = 0;
    public GameObject[] selectorArr;
    private TextMeshProUGUI AudioSetingText;
    public int selected = 0;
    private Ray ray;
    public RaycastHit hit;
    public UnityEvent buttonEvents;
    public Event test;



    [Header("Settings")]
    public GameObject button;
    public TMP_Dropdown m_ResolutionsDropdown;
    public TMP_Dropdown m_GraphicsDropdown;
    public Toggle m_FullscreenToggle;

    public Slider m_MasterSlider;
    public Slider m_EffectsSlider;
    public Slider m_MusicSlider;

    void Start ()
    {

        AudioSetingText = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();

        selectorArr = new GameObject[] { m_FullscreenToggle.gameObject, m_ResolutionsDropdown.gameObject, m_GraphicsDropdown.gameObject, m_MasterSlider.gameObject, m_EffectsSlider.gameObject, m_MusicSlider.gameObject, button };

        foreach(GameObject go in selectorArr)
        {
            Disable(go);
        }
        Enable(selectorArr[selected]);

        #region ResolutionDropdown Setup
        var resolutions = SettingsManager.Instance.Resolutions;

        // Add the resolution options as formatted strings.
        List<string> options = new List<string>();
        foreach (var resolution in resolutions)
        {
            string option = resolution.width + " x " + resolution.height;
            options.Add(option);
        }

        m_ResolutionsDropdown.ClearOptions();
        m_ResolutionsDropdown.AddOptions(options);

        // Set the current resolution index on the dropdown.
        var currentResolution = Screen.currentResolution;
        m_ResolutionsDropdown.value = Array.FindIndex(resolutions, resolution => resolution.width == currentResolution.width && resolution.height == currentResolution.height); ;
        m_ResolutionsDropdown.RefreshShownValue();
        #endregion

        #region QualityDropdown Setup
        List<string> qualities = new List<string>(SettingsManager.Instance.Qualities);
        m_GraphicsDropdown.ClearOptions();
        m_GraphicsDropdown.AddOptions(qualities);

        m_GraphicsDropdown.value = QualitySettings.GetQualityLevel();
        m_ResolutionsDropdown.RefreshShownValue();
        #endregion

        m_MasterSlider.value = SettingsManager.Instance.MasterVolume;
        m_EffectsSlider.value = SettingsManager.Instance.EffectsVolume;
        m_MusicSlider.value = SettingsManager.Instance.MusicVolume;
    }

    void Update()
    {
        if (!selectorArr[selected].GetComponent<TMP_Dropdown>() || !selectorArr[selected].GetComponent<TMP_Dropdown>().IsExpanded)
        {

            if (hided)
            {
                foreach (GameObject go in selectorArr)
                {
                    Disable(go);
                    if (go.GetComponent<Slider>())
                    {
                        go.GetComponent<SliderBehaviour>().hided = false;
                    }
                }
                AudioSetingText.color = highlitedColor;
                hided = false;
            }

            if (InputManager.ButtonB || Input.GetKey(KeyCode.Escape))
            {
                InputManager.ButtonB = false;
                EventSystemWrapper.Instance.Select(button, 0.1f);
                gameObject.SetActive(false);
            }


            if (gameObject.activeInHierarchy)
            {
                ray.origin = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -100);
                ray.direction = new Vector3(0, 0, 1);

                //Key Input
                if (time >= delay)
                {
                    if (DownInput()) MoveDown();
                    else if (UpInput()) MoveUp();
                }

                //Mouse Input
                if (Physics.Raycast(ray, out hit) && Cursor.visible)
                {
                    Debug.Log(hit.transform.gameObject.name);
                    if (hit.transform.gameObject != selectorArr[selected])
                    {
                        Disable(selectorArr[selected]);
                        for (int i = 0; i < selectorArr.Length; i++)
                        {
                            if (hit.transform.gameObject == selectorArr[i])
                            {
                                selected = i;
                                Enable(selectorArr[selected]);
                            }
                        }
                    }
                }
                //else if (Cursor.visible) ESys.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);


                Enable(selectorArr[selected]);

                //Alternative controls (don't use gamepad and mouse to prevent override)
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    InputManager.ButtonA = false;
                    Execute(selectorArr[selected]);
                }

            }
            else
            {
                Disable(selectorArr[selected]);
                selected = 0;
            }

            time += Time.unscaledDeltaTime;
        }
        else if (selectorArr[selected].GetComponent<TMP_Dropdown>().IsExpanded && !hided)
        {
            for(int i = selected+1; i < selectorArr.Length; i++)
            {
                Hide(selectorArr[i]);
            }
            AudioSetingText.color = hidedColor;
            hided = true;
        }

    }

    public void CloseMenu()
    {
        Disable(selectorArr[selected]);
        selected = 0;
        Enable(selectorArr[selected]);
        InputManager.ButtonA = false;
        InputManager.ButtonB = false;

        Debug.Log("Exito");
    }

    void Enable(GameObject go)
    {
        if (go.GetComponent<Button>())
        {
            go.GetComponentInChildren<TextMeshProUGUI>().color = highlitedColor;
            go.GetComponent<Button>().Select();
            buttonEvents = go.GetComponent<Button>().onClick;
        }
        else if (go.GetComponent<Toggle>())
        {
            go.transform.parent.GetChild(go.transform.GetSiblingIndex()-1).GetComponent<TextMeshProUGUI>().color = highlitedColor;          //Title
            go.GetComponentInChildren<Image>().color = highlitedColor;                                                                      //Option
            go.GetComponent<Toggle>().Select();
        }
        else if (go.GetComponent<TMP_Dropdown>())
        {
            go.transform.parent.GetChild(go.transform.GetSiblingIndex() - 1).GetComponent<TextMeshProUGUI>().color = highlitedColor;        //Title
            go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = highlitedColor;                                                //Option
            go.GetComponent<TMP_Dropdown>().Select();
          
        }
        else if (go.GetComponent<Slider>())
        {
            go.transform.parent.parent.GetChild(go.transform.parent.GetSiblingIndex() - 1).GetComponent<TextMeshProUGUI>().color = highlitedColor;
            go.GetComponent<Slider>().Select();
        }
    }

    void Disable(GameObject go)
    {

        if (go.GetComponent<Button>())
        {
            go.GetComponentInChildren<TextMeshProUGUI>().color = normalColor;
        }
        else if (go.GetComponent<Toggle>())
        {
            go.transform.parent.GetChild(go.transform.GetSiblingIndex() - 1).GetComponent<TextMeshProUGUI>().color = normalColor;           //Title
            go.GetComponentInChildren<Image>().color = normalColor;                                                                         //Option
        }
        else if (go.GetComponent<TMP_Dropdown>())
        {
            go.transform.parent.GetChild(go.transform.GetSiblingIndex() - 1).GetComponent<TextMeshProUGUI>().color = normalColor;           //Title
            go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = normalColor;                                                   //Option
        }
        else if (go.GetComponent<Slider>())
        {
            go.transform.parent.parent.GetChild(go.transform.parent.GetSiblingIndex() - 1).GetComponent<TextMeshProUGUI>().color = normalColor;
        }

    }

    void Hide(GameObject go)
    {

        if (go.GetComponent<Button>())
        {
            go.GetComponentInChildren<TextMeshProUGUI>().color = hidedColor;
        }
        else if (go.GetComponent<Toggle>())
        {
            go.transform.parent.GetChild(go.transform.GetSiblingIndex() - 1).GetComponent<TextMeshProUGUI>().color = hidedColor;           //Title
            go.GetComponentInChildren<Image>().color = hidedColor;                                                                         //Option
        }
        else if (go.GetComponent<TMP_Dropdown>())
        {
            go.transform.parent.GetChild(go.transform.GetSiblingIndex() - 1).GetComponent<TextMeshProUGUI>().color = hidedColor;           //Title
            go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = hidedColor;                                                   //Option
        }
        else if (go.GetComponent<Slider>())
        {
            go.GetComponent<SliderBehaviour>().hided = true;
            go.transform.parent.parent.GetChild(go.transform.parent.GetSiblingIndex() - 1).GetComponent<TextMeshProUGUI>().color = hidedColor;
        }

    }

    void Execute(GameObject go)
    {
        if (go.GetComponent<Button>())
        {
            buttonEvents = go.GetComponent<Button>().onClick;
            buttonEvents.Invoke();
        }
        else if (go.GetComponent<Toggle>())
        {                                                                  
            go.GetComponent<Toggle>().isOn = !go.GetComponent<Toggle>().isOn;
        }
        else if (go.GetComponent<TMP_Dropdown>())
        {
            go.GetComponent<TMP_Dropdown>().Show();
        }
        else if (go.GetComponent<Slider>())
        {
        }
    }

    ///Input
    bool DownInput()
    {
        return InputManager.LeftJoystick.y > 0.1f || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || InputManager.Dpad.y < -0.1f;
    }

    bool UpInput()
    {
        return InputManager.LeftJoystick.y < -0.1f || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || InputManager.Dpad.y > 0.1f;
    }


    ///Movement
    void MoveDown()
    {

        Disable(selectorArr[selected]);
        selected = (selected + 1) % selectorArr.Length;
        Cursor.visible = false;
        time = 0;

    }

    void MoveUp()
    {

        Disable(selectorArr[selected]);
        if (selected - 1 >= 0) selected--;
        else selected = selectorArr.Length - 1;
        Cursor.visible = false;
        time = 0;

    }



    #region Graphics API
    public void SetOptions()
    {
        SettingsManager.Instance.SetOptions();
    }

    public void SetResolution()
    {
        SettingsManager.Instance.ResolutionIndex = m_ResolutionsDropdown.value;
    }

    public void SetFullscreen()
    {
        SettingsManager.Instance.IsFullscreen = m_FullscreenToggle.isOn;
    }

    public void SetQuality()
    {
        SettingsManager.Instance.QualityLevel = m_GraphicsDropdown.value;
    }
    #endregion

    #region Audio API
    public void SetMasterVolume()
    {
        SettingsManager.Instance.MasterVolume = m_MasterSlider.value;
    }

    public void SetEffectsVolume()
    {
        SettingsManager.Instance.EffectsVolume = m_EffectsSlider.value;
    }

    public void SetMusicVolume()
    {
        SettingsManager.Instance.MusicVolume = m_MusicSlider.value;
    }
    #endregion
}
