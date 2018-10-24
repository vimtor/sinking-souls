using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance = null; // Singleton.

    public Sound[] sounds;

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

        foreach(Sound sound in sounds) {
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    public void Play(string name) {
        Sound soundToPlay = Array.Find(sounds, sound => sound.name == name);

        if(soundToPlay == null) {
            Debug.LogWarning("Sound" + name + "not found");
            return;
        }

        soundToPlay.source.Play();
    }

}
