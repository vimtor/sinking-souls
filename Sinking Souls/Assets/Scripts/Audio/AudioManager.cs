using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance = null; // Singleton.

    public Sound[] effects;
    public Sound[] music;

    private void Awake() {

        #region SINGLETON
        if(instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }
        #endregion

        DontDestroyOnLoad(gameObject);

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

    public void Play(string name) {
        Sound soundToPlay = Array.Find(effects, sound => sound.name == name);

        if (soundToPlay == null) {
            Debug.LogWarning("Sound" + name + "not found");
            return;
        }

        soundToPlay.source.Play();
    }

}
