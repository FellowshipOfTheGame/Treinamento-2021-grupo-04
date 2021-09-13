using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Singleton Pattern
    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            // gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
    #endregion

    public static AudioClip hit, jump, shoot, walk, ammo;

    [SerializeField] float hitVolScale, jumpVolScale, shootVolScale, walkVolScale, ammoVolScale;

    // referenciar componentes pelo Editor
    [SerializeField] AudioSource musicAudioSource, effectsAudioSource;

    [SerializeField] [Range(0, 1)] float reducedVolumeInPauseMenu = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        SetVolumeFromPlayerPrefs(); // inicializa volumes com 

        // Load sound effects
        hit = Resources.Load<AudioClip>("Hit");
        jump = Resources.Load<AudioClip>("Jump");
        shoot = Resources.Load<AudioClip>("Shoot");
        walk = Resources.Load<AudioClip>("Walk");
        ammo = Resources.Load<AudioClip>("Ammo");
    }

    public void SetVolumeFromPlayerPrefs()
    {
        musicAudioSource.volume = PlayerPrefs.GetFloat("MusicVolume", .5f);
        effectsAudioSource.volume = PlayerPrefs.GetFloat("EffectsVolume", .5f);
    }

    public void ReduceAudioVolume()
    {
        // musicAudioSource.Pause();

        // ao invés de pausar, diminuir som quando tela de Pause é ativada
        musicAudioSource.volume *= reducedVolumeInPauseMenu;
        effectsAudioSource.volume *= reducedVolumeInPauseMenu;
    }

    public void ResumeAudioVolume()
    {
        // musicAudioSource.Play();

        // retornar volume ao padrão do player prefs
        SetVolumeFromPlayerPrefs();
    }

    public void PlaySoundEffects(string clip)
    {
        switch (clip)
        {
            case "hit":
                effectsAudioSource.PlayOneShot(hit, hitVolScale);
                break;
            case "jump":
                effectsAudioSource.PlayOneShot(jump, jumpVolScale);
                break;
            case "walk":
                effectsAudioSource.PlayOneShot(walk, walkVolScale);
                break;
            case "shoot":
                effectsAudioSource.PlayOneShot(shoot, shootVolScale);
                break;
            case "ammo":
                effectsAudioSource.PlayOneShot(ammo, ammoVolScale);
                break;
            default:
                break;
        }
    }
}

