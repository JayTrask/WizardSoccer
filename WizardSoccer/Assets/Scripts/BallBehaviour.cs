using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate()
	{
		if (GetComponent<Rigidbody>().velocity.magnitude < 0.1f)
		{
			GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Goal1" || collision.gameObject.tag == "Goal2")
		{
			GetComponent<Transform>().position = new Vector3(10f, 0.56f, 12.5f);
			GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
	}
}
