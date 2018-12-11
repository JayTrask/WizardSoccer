using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewChanger : MonoBehaviour {

	public Material enemyGoal;
	public Material playerGoal;

	public void SetView(int playerID)
	{
		GameObject goal1 = GameObject.Find("Goal1");
		GameObject goal2 = GameObject.Find("Goal2");

		GameObject southWall = GameObject.Find("SouthWall");
		GameObject northWall = GameObject.Find("NorthWall");

		if (playerID == 0)
		{
			SetGoalView(goal1, false);
			southWall.GetComponent<MeshRenderer>().enabled = false;
			southWall.layer = 2;
		}
		else
		{
			SetGoalView(goal2, false);
			northWall.GetComponent<MeshRenderer>().enabled = false;
			northWall.layer = 2;
		}
		Destroy(gameObject);
	}

	void SetGoalView(GameObject goal, bool status)
	{

		foreach (Transform child in goal.transform)
		{
			if (child.name != "ScoreArea")
			{
				child.GetComponent<MeshRenderer>().enabled = status;

				if (status == false)
				{
					child.gameObject.layer = 2;
				}
			}
			else
			{
				if (status)
				{
					child.GetComponent<MeshRenderer>().material = enemyGoal;

				}
				else
				{
					Destroy(child.GetComponent<Animator>());
					child.GetComponent<MeshRenderer>().material = playerGoal;
				}
			}

		}
	}
}
