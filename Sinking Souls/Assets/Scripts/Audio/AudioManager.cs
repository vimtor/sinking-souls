using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance = null; // Singleton.

    [Header("Sound Lists")]
    public Sound[] effects;
    public Sound[] music;

    public enum SoundType {
        EFFECT, MUSIC
    };

    private void Awake() {

        #region SINGLETON
        if(instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        #endregion

        Array.ForEach(effects, sound => SetupSound(sound));
        Array.ForEach(music, sound => SetupSound(sound));

    }

    private void SetupSound(Sound sound) {

        sound.source = gameObject.AddComponent<AudioSource>();

        sound.source.clip = sound.clip;
        sound.source.volume = sound.volume;
        sound.source.pitch = sound.pitch;
        sound.source.loop = sound.loop;

    }

    public void ChangeVolume(float volume, SoundType type)
    {
        if(volume > 1 || volume < 0) Debug.LogError("Volume value is not valid.");

        switch (type) {
            case SoundType.EFFECT:
                Array.ForEach(effects, sound => sound.source.volume = volume);
                break;

            case SoundType.MUSIC:
                Array.ForEach(music, sound => sound.source.volume = volume);
                break;
        }

    }

    public void Play(string name)
    {
        Sound soundToPlay = Array.Find(effects, sound => sound.name == name);

        if (soundToPlay == null) {
            Debug.LogWarning("Sound" + name + " not found");
            return;
        }

        soundToPlay.source.Play();
    }

    public void PlayMusic(string name)
    {
        Sound soundToPlay = Array.Find(music, sound => sound.name == name);

        if (soundToPlay == null)
        {
            Debug.LogWarning("Sound" + name + " not found");
            return;
        }

        soundToPlay.source.Play();
    }


    public void Pause(string name) {

        Sound soundToPlay = Array.Find(effects, sound => sound.name == name);

        if (soundToPlay == null) {
            Debug.LogWarning("Sound" + name + " not found");
            return;
        }

        soundToPlay.source.Pause();
    }

    public void Stop(string name)
    {
        Sound soundToPlay = Array.Find(effects, sound => sound.name == name);

        if (soundToPlay == null)
        {
            Debug.LogWarning("Sound" + name + " not found");
            return;
        }

        soundToPlay.source.Stop();
    }
}
