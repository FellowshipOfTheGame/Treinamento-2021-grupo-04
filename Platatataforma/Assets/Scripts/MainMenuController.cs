using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1f;   
    }
    
    public void StartButton()
    {
        SceneManager.LoadScene("Round");
        
    }

    public void OptionsButton()
    {
        Debug.Log("Options selected");
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
