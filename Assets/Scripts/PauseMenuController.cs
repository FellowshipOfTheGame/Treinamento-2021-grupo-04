using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public GameObject PauseScreenObject;
    public GameObject EndScreenObject;
    public Text endText;
    public string sceneName;
    public DeathZone deathZone;

    public void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            PauseScreenObject.SetActive(!PauseScreenObject.activeInHierarchy);
            if (PauseScreenObject.activeInHierarchy)
            {
                Time.timeScale = 0f;
                SoundManager.instance.ReduceAudioVolume();
            }
            else
            {
                Time.timeScale = 1f;
                SoundManager.instance.ResumeAudioVolume();
            }
        }



        if (deathZone.activateEndScreen)
        {
            EndScreenObject.SetActive(true);
            Time.timeScale = 0f;
            SoundManager.instance.ReduceAudioVolume();
            endText.text = deathZone.winner; 
            deathZone.activateEndScreen = false;
        }

    
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
        SoundManager.instance.ResumeAudioVolume();
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(sceneName);
        SoundManager.instance.ResumeAudioVolume();
    }

    public void ContinueButton()
    {
        PauseScreenObject.SetActive(!PauseScreenObject.activeInHierarchy);
        if (PauseScreenObject.activeInHierarchy)
        {
            Time.timeScale = 0f;
            AudioSource[] foundAudioObjects = FindObjectsOfType<AudioSource>();
            SoundManager.instance.ReduceAudioVolume();
        }
        else
        {
            Time.timeScale = 1f;
            SoundManager.instance.ResumeAudioVolume();
        }
    }
}
