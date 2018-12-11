using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetScore : NetworkBehaviour {
	
	//Pointers to the two score displays
	public GameObject player_1_score_disp;
	public GameObject player_2_score_disp;
	private GameObject player_1;
	private GameObject player_2;

	//Player 1's score
	[SyncVar(hook = "p1_score_change")]
	public int p1_score;
	
	//Player 2's score
	[SyncVar(hook = "p2_score_change")]
	public int p2_score;

	
	//Will be called by GoalArea, and will only run on the server
	[ServerCallback]
	public void IncrementScore(int playerID)
    {
        if(playerID == 0)
        {
            p1_score++;
        }

        if(playerID == 1)
        {
            p2_score++;
        }
    }
	
	//Called on client and server when p1_score changes
	void p1_score_change(int newScore)
	{
		Debug.Log("Player 1 Scored. New score is " + newScore);
		
		if(newScore > 2)
		{
			HostGameOver();
			ClientGameOver();
		}
		else
		{
			player_1_score_disp.GetComponent<ScoreUpdate>().UpdateScore(newScore);
		}		
		
	}
	
	//Called on client and server when p2_score changes
	void p2_score_change(int newScore)
	{
		Debug.Log("Player 2 Scored. New score is " + newScore);
		
		if(newScore > 9)
		{
			HostGameOver();
			ClientGameOver();
		}
		else
		{
			player_2_score_disp.GetComponent<ScoreUpdate>().UpdateScore(newScore);
		}		
		
	}
	
	[Server]
	void HostGameOver()
	{
		GameObject.FindGameObjectsWithTag("Player")[0].transform.Find("MainCamera/Canvas/GameOver").gameObject.SetActive(true);
		GameObject.FindGameObjectsWithTag("Player")[0].transform.Find("MainCamera/Canvas/SpellButtons").gameObject.SetActive(false);
	}
	
	[Client]
	void ClientGameOver(){
		GameObject.FindGameObjectsWithTag("Player")[1].transform.Find("MainCamera/Canvas/GameOver").gameObject.SetActive(true);
		GameObject.FindGameObjectsWithTag("Player")[1].transform.Find("MainCamera/Canvas/SpellButtons").gameObject.SetActive(false);
	}
	
	void Update(){}
}