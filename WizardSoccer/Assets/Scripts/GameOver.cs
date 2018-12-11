using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

	
	// Sets the sprite image for the game over screen depending on whether or not the player has won or lost. The function will be passed 1 
    // if the player has won, and -1 if they player has lost. 
	public void setGameOver(int status) {
		if(status == 1)
        {
            gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Scoring/Winner.png");
        }

        if(status == -1)
        {
            gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Scoring/Loser.png");
        }
	}
}
