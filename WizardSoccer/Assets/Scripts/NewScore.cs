using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NewScore : NetworkBehaviour {

	public GameScore gameScore;

	[Command]
	public void CmdIncrementScore(int playerID)
	{
		RpcLocalScore(playerID);
	}

	[ClientRpc]
	void RpcLocalScore(int playerID)
	{
		gameScore.IncrementScore(playerID);
	}


}
