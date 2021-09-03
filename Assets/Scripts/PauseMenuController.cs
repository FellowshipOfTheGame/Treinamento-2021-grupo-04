using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject PauseScreenObject;

    public void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            PauseScreenObject.SetActive(!PauseScreenObject.activeInHierarchy);
            if (PauseScreenObject.activeInHierarchy)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OptionsButton()
    {
        Debug.Log("Options selected");
    }
}
