using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnLife : MonoBehaviour {

	public int totalTurns;
	int turnsLeft;
	// Use this for initialization
	void Start () {
		turnsLeft = totalTurns;
	}
	
	public void DecreaseLife()
	{
		turnsLeft -= 1;

		if (turnsLeft < 0)
		{
			Destroy(gameObject);
		}
	}

	
}
