using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SpellButton : GUIButtons {

	public GameObject player;

	public Spell spell;

	public bool turnReady;

	public GameMaster gameMaster;


	public void Start()
	{
		turnReady = true;
		GetComponent<Button>().onClick.AddListener(WhenPressed);
	}


	public override void WhenPressed()
	{
		if (turnReady && gameMaster.spellButtonsEnabled)
		{
            /* OLD CODE (working)
			Aimer aimer = Instantiate(Resources.Load("Aimer") as GameObject).GetComponent<Aimer>();
			aimer.camera = player.GetComponent<Player>().playerCamera.GetComponent<Camera>();
			aimer.spell = spell;
			aimer.gameMaster = gameMaster;
			aimer.player = player.GetComponent<Player>();
			aimer.targetList = player.GetComponent<TargetList>();
			player.GetComponent<TargetList>().CmdClear();
			aimer.startFollowing = true;
			turnReady = false;
			gameObject.GetComponent<Available>().StartRecharge();
            */

            // NEW CODE

            gameMaster.spellButtonsEnabled = false;

            TracerGM tracerGM = (Instantiate(Resources.Load("TracerGM")) as GameObject).GetComponent<TracerGM>();
            tracerGM.spell = spell;
            tracerGM.gameMaster = gameMaster;
            tracerGM.player = player;

            turnReady = false;
            gameObject.GetComponent<Available>().StartRecharge();

            //
        }
	}

}
