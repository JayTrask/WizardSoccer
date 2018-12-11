using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    public int playerID;

    public void IncrementScore()
    {
        GetComponent<GameScore>().IncrementScore(playerID);
    }
}
