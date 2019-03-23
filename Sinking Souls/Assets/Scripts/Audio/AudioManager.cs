using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager m_Instance;
    public static AudioManager Instance
    {
        get { return m_Instance; }
    }

    [Header("AudioMixer Setup")]
    [SerializeField] private AudioMixer m_AudioMixer;
    public AudioMixer AudioMixer
    {
        get { return m_AudioMixer; }
    }

    [SerializeField] private AudioMixerGroup m_EffectsGroup;
    [SerializeField] private AudioMixerGroup m_MusicGroup;

    [Header("Sound Lists")]
    public Sound[] m_Effects;
    public Sound[] m_Music;

    private void Awake()
    {
        #region SINGLETON
        if(m_Instance == null) {
            m_Instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        #endregion

        Array.ForEach(m_Effects, sound => SetupEffect(sound));
        Array.ForEach(m_Music, sound => SetupMusic(sound));
    }

    #region Setup Functions
    private void SetupSound(Sound sound)
    {
        sound.source = gameObject.AddComponent<AudioSource>();

        sound.source.clip = sound.clip;
        sound.source.volume = sound.volume;
        sound.source.pitch = sound.pitch;
        sound.source.loop = sound.loop;
    }

    private void SetupEffect(Sound sound)
    {
        SetupSound(sound);
        sound.source.outputAudioMixerGroup = m_EffectsGroup;
    }

    private void SetupMusic(Sound sound)
    {
        SetupSound(sound);
        sound.source.outputAudioMixerGroup = m_MusicGroup;
    }
    #endregion

    #region Change Volume Functions
    public void SetMasterVolume(float volume)
    {
        m_AudioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetEffectsVolume(float volume)
    {
        m_AudioMixer.SetFloat("EffectsVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        m_AudioMixer.SetFloat("MusicVolume", volume);
    }
    #endregion

    #region Audio Playing Functions
    public void PlayEffect(string name)
    {
        Sound soundToPlay = Array.Find(m_Effects, sound => sound.name == name);

        if (soundToPlay == null) {
            Debug.LogWarning("Sound " + name + " not found");
            return;
        }

        if (!soundToPlay.source.isPlaying) soundToPlay.source.Play();
    }

    public void PlayMusic(string name)
    {
        Sound soundToPlay = Array.Find(m_Music, sound => sound.name == name);

        if (soundToPlay == null)
        {
            Debug.LogWarning("Sound " + name + " not found");
            return;
        }

        soundToPlay.source.Play();
    }


    public void Pause(string name) {

        Sound soundToPlay = Array.Find(m_Effects, sound => sound.name == name);

        if (soundToPlay == null) {
            Debug.LogWarning("Sound" + name + " not found");
            return;
        }

        soundToPlay.source.Pause();
    }

    public void Stop(string name)
    {
        Sound soundToPlay = Array.Find(m_Effects, sound => sound.name == name);

        if (soundToPlay == null)
        {
            Debug.LogWarning("Sound" + name + " not found");
            return;
        }

        soundToPlay.source.Stop();
    }
    #endregion
}
