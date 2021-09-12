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
    public SoundManager soundManager;
    public DeathZone deathZone;

    public void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            PauseScreenObject.SetActive(!PauseScreenObject.activeInHierarchy);
            if (PauseScreenObject.activeInHierarchy)
            {
                Time.timeScale = 0f;
                AudioSource[] foundAudioObjects = FindObjectsOfType<AudioSource>();
                soundManager.PauseAudioObjects();
            }
            else
            {
                Time.timeScale = 1f;
                soundManager.ResumeAudioObjects();
            }
        }



        if (deathZone.activateEndScreen)
        {
            EndScreenObject.SetActive(true);
            Time.timeScale = 0f;
                AudioSource[] foundAudioObjects = FindObjectsOfType<AudioSource>();
                soundManager.PauseAudioObjects();
            endText.text = deathZone.winner; 
            deathZone.activateEndScreen = false;
        }

    
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ContinueButton()
    {
        PauseScreenObject.SetActive(!PauseScreenObject.activeInHierarchy);
        if (PauseScreenObject.activeInHierarchy)
        {
            Time.timeScale = 0f;
            AudioSource[] foundAudioObjects = FindObjectsOfType<AudioSource>();
            soundManager.PauseAudioObjects();
        }
        else
        {
            Time.timeScale = 1f;
            soundManager.ResumeAudioObjects();
        }
    }
}
