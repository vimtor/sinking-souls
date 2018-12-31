using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public GameObject m_PauseContent;
    public GameObject m_ResumeButton;

    public EventSystem m_EventSystem;

    private bool m_IsPaused;

	void Start ()
    {
        m_IsPaused = false;
	}
	
	void Update ()
    {
		if (InputManager.Start())
        {
            if (!m_IsPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
	}

    private void Pause()
    {
        m_IsPaused = true;
        m_PauseContent.SetActive(true);
        m_EventSystem.SetSelectedGameObject(m_ResumeButton);
        Time.timeScale = 0.0f;
    }

    private void Resume()
    {
        m_IsPaused = false;
        m_PauseContent.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
