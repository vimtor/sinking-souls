using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    private static SettingsManager m_Instance;
    public static SettingsManager Instance
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
        get { return m_EffectsVolume; }
    }

    private float m_MusicVolume;
    public float MusicVolume
    {
        set { m_MusicVolume = value; }
        get { return m_MusicVolume; }
    }

    private float m_MasterVolume;
    public float MasterVolume
    {
        set { m_MasterVolume = value; }
        get { return m_MasterVolume; }
    }
    #endregion

    private void Awake()
    {
        #region SINGLETON
        if (m_Instance == null)
        {
            m_Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        #endregion

        m_Resolutions = Screen.resolutions;
    }

    private void Start()
    {
        AudioManager.Instance.AudioMixer.GetFloat("MasterVolume", out m_MasterVolume);
        AudioManager.Instance.AudioMixer.GetFloat("EffectsVolume", out m_EffectsVolume);
        AudioManager.Instance.AudioMixer.GetFloat("MusicVolume", out m_MusicVolume);
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
