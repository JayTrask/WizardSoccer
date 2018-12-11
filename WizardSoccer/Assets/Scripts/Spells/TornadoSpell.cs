using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TornadoSpell : GenSpell {

	string wallObject = "WindSpell";
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

		Vector3 direction = targetPos[targetPos.Count - 1] - targetPos[0];
		direction = direction / direction.magnitude;

		foreach (Vector3 place in targetPos)
		{
			GameObject tempWall = Instantiate(Resources.Load("Particle Effects/" + wallObject) as GameObject);
			tempWall.transform.position = place;
			tempWall.AddComponent<Tornado>();
			tempWall.GetComponent<Tornado>().windDirection = direction;
			NetworkServer.Spawn(tempWall);
		}
	}
}
