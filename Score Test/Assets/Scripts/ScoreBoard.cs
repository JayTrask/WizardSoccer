using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoard : MonoBehaviour {

    public GameObject Score1;
    public GameObject Score2;

    int player1_score = 0;
    int player2_score = 0;

    public void IncrementScore(int playerID)
    {
        if (playerID == 0)
        {
            player1_score++;
        }

        if (playerID == 1)
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
            Score1.GetComponent<ScoreUp>().UpdateScore(player1_score);
            Score2.GetComponent<ScoreUp>().UpdateScore(player2_score);// Update each scoreboard object with accurate player score
        }
        else
        {
            if (gameStatus == 0)
            {
                return;
                //TODO: Make winning function for player1
            }
            else
            {
                return;
                //TODO: Make winning function for player2
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
        if (player1_score == 10)
        {
            return 0;
        }

        if (player2_score == 10)
        {
            return 1;
        }

        else
        {
            return -1;
        }
    }

}
