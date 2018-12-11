using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ScoreUpdate : MonoBehaviour {

    public GameObject Score;

    
	public void UpdateScore(int score) {
		Score.GetComponent<Image>().sprite = Resources.Load<Sprite>("Scoring/" + score.ToString());
    }
}
