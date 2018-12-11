using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Available : MonoBehaviour {


	public int rechargeTime;
	public Text uiText;

	int remRecharge;
	

	public void StartRecharge()
	{
		remRecharge = rechargeTime;
		uiText.text = remRecharge.ToString();
		base.GetComponent<Button>().interactable = false;
		
	}

	public void DecreaseRecharge()
	{
		remRecharge -= 1;
		uiText.text = remRecharge.ToString();

		if (remRecharge == 0)
		{
			gameObject.GetComponent<SpellButton>().turnReady = true;
			uiText.text = "";
			base.GetComponent<Button>().interactable = true;
		}
		
	}

}
