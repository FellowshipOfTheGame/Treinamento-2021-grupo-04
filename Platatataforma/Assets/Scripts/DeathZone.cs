using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    bool gameOver = false;

    private void Update()
    {
        if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player1") || collider.CompareTag("Player2"))
        {
            if (!gameOver)
            {
                if (collider.CompareTag("Player1"))
                    Debug.Log("Player 2 won the match!!!" + "  --> Press R to restart");
                else if (collider.CompareTag("Player2"))
                    Debug.Log("Player 1 won the match!!!" + "  --> Press R to restart");
            }

            Destroy(collider.gameObject);
            gameOver = true;
        }
    }
}
