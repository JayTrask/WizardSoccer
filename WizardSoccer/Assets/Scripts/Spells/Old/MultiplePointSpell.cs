using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MultiplePointSpell : GeneralSpell {

	List<Vector3> coordinates;

	int maxPointCount = 20;
	float minDragDistance = 0.15f;


	
	public override void Cast()
	{
		CmdCast();
	}

	[Command]
	void CmdCast()
	{
		Destroy(targetShow);

		foreach (Vector3 place in coordinates)
		{
			GameObject tempThing = Instantiate(Resources.Load("Models/Pillar") as GameObject);
			tempThing.transform.position = place;
			NetworkServer.SpawnWithClientAuthority(tempThing, player);

		}
	}


	public override void SaveCoordinates(Vector3 updatedCoordinates)
	{
		if ((updatedCoordinates - coordinates[coordinates.Count - 1]).magnitude > minDragDistance)
		{
			if (coordinates.Count > maxPointCount)
			{
				coordinates.RemoveAt(0);
			}
			
			coordinates.Add(updatedCoordinates);
			
		}
	}

	public override void InitializeCoordinates()
	{
		coordinates = new List<Vector3>();
		coordinates.Add(GetWorldPoint());
		targetShow.transform.position = GetWorldPoint();
		targetShow.GetComponent<MeshRenderer>().enabled = true;
	}

	public override void AnimatePlayer()
	{
		//animator.SetTrigger("Attack2");
		GetComponent<NetworkAnimator>().SetTrigger("Attack2");
		
	}


}
