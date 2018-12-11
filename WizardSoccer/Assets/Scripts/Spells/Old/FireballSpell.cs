using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FireballSpell : GeneralSpell {

	float shotSpeed = 30.0f;
	

		
	public override void Cast()
	{		
		CmdCast();
	}

	[Command]
	void CmdCast()
	{
		print("SDLKFJDSLKFJ");
		Vector3 shotTrajectory = GetProjectileDirection(spawnPos);
		GameObject tempFireball = Instantiate(Resources.Load("Models/Projectile") as GameObject);
		

		tempFireball.transform.position = spawnPos;
		Projectile tempProjectile = tempFireball.GetComponent<Projectile>();
		
		tempProjectile.startingVelocity = shotTrajectory * shotSpeed;
		tempProjectile.explosion = Resources.Load("Particle Effects/Explosion") as GameObject;
		tempProjectile.afterExplosion = Resources.Load("Particle Effects/Fire") as GameObject;

		NetworkServer.SpawnWithClientAuthority(tempFireball, connectionToClient);
		//tempProjectile.CmdLaunch();
		
	}

	
}
