using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StatusEffect : NetworkBehaviour {

	public GameObject mesh;
	public Player player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision)
	{
		
		switch (collision.gameObject.tag)
		{
			case "HexProjectile":
				CmdHexEffect();
				break;
			case "CurseProjectile":
				CmdCurseEffect();
				break;
		}
	}

	[Command]
	void CmdCurseEffect()
	{
		RpcCurseEffect();

	}

	[ClientRpc]
	void RpcCurseEffect()
	{
		GetComponent<Player>().status.blnCursed = true;
		GetComponent<Player>().status.curseDuration = 1;
	}


	[Command]
	void CmdHexEffect()
	{
		Transform playerPos = transform;
		string hexCritterModel = "Chicken";
		GameObject hexCritter = (GameObject)Instantiate(Resources.Load("Models/" + hexCritterModel));
		hexCritter.transform.position = playerPos.position;
		hexCritter.transform.rotation = playerPos.rotation;
		NetworkServer.Spawn(hexCritter);
		RpcHexEffect();
		

	}

	[ClientRpc]
	void RpcHexEffect()
	{
		mesh.SetActive(false);
		GetComponent<Collider>().enabled = false;
		GetComponent<Player>().status.blnHexed = true;
		GetComponent<Player>().status.hexDuration = 2;
		GetComponent<Player>().SetReady();
	}

}
