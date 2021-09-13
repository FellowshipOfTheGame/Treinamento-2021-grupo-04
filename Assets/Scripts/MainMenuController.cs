using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public string sceneName;
    public GameObject menuOptions;
    public GameObject[] sliders;
    private GameObject[] Buttons;

    private void Start()
    {
        Buttons = GameObject.FindGameObjectsWithTag("Buttons");
        Time.timeScale = 1f;
    }

    public void StartButton()
    {
        SceneManager.LoadScene(sceneName);

    }

    public void OptionsButton()
    {
        foreach (GameObject btn in Buttons)
        {
            sliders[0].GetComponent<Slider>().value = PlayerPrefs.GetFloat("MusicVolume", .5f); //Update slide value when acessing the options.
            sliders[1].GetComponent<Slider>().value = PlayerPrefs.GetFloat("EffectsVolume", .5f);
            btn.SetActive(false);
            menuOptions.SetActive(true);
        }
    }

    public void SaveOptions()
    {
        foreach (GameObject btn in Buttons)
        {
            UpdateVolumeValue();
            btn.SetActive(true);
            menuOptions.SetActive(false);
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    void UpdateVolumeValue()
    {
        PlayerPrefs.SetFloat("MusicVolume", sliders[0].GetComponent<Slider>().value); //update pref volume when leaving options.
        PlayerPrefs.SetFloat("EffectsVolume", sliders[1].GetComponent<Slider>().value);
        SoundManager.instance.SetVolumeFromPlayerPrefs(); // update Audio Source volumes with new ones
    }
}

