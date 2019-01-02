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
		if (InputManager.ButtonStart)
        {
            InputManager.ButtonStart = false;

            if (!m_IsPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }

        if (m_IsPaused)
        {
            if (InputManager.ButtonB)
            {
                InputManager.ButtonB = false;
                Resume();
            }
        }
        
	}

    public void Pause()
    {
        m_IsPaused = true;
        m_PauseContent.SetActive(true);
        m_EventSystem.SetSelectedGameObject(m_ResumeButton);
        Time.timeScale = 0.0f;
    }

    public void Resume()
    {
        m_IsPaused = false;
        m_PauseContent.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
