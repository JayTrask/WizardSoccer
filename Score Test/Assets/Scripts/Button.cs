using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

    public GameObject ScoreBoard;

    
	// Update is called once per frame
	public void updateScore() {
        ScoreBoard.GetComponent<ScoreBoard>().IncrementScore(0);
	}
}
