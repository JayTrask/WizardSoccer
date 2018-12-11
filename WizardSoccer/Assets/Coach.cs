using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Coach : MonoBehaviour {

	public Transform ball;
	Transform enemyGoal;
	Transform ownGoal;

	public int playerID;

	public List<Monster> players = new List<Monster>();
	public List<GameObject> enemies = new List<GameObject>();

	public string boardState;

	List<Vector3> offensePositions = new List<Vector3>();
	List<Vector3> defensePositions = new List<Vector3>();
	

	// Use this for initialization
	void Start () {
		SetGoals();
		SetPositions();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void DetermineMoves()
	{
		List<float> enemyBallDist = EnemyBallDist();
		List<float> playerBallDist = PlayersBallDist();
		

		if (enemyBallDist.Count > 0 && playerBallDist.Count > 0)
		{
			if (enemyBallDist.Min() < playerBallDist.Min())
			{
				DefenseMoves(enemyBallDist, playerBallDist);
			}
			else
			{
				OffenseMoves(enemyBallDist, playerBallDist);
			}
		}else
		{
			if (playerBallDist.Count > 0)
			{
				enemyBallDist.Add(1000.0f);
				OffenseMoves(enemyBallDist, playerBallDist);
			}
		}


	}

	public void OrderPlayers()
	{
		FindMonsters();
		DetermineMoves();
	}

	void DefenseMoves(List<float> enemyBallDist, List<float> playerBallDist)
	{
		foreach (Monster player in players)
		{
			List<float> tempDist = DistToEnemies(player.transform);

			int whichBlock = tempDist.IndexOf(tempDist.Min());

			player.NewOrders(enemies[whichBlock].transform, "block");
		}
	}

	void OffenseMoves(List<float> enemyBallDist, List<float> playerBallDist)
	{

		Transform tempPos = new GameObject().transform;

		List<float> tempPlayerDist = new List<float>();

		foreach(float entry in playerBallDist)
		{
			tempPlayerDist.Add(entry);
		}



		int extraStart = 0;

		if (Vector3.Distance(enemyGoal.position, ball.position) < Vector3.Distance(ownGoal.position, ball.position))
		{
			//closer to enemy goal

			if (players.Count > 1)
			{
				if (Vector3.Distance(ball.position, enemyGoal.position) < 6.0f)
				{
					players[playerBallDist.IndexOf(playerBallDist.Min())].NewOrders(ball.transform, "shoot");
					extraStart += 1;

				}else
				{
					int oldMin = playerBallDist.IndexOf(tempPlayerDist.Min());
					tempPlayerDist.RemoveAt(tempPlayerDist.IndexOf(tempPlayerDist.Min()));
					print(playerBallDist.Count);

					int newMin = playerBallDist.IndexOf(tempPlayerDist.Min());
					int newNewMin = tempPlayerDist.IndexOf(tempPlayerDist.Min());
					
					if (tempPlayerDist.Min() < enemyBallDist.Min())
					{
						
						tempPos.position = offensePositions[0];
						players[oldMin].NewOrders(tempPos, "move");
						players[newMin].SetAlly(players[oldMin].transform);
						players[newMin].NewOrders(ball.transform, "pass");

						tempPlayerDist.RemoveAt(newNewMin);

						extraStart += 2;

					}

					
				}
			}

			


		}
		else
		{
			//closer to own goal

		}
	}

	void FindMonsters()
	{
		GameObject[] monsters = GameObject.FindGameObjectsWithTag("monster");

		players.Clear();
		enemies.Clear();

		foreach(GameObject monster in monsters)
		{
			if (monster.GetComponent<Monster>().playerID == playerID)
			{
				players.Add(monster.GetComponent<Monster>());
			}
			else
			{
				enemies.Add(monster);
			}
		}
	}

	public void SetGoals()
	{
		if (playerID == 0)
		{
			ownGoal = GameObject.Find("Goal1").transform;
			enemyGoal = GameObject.Find("Goal2").transform;
		}
		else
		{
			ownGoal = GameObject.Find("Goal2").transform;
			enemyGoal = GameObject.Find("Goal1").transform;
		}
	}

	List<float> DistToEnemies(Transform whichPlayer)
	{
		List<float> distToEnemies = new List<float>();

		foreach (GameObject enemy in enemies)
		{
			distToEnemies.Add(Vector3.Distance(enemy.transform.position, whichPlayer.position));
		}

		return distToEnemies;
	}


	List<float> EnemyBallDist()
	{
		List<float> enemyDistances = new List<float>();

		foreach (GameObject enemy in enemies)
		{
			enemyDistances.Add(Vector3.Distance(enemy.transform.position, ball.position));
		}

		return enemyDistances;
	}

	List<float> PlayersBallDist()
	{
		List<float> playerDistances = new List<float>();

		foreach (Monster player in players)
		{
			playerDistances.Add(Vector3.Distance(player.transform.position, ball.position));
		}

		return playerDistances;
	}


	void SetPositions()
	{
		float dirMult = 1.0f;

		if (playerID == 0)
		{
			dirMult = -1.0f;
		}
		offensePositions = new List<Vector3>();
		defensePositions = new List<Vector3>();

		Vector3 enemyGoalPos = enemyGoal.position;
		Vector3 ownGoalPos = ownGoal.position;

		offensePositions.Add(enemyGoalPos += new Vector3(-1.0f, 0.0f, dirMult * 5.0f));
		offensePositions.Add(offensePositions[0] += new Vector3(4.0f, 0.0f, 0.0f));

		dirMult = 1.0f;

		if (playerID == 1)
		{
			dirMult = -1.0f;
		}

		defensePositions.Add(ownGoalPos += new Vector3(-1.0f, 0.0f, dirMult * 5.0f));
		defensePositions.Add(ownGoalPos += new Vector3(4.0f, 0.0f, 0.0f));

		


	}
	

	


}
