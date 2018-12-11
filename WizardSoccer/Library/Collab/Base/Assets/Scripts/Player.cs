using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class statusEffect
{
	public bool blnCursed;
	public bool blnHexed;
	public double accuracy=100;

}

public class Player : NetworkBehaviour {
	
	public GameObject playerCamera;

	public GameObject projectileSpawn;

	public GameMaster gameMaster;

	public GameObject spellButtons;

    public GameObject scoreBoard;

    public GameObject GameScore;

	public Spell theSpell;

	public statusEffect status;

    [SyncVar]
	public int playerID;

	private void Start()
	{
		status = new statusEffect();
		status.blnCursed = false;
		status.blnHexed = false;
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
		CastSpell();
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
					if (!tempButton.turnReady)
					{
						spellButton.GetComponent<Available>().DecreaseRecharge();
					}
				}
			}
		}
	}

	

	
}
