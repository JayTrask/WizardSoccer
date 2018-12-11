using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalArea : MonoBehaviour {

    public GameObject Goal;
    public GameObject GameScore;

   // void Start()
   // {
   //     GameScore = GameObject.Find("ScoreBoard");
   // }

        void OnCollisionEnter(Collision collision) {
        if(GameScore == null)
        {
            GameScore = GameObject.FindGameObjectWithTag("scoreHUD");
        }
		if(collision.gameObject.tag == "Ball")
        {
            int playerID;
            if(Goal.tag == "Goal1") { playerID = 1; }
            else { playerID = 0; }
			GameScore.GetComponent<GameScore>().IncrementScore(playerID);

		//	GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		//	foreach (GameObject player in players)
		//	{
		//		player.GetComponent<NewScore>().CmdIncrementScore(playerID);
		//	}
        }
	}
}
