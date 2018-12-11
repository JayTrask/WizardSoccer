using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MonsterSpell : GenSpell {

	public string playerAnimation = "Attack2";

	public override void Cast()
	{
		AnimatePlayer(playerAnimation);
		CmdCast();
	}

	[Command]
	public void CmdCast()
	{
		Vector3 targetPos = GetComponent<TargetList>().targetPos[0];
		string whichMonster;

		if (gameObject.GetComponent<Player>().playerID == 0)
		{
			whichMonster = "StoneMonster";
		}else
		{
			whichMonster = "StoneMonster1";
		}

		GameObject tempMonster = Instantiate(Resources.Load("Models/" + whichMonster) as GameObject);
		tempMonster.transform.position = targetPos;
		tempMonster.GetComponent<Monster>().SetPlayerID(gameObject.GetComponent<Player>().playerID);
		NetworkServer.Spawn(tempMonster);
		
	}
}
