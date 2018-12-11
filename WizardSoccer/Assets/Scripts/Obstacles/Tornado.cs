using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour {

    public Vector3 windDirection;

    public void getWindDirection(WindSpell windspell)
    {
        windDirection = windspell.GetWindDirection();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != "Wind")
        {
            WindForce tempWindForce = other.gameObject.AddComponent<WindForce>();
            tempWindForce.setForce(windDirection);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
