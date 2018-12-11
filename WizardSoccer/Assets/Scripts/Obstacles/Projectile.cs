using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Projectile : NetworkBehaviour {

	public Vector3 startingVelocity;

	bool destroy = false;

	public GameObject explosion;
	public GameObject afterExplosion;

	private void OnCollisionEnter(Collision collision)
	{
		destroy = true;
		CmdOnImpact();
	}

	private void LateUpdate()
	{
		if (destroy)
		{
			Destroy(gameObject);
		}
	}

	public virtual void Launch()
	{
		GetComponent<Rigidbody>().velocity = startingVelocity;
	}

	[Command]
	public virtual void CmdOnImpact()
	{
		if (explosion != null)
		{
			GameObject tempExplosion = Instantiate(explosion);
			tempExplosion.transform.position = transform.position;
			NetworkServer.Spawn(tempExplosion);
		}

		if (afterExplosion != null)
		{
			GameObject tempAfterExplosion = Instantiate(afterExplosion);
			tempAfterExplosion.transform.position = transform.position;
			NetworkServer.Spawn(tempAfterExplosion);
		}


	}

	



}
