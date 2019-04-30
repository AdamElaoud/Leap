using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public static AudioManager instance;

	public Sound[] sounds;
    bool sloMo;
    float speedUp;

	void Awake() {
        // insure only 1 copy of AudioManager
        if (instance == null) {
		    instance = this;
            DontDestroyOnLoad(gameObject);

        } else {
            Destroy(gameObject);
            return;
        }		

		foreach (Sound s in sounds) {
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;
		}
	}

    void Start() {
        sloMo = false;
        speedUp = 0;
        Play("Leap_Theme");
    }

	public void Play(string soundName) {
		Sound sound = Array.Find(sounds, s => s.name == soundName);

		if (sound == null) {
			Debug.LogWarning("Sound: " + soundName + " not found!");
			return;
		}

		sound.source.volume = sound.volume;
		sound.source.pitch = sound.pitch;

		sound.source.Play();
	}

    public void SpeedUp(string soundName) {
        Sound sound = Array.Find(sounds, s => s.name == soundName);

        if (sound == null) {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }

        sound.pitch += 0.01f;
        speedUp += 0.01f;
        sound.source.pitch = sound.pitch;
    }

    public void ResetPitch(string soundName) {
        Sound sound = Array.Find(sounds, s => s.name == soundName);

        if (sound == null) {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }

        sound.pitch = 1;
        sound.source.pitch = sound.pitch;
    }

    public void SloMo() {
        if (sloMo) {
            foreach (Sound s in sounds) {
                // restore sound speed
                if (s.name.Equals("Leap_Theme"))
                    s.source.pitch = 1 + speedUp;
                else
                    s.source.pitch = 1;
            }

            sloMo = false;

        } else {
            foreach (Sound s in sounds) {
                s.source.pitch = 0.95f;
            }

            sloMo = true;
        }
        
    }

}
