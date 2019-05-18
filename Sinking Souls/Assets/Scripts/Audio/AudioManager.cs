using System;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

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

    public void Fade(string audioName, int seconds, float desiredVolume = 0.0f)
    {
        var sound = FindSound(audioName);
        
        StartCoroutine(Fade(sound.source, seconds, desiredVolume));
    }

    private IEnumerator Fade(AudioSource source, int seconds, float desiredVolume)
    {
        float initialVolume = source.volume;

        for (float t = 0; t < seconds; t += 0.05f)
        {
            // Interpolate values between [0, seconds] to [0, 1] for the Mathf.Lerp function.
            source.volume = Mathf.Lerp(initialVolume, desiredVolume, MapValue(t, 0, seconds, 0, 1));
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }

    private float MapValue(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    #endregion

    Sound FindSound(string audioName)
    {
        var foundSound = Array.Find(m_Effects, sound => sound.name == audioName);

        if (foundSound != null) return foundSound;

        foundSound = Array.Find(m_Music, sound => sound.name == audioName);

        if (foundSound != null) return foundSound;

        Debug.LogWarning("Sound named " + audioName + " not found.");
        return null;
    }

    #region Audio Playing Functions

    public void Play(string audioName)
    {
        Sound sound = FindSound(audioName);

        if (sound.interruptible)
        {
            if (sound.source.isPlaying) sound.source.Stop();
            sound.source.Play();

            return;
        }

        sound.source.volume = sound.volume;
        if (!sound.source.isPlaying) sound.source.Play();
    }

    [Obsolete("This is an deprecated method. Use Play() method instead.")]
    public void PlayEffect(string audioName)
    {
        Sound soundToPlay = FindSound(audioName);

        if (soundToPlay.interruptible)
        {
            if (soundToPlay.source.isPlaying) soundToPlay.source.Stop();
            soundToPlay.source.Play();

            return;
        }

        soundToPlay.source.volume = soundToPlay.volume;
        if (!soundToPlay.source.isPlaying) soundToPlay.source.Play();
    }

    [Obsolete("This is an deprecated method. Use Play() method instead.")]
    public void PlayMusic(string audioName)
    {
        Sound sound = FindSound(audioName);

        sound.source.volume = sound.volume;
        sound.source.Play();
    }


    public void Pause(string audioName) {

        Sound sound = FindSound(audioName);

        sound.source.Pause();
    }

    public void Stop(string audioName)
    {
        Sound sound = FindSound(audioName);

        sound.source.Stop();
    }

    public void StopAll()
    {
        Array.ForEach(m_Effects, sound => sound.source.Stop());
        Array.ForEach(m_Music, sound => sound.source.Stop());
    }

    public void StopAllEffects()
    {
        Array.ForEach(m_Effects, sound => sound.source.Stop());
    }

    public void PlayFade(string audioName, int seconds, float initialVolume, float finalVolume)
    {
        var sound = FindSound(audioName);

        sound.source.volume = initialVolume;
        sound.source.Play();
        StartCoroutine(Fade(sound.source, seconds, finalVolume));
    }

    // Plays the audio from the initial volume to the volume specified in the inspector.
    public void PlayFade(string audioName, int seconds, float initialVolume)
    {
        var sound = FindSound(audioName);

        sound.source.volume = initialVolume;
        sound.source.Play();
        StartCoroutine(Fade(sound.source, seconds, sound.volume));
    }
    #endregion
}
