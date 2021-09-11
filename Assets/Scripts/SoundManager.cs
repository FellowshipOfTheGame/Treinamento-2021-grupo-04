using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
        AudioSource[] foundAudioObjects = FindObjectsOfType<AudioSource>();
            foreach (AudioSource AO in foundAudioObjects)
            {
                if (AO.name != "SoundManager")
                {
                    AO.volume = PlayerPrefs.GetFloat("EffectsVolume");
                }
            }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseAudioObjects()
    {
        AudioSource[] foundAudioObjects = FindObjectsOfType<AudioSource>();
            foreach (AudioSource AO in foundAudioObjects)
            {
                AO.Pause();
            }
    }

    public void ResumeAudioObjects()
    {
        AudioSource[] foundAudioObjects = FindObjectsOfType<AudioSource>();
            foreach (AudioSource AO in foundAudioObjects)
            {
                AO.Play();
            }
    }
}

