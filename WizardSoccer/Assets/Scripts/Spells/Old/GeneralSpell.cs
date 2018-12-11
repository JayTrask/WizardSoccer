using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GeneralSpell : NetworkBehaviour
{
	public Camera camera;
	RaycastHit hit;
	
	//represents the state of the spell, varies from idle-> aiming -> cast -> idle
	GameMaster gameMaster;
	
	public string state = "ready";

	//Coordinates corresponding to the position clicked by the player  the difference between this and the projectile position gives the 
	//vector correspoiding to movement of the ball
	public Vector3 coordinates;

	//This is the model for the sphere
	public GameObject targetShow;

	//this is the animator function
	public Animator animator;

	float castDelay = 0.75f;

	float castTime;

	public GameObject player;

	public Vector3 spawnPos;

	int playerID;

	void Start()
	{
		//gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();
		targetShow = Instantiate(Resources.Load("Models/Sphere") as GameObject);
		targetShow.GetComponent<MeshRenderer>().enabled = false;
		//camera = GameObject.Find("MainCamera").GetComponent<Camera>();
		//animator = GameObject.Find("Player1").GetComponent<Animator>();
	}




	void Update()
	{
		if (isLocalPlayer)
		{
			switch (state)
			{

				case "casting":

					if (Time.time > castTime)
					{
						SpellCast();
					}

					break;

				case "aiming":

					Aim();

					if (Input.GetMouseButtonUp(0))
					{
						SetState("casting");
						//gameMaster.PlayerReady(playerID, this);
					}

					break;

				case "ready":

					if (Input.GetMouseButtonDown(0))
					{
						SetState("aiming");
						InitializeCoordinates();
					}

					break;

				case "waiting to cast":

					break;
			}
		}
		
	}


	void SetCastTime()
	{
		castTime = Time.time + castDelay;
	}


	public void Aim()
	{
		if (!isServer)
		{
			Vector3 tempCoordinates = GetWorldPoint();
			targetShow.transform.position = tempCoordinates;
			SaveCoordinates(tempCoordinates);
		}
		
		CmdAim();
	}

	[Command]
	public void CmdAim()
	{
		Vector3 tempCoordinates = GetWorldPoint();
		targetShow.transform.position = tempCoordinates;
		SaveCoordinates(tempCoordinates);
	}

	public void SpellCast()
	{
		Cast();
		Destroy(targetShow);
		CmdDestroySpell();
	}

	[Command]
	void CmdDestroySpell()
	{
		Destroy(this);
	}
	

	public virtual void Cast()
	{
		print("DID IT");
	}

	public virtual void InitializeCoordinates()
	{
		coordinates = new Vector3();
		targetShow.transform.position = GetWorldPoint();
		targetShow.GetComponent<MeshRenderer>().enabled = true;
	}

	public virtual void SaveCoordinates(Vector3 updatedCoordinates)
	{
		coordinates = updatedCoordinates;
	}

	public Vector3 GetWorldPoint()
	{
		Ray ray = camera.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			return hit.point;
		}else
		{
			return Vector3.zero;
		}
	}

	public virtual void AnimatePlayer()
	{
		//animator.SetTrigger("Attack1");
		if (isLocalPlayer)
		{
			GetComponent<NetworkAnimator>().SetTrigger("Attack1");
		}
		
		
	}

	//retruns the vector corresponding to the direction of the projectile
	public Vector3 GetProjectileDirection(Vector3 spawnPos)
	{
		Vector3 velocityVec = coordinates - spawnPos;
		velocityVec /= velocityVec.magnitude;
		return velocityVec;
	}


	public void SetState(string newState)
	{
		switch (newState)
		{
			case "casting":
				state = newState;
				SetCastTime();
				AnimatePlayer();
				break;

			case "aiming":
				state = newState;
				break;

			case "idle":
				state = newState;
				break;

			case "waiting to cast":
				state = newState;
				break;

			case "ready":
				state = newState;
				break;

			default:
				print("Invalid state");
				break;
		}

		
	}

	

}
