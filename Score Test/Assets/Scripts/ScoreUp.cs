using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUp : MonoBehaviour {

    public GameObject Score;


    public void UpdateScore(int score)
    {
        Score.GetComponent<Image>().sprite = Resources.Load<Sprite>("ScoreBoard1");
    }
}
