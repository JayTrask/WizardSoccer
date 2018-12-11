using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIButtons : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		GetComponent<Button>().onClick.AddListener(WhenPressed);
	}
	
	// Update is called once per frame
	public virtual void WhenPressed () {

		//Add function for when button is pressed
	}
}
