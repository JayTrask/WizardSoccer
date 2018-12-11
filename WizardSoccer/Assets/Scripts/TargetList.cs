using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TargetList : NetworkBehaviour {

	public List<Vector3> targetPos;

	int maxTargetCount = 10;
	
	public virtual void SetTarget(Vector3 newTarget)
	{
		CmdSetTarget(newTarget);
	}

	[Command]
	public virtual void CmdSetTarget(Vector3 newTarget)
	{
		//Execute on Server
		RpcTargetPos(newTarget);
	}

	[ClientRpc]
	public virtual void RpcTargetPos(Vector3 newTarget)
	{
		//Execute on each client
		if (targetPos.Count >= maxTargetCount)
		{
			targetPos.RemoveAt(0);
		}

		targetPos.Add(newTarget);
	}

	[Command]
	public void CmdClear()
	{
		RpcClearTargets();
	}

	[ClientRpc]
	public void RpcClearTargets()
	{
		targetPos = new List<Vector3>();
	}

	
}
