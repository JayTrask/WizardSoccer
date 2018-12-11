using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aimer : MonoBehaviour {

	public Spell spell;
	public TargetList targetList;
	public Camera cam;

	public bool startFollowing;

	Vector3 lastPoint;

	float minDragDistance = 0.25f;

	public GameMaster gameMaster;

	public Player player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (startFollowing)
		{
			Aiming();

			if (Input.GetMouseButtonUp(0))
			{
				//spell.Cast();
				
				player.GetComponent<Player>().SetReady();
				player.GetComponent<Player>().theSpell = spell;
				
				Destroy(gameObject);
			}
		}
		
	}

	public virtual void Aiming()
	{
		Vector3 tempCoordinates = GetWorldPoint();
		transform.position = tempCoordinates;

		if ((tempCoordinates - lastPoint).magnitude > minDragDistance)
		{
			targetList.SetTarget(transform.position);
			lastPoint = transform.position;
		}

		ModifyAccuracy();
	}

	public void ModifyAccuracy()
	{
		//print("aimer stattus " + player.GetComponent<Player>().status.blnCursed);
		double accuracy = player.GetComponent<Player>().status.accuracy;
		if (player.GetComponent<Player>().status.blnCursed)
		{
			accuracy = accuracy * 0.2;
		}
		for (int i = 0; i < targetList.targetPos.Count; i++)
		{
			//float radiusMultiplier = (float)((100 - accuracy) / accuracy);	
			float radiusMultiplier = (float)((1 / (accuracy / 100.0)) - 1);
			//print("Radius Multiplier : "+ radiusMultiplier);
			targetList.targetPos[i] += Random.insideUnitSphere*radiusMultiplier;	
		}
		
	}

	public Vector3 GetWorldPoint()
	{
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			return hit.point;
		}
		else
		{
			return Vector3.zero;
		}
	}



}
