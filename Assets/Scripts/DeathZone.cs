using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    bool gameOver = false;
    public bool activateEndScreen;
    public string winner;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player1") || collider.CompareTag("Player2"))
        {
            if (!gameOver)
            {
                activateEndScreen = true;
                if (collider.CompareTag("Player1"))
                    winner = "Player 2 won the match!!!";
                else if (collider.CompareTag("Player2"))
                    winner = "Player 1 won the match!!!";
            }

            Destroy(collider.gameObject);
            gameOver = true;
        }
    }
}
