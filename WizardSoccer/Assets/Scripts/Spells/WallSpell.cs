using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WallSpell : GenSpell {

	string wallObject = "Pillar";
	public string playerAnimation = "Attack2";

	public override void Cast()
	{
		AnimatePlayer(playerAnimation);
		CmdCast();
	}

	[Command]
	public void CmdCast()
	{
		List<Vector3> targetPos = GetComponent<TargetList>().targetPos;

		foreach (Vector3 place in targetPos)
		{
			GameObject tempWall = Instantiate(Resources.Load("Models/" + wallObject) as GameObject);
			tempWall.transform.position = place;
			NetworkServer.Spawn(tempWall);
		}
	}

}
