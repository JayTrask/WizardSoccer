using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GenSpell : NetworkBehaviour, Spell {

	int maxTargetCount = 10;

	public virtual void Cast()
	{
		print("Cast spell");
	}

	public void AnimatePlayer(string playerAnimation)
	{
		GetComponent<NetworkAnimator>().SetTrigger(playerAnimation);
	}

	
	
}
