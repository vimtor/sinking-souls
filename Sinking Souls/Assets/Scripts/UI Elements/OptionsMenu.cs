using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// This class is thought to be an adapter or interface between the UI and OptionsManager.
/// It has different functions that use OptionsManager functions.
/// </summary>
public class OptionsMenu : MonoBehaviour
{
    public TMP_Dropdown m_ResolutionsDropdown;
    public TMP_Dropdown m_GraphicsDropdown;
    public Toggle m_FullscreenToggle;

    public Slider m_MasterSlider;
    public Slider m_EffectsSlider;
    public Slider m_MusicSlider;

    void Start ()
    {
        #region ResolutionDropdown Setup
        var resolutions = OptionManager.Instance.Resolutions;

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
        List<string> qualities = new List<string>(OptionManager.Instance.Qualities);
        m_GraphicsDropdown.ClearOptions();
        m_GraphicsDropdown.AddOptions(qualities);

        m_GraphicsDropdown.value = QualitySettings.GetQualityLevel();
        m_ResolutionsDropdown.RefreshShownValue();
        #endregion
    }

    #region Graphics API
    public void SetOptions()
    {
        OptionManager.Instance.SetOptions();
    }

    public void SetResolution()
    {
        OptionManager.Instance.ResolutionIndex = m_ResolutionsDropdown.value;
    }

    public void SetFullscreen()
    {
        OptionManager.Instance.IsFullscreen = m_FullscreenToggle.isOn;
    }

    public void SetQuality()
    {
        OptionManager.Instance.QualityLevel = m_GraphicsDropdown.value;
    }
    #endregion

    #region Audio API
    public void SetMasterVolume()
    {
        OptionManager.Instance.MasterVolume = m_MasterSlider.value;
    }

    public void SetEffectsVolume()
    {
        OptionManager.Instance.EffectsVolume = m_EffectsSlider.value;
    }

    public void SetMusicVolume()
    {
        OptionManager.Instance.MusicVolume = m_MusicSlider.value;
    }
    #endregion
}
