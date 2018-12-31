using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    private static OptionManager m_Instance;
    public static OptionManager Instance
    {
        get { return m_Instance; }
    }

    #region Graphic Options
    private Resolution[] m_Resolutions;
    public Resolution[] Resolutions
    {
        get { return m_Resolutions; }
    }

    private int m_ResolutionIndex;
    public int ResolutionIndex
    {
        set { m_ResolutionIndex = value; }
    }

    private bool m_IsFullscreen;
    public bool IsFullscreen
    {
        set { m_IsFullscreen = value; }
    }

    private int m_QualityLevel;
    public int QualityLevel
    {
        set { m_QualityLevel = value;  }
    }

    public string[] Qualities
    {
        get { return QualitySettings.names; }
    }
    #endregion

    #region Audio Options
    private float m_EffectsVolume;
    public float EffectsVolume
    {
        set { m_EffectsVolume = value; }
    }

    private float m_MusicVolume;
    public float MusicVolume
    {
        set { m_MusicVolume = value; }
    }

    private float m_MasterVolume;
    public float MasterVolume
    {
        set { m_MasterVolume = value; }
    }
    #endregion

    private void Awake()
    {
        #region SINGLETON
        if (m_Instance == null)
        {
            m_Instance = this;
        }
        else if (m_Instance != this)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(gameObject);
        #endregion

        m_Resolutions = Screen.resolutions;
    }

    public void SetOptions()
    {
        Resolution resolution = m_Resolutions[m_ResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, m_IsFullscreen);
        QualitySettings.SetQualityLevel(m_QualityLevel);

        AudioManager.Instance.SetMasterVolume(m_MasterVolume);
        AudioManager.Instance.SetEffectsVolume(m_EffectsVolume);
        AudioManager.Instance.SetMusicVolume(m_MusicVolume);
    }
}
