using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSpawner : NetworkManager {

	public bool host = false;
	int playerCount = 0;
	public GameObject[] spawnPositions;

	public Material enemyGoal;
	public Material playerGoal;
	
	public GUISkin skin;
	public bool showMatches = false;
	public GameObject LobbyManagerGameObject;
	
	private int ypos = 10;
	
	public void StartHost()
	{
		host = true;
		base.StartHost();
		GameObject.Find("LobbyManager").GetComponent<Canvas>().enabled = true;
	}
	
	public void StartClient()
	{
		host = false;
		base.StartClient();
		GameObject.Find("LobbyManager").GetComponent<Canvas>().enabled = true;
	}
	
	public void InternetHost()
	{
		host = true;
		showMatches = false;
		base.matchMaker.CreateMatch(base.matchName, 2, true, "", "", "", 0, 0, base.OnMatchCreate);
		GameObject.Find("LobbyManager").GetComponent<Canvas>().enabled = true;
	}
	
	public void InternetClient()
	{
		host = false;
		base.matchMaker.ListMatches(0, 20, "", true, 0, 0, base.OnMatchList);
		showMatches = true;
	}
	
	public void OnMatchNameChanged(string newName)
	{
		base.matchName = newName;
	}
	
	public void gameShutdown()
	{
		if(host)
		{
			base.StopHost();
		}
		else
		{
			base.StopClient();
		}
		Destroy(LobbyManagerGameObject);
		PlayerSpawner.Shutdown();
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		spawnPositions = GameObject.FindGameObjectsWithTag("SpawnPosition");
		
		GameObject player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
		
		player.GetComponent<Player>().playerID = playerCount;

		player.transform.position = spawnPositions[playerCount].transform.position;
		player.transform.rotation = spawnPositions[playerCount].transform.rotation;
		player.GetComponent<Collider>().transform.position = player.transform.position;
		player.GetComponent<Collider>().transform.rotation = player.transform.rotation;


		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

		playerCount += 1;
	}

	void SetGoal(bool status, GameObject[] goalComponents)
	{
		foreach (GameObject component in goalComponents)
		{
			component.GetComponent<MeshRenderer>().enabled = status;

			if (!status)
			{
				component.layer = Physics.IgnoreRaycastLayer;
			}
		}
	}
	
	void OnGUI()
	{
		ypos = 250;
		if(base.matchInfo == null)
		{
			if(base.matches != null && showMatches)
			{
				foreach (var match in base.matches)
				{
		 
					if (GUI.Button(new Rect(300, ypos, 130, 35), "Join Match: " + match.name, skin.button))
					{
			 
						 base.matchName = match.name;
				 
						 base.matchSize = (uint)match.currentSize;
				 
						 base.matchMaker.JoinMatch(match.networkId, "" , "", "", 0, 0, base.OnMatchJoined);
						 
						 showMatches = false;
						 
						 host = false;
						 
						 GameObject.Find("LobbyManager").GetComponent<Canvas>().enabled = true;
					}
					ypos+=40	;
				}
			}
		}
	}
}
