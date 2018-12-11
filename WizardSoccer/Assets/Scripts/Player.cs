using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public struct statusEffect
{
	public bool blnCursed;
	public bool blnHexed;
	public double accuracy;
	public int curseDuration ;
	public int hexDuration ;

}

public class Player : NetworkBehaviour {
	
	public GameObject playerCamera;

	public GameObject projectileSpawn;

	public GameMaster gameMaster;

	public GameObject spellButtons;

    public GameObject scoreBoard;

    public GameObject GameScore;

	public GameObject mesh;

	public int value;

	public Spell theSpell;

	public statusEffect status;

    [SyncVar]
	public int playerID;

	private void Start()
	{
		status = new statusEffect();
		status.blnCursed = false;
		status.blnHexed = false;
		status.accuracy = 100;
		status.hexDuration = -1;
		status.curseDuration = -1;

        GameObject.FindGameObjectWithTag("Goal1").GetComponent<GoalArea>().GameScore = GameScore;
        GameObject.FindGameObjectWithTag("Goal2").GetComponent<GoalArea>().GameScore = GameScore;
		
		gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();        

        if (isLocalPlayer == true)
		{
			
			GameObject tempViewChange = Instantiate(Resources.Load("ViewChanger")as GameObject);
			
			tempViewChange.GetComponent<ViewChanger>().SetView(playerID);

			int spellCount = 0;

			foreach (Transform item in spellButtons.transform)
			{
				SpellButton tempSpellButton = item.GetComponent<SpellButton>();

				if (tempSpellButton != null)
				{
					tempSpellButton.player = gameObject;
					tempSpellButton.gameMaster = gameMaster;

					switch(spellCount)
					{
						case 0:
							tempSpellButton.spell = GetComponent<ProjectileSpell>();
							break;

						case 1:
							tempSpellButton.spell = GetComponent<WallSpell>();
							break;

						case 2:
							tempSpellButton.spell = GetComponent<TornadoSpell>();
							break;

						case 3:
							tempSpellButton.spell = GetComponent<HexSpell>();
							break;

						case 4:
							tempSpellButton.spell = GetComponent<CurseSpell>();
							break;

						case 5:
							tempSpellButton.spell = GetComponent<MonsterSpell>();
							break;

						default:
							break;
					}

					spellCount += 1;

				}
				
			}

			playerCamera.SetActive(true);
		}
		else
		{
			playerCamera.SetActive(false);
		}
	}

	[ClientRpc]
	public void RpcCastIt()
	{
		if (!status.blnHexed)
		{
			CastSpell();
		}
	}

	public void CastSpell()
	{
		if (theSpell != null)
		{
			theSpell.Cast();
		}
	}


	public void SetReady()
	{
		CmdSetReady();
	}
	
	[Command]
	void CmdSetReady()
	{
		gameMaster.PlayerReady(playerID);
	}

	[ClientRpc]
	void RpcUpdateReady()
	{
		gameMaster.PlayerReady(playerID);
	}

	[ClientRpc]
	public void RpcUpdateButtons()
	{
		if (spellButtons.activeSelf)
		{
			foreach (Transform spellButton in spellButtons.transform)
			{
				SpellButton tempButton = spellButton.GetComponent<SpellButton>();

				if (tempButton != null)
				{
					if (!tempButton.turnReady && !status.blnHexed)
					{
						spellButton.GetComponent<Available>().DecreaseRecharge();
					}
				}
			}
		}
	}

	[ClientRpc]
	public void RpcStatusUpdate()
	{
		status.hexDuration -= 1;
		if (status.blnHexed)
		{
			print("Status Value : " + status.hexDuration);
			disableSpells();

			if (status.hexDuration < 0)
			{
				status.blnHexed = false;
				status.hexDuration = 0;
				mesh.SetActive(true);
				GetComponent<Collider>().enabled = true;
				enableSpells();
				CmdUpdateStatus(playerID);
				CmdSetNotReady(playerID);
			}
			
			
		}

		if (status.blnCursed)
		{
			status.curseDuration -= 1;
			if (status.curseDuration <= 0)
			{
				status.blnCursed = false;
			}
		}
		
		status.accuracy = 100;
	}

	
	public void disableSpells()
	{
		foreach (Transform spellButton in spellButtons.transform)
		{
			SpellButton tempButton = spellButton.GetComponent<SpellButton>();
			if (tempButton != null)
			{
				tempButton.turnReady = false;
			}
		}
	}

	public void enableSpells()
	{
		foreach (Transform spellButton in spellButtons.transform)
		{
			SpellButton tempButton = spellButton.GetComponent<SpellButton>();
			if (tempButton != null)
			{
				tempButton.turnReady = true;
			}
		}
	}

	[Command]
	public void CmdUpdateStatus(int playerid)
	{
		gameMaster.playerStatusChange(playerid, status);
	}

	[Command]
	public void CmdSetNotReady(int playerid)
	{
		gameMaster.PlayerNotReady(playerid);
	}

}
