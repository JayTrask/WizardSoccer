using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameScore : MonoBehaviour {

    public GameObject player1;
    public GameObject player2;
    public GameObject GameOverPlayer1;
    public GameObject GameOverPlayer2;

    
    int player1_score = 0;
    int player2_score = 0;

	// Use this for initialization

    
    public void IncrementScore(int playerID)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameOverPlayer1 = players[0];
        GameOverPlayer2 = players[1];

        if(playerID == 0)
        {
            player1_score++;
        }

        if(playerID == 1)
        {
            player2_score++;
        }

        UpdateScoreBoard();

    }

    
    private void UpdateScoreBoard()
    {
        int gameStatus = isGameOver();
        if (gameStatus == -1)
        {
            player1.GetComponent<ScoreUpdate>().UpdateScore(player1_score);
            player2.GetComponent<ScoreUpdate>().UpdateScore(player2_score);// Update each scoreboard object with accurate player score
        }
        else
        {
            if(gameStatus == 0)
            {
                GameOverPlayer1.GetComponent<GameOver>().setGameOver(1);
                GameOverPlayer2.GetComponent<GameOver>().setGameOver(-1);
                
            }
            else
            {
                GameOverPlayer1.GetComponent<GameOver>().setGameOver(-1);
                GameOverPlayer2.GetComponent<GameOver>().setGameOver(1);
            }
        }
    }


/*
 * isGameOver() Checks to see if either player has reached the desired number of points required to win the game (i.e. 9).
 * The function returns an int with which player has won; if neither player has won, the function returns -1. This function
 * is called by the GameMaster object where it is used to determine and display the winner of the game.
 */
    public int isGameOver() 
    {
        if(player1_score == 10)
        {
            return 0;
        }

        if(player2_score == 10)
        {
            return 1;
        }

        else
        {
            return -1;
        }
    }

}
