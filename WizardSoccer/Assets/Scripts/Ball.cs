using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public int playerID;

	void FixedUpdate()
	{
		if (GetComponent<Rigidbody>().velocity.magnitude < 0.1f)
		{
			GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
	}

    private void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;
        var goal = hit.GetComponent<Goal>();

        if (goal != null)
        {
            goal.IncrementScore();
        }
    }
}
