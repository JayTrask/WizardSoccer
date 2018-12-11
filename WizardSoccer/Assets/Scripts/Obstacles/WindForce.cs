using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindForce : MonoBehaviour {

	public Vector3 force;

    public void setForce(Vector3 newForce)
    {
        force = newForce;
    }
	private void FixedUpdate()
	{
		gameObject.GetComponent<Rigidbody>().AddForce(force);
	}
}
