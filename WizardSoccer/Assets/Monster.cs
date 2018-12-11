using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour {

	public int playerID;

	GameObject ball;

	Animator animator;

	NavMeshAgent agent;

	float distTrav;

	float maxDist = 8.0f;

	Vector3 lastPosition;
	Vector3 targetLastPosition;

	string state = "idle";

	string orders;

	Transform target;
	Transform ally;
	Transform ownGoal;
	Transform enemyGoal;

	// Use this for initialization
	void Start() {
		ball = GameObject.Find("Ball");
		lastPosition = transform.position;
		agent = GetComponent<NavMeshAgent>();
		SetGoals();
	}


	private void OnTriggerEnter(Collider collision)
	{

		switch (orders)
		{
			case "shoot":
				if (collision.gameObject.name == "Ball")
				{
					ShootBall();
				}
				break;

			case "pass":
				if (collision.gameObject.name == "Ball")
				{
					PassBall();
				}
				break;

			case "block":
				if (collision.gameObject.transform.root == target)
				{
					Block();
				}
				break;

			case "clear":
				if (collision.gameObject.name == "Ball")
				{
					ClearBall();
				}
				break;

			case "move":
				if (collision.gameObject.name == "Ball")
				{
					ShootBall();
				}
				break;
			
		}
	}

	// Update is called once per frame
	private void Update()
	{
		switch (state)
		{
			case "moving":
				CheckDistance();
				break;

			case "following":
				UpdateTarget();
				CheckDistance();
				break;
		}

	}

	public void SetAlly(Transform teamMate)
	{
		ally = teamMate;
	}

	public void NewOrders(Transform newTarget, string newOrders)
	{
		orders = newOrders;
		agent = GetComponent<NavMeshAgent>();
		agent.SetDestination(newTarget.position);
		target = newTarget;

		switch (orders)
		{
			case "block":
				state = "following";
				break;

			case "shoot":
				state = "following";
				break;

			case "clear":
				state = "following";
				break;

			case "move":
				state = "moving";
				break;
			
		}

	}

	void UpdateTarget()
	{
		if (targetLastPosition != Vector3.zero)
		{
			Vector3 targetTraj = target.position - targetLastPosition;
			if (targetTraj.magnitude == 0)
			{
				targetTraj = Vector3.zero;
			}
			else
			{
				targetTraj /= targetTraj.magnitude;
			}

			agent.SetDestination(target.position + targetTraj);
		}
		else
		{
			agent.SetDestination(target.position);
		}

		targetLastPosition = target.position;
	}

	void CheckDistance()
	{
		distTrav += Vector3.Distance(lastPosition, transform.position);
		lastPosition = transform.position;

		if (maxDist <= distTrav)
		{
			
			StopMonster();
		}

		lastPosition = transform.position;

	}




	void PassBall()
	{
		Vector3 endPosition = ally.GetComponent<Monster>().target.position;
		Vector3 passTraj = GetTraj(endPosition, 5.0f, 4.0f, 0.65f);
		LaunchBall(passTraj);
		StopMonster();
	}

	void ShootBall()
	{
		print("Shoot");
		Vector3 shotTarget = enemyGoal.position;
		float regMissChance = 0.3f;
		float distOffset = (Mathf.Abs(transform.position.z - enemyGoal.position.z)) / (Mathf.Abs(enemyGoal.position.z - transform.position.z));
		float distMissChance = (0.9f - 0.3f) * distOffset;
		
		if (Random.Range(0.0f, 1.0f) > 0.5f)
		{
			shotTarget += new Vector3(2.0f, 0.0f, 0.0f);
		}else
		{
			shotTarget += new Vector3(-2.0f, 0.0f, 0.0f);
		}

		Vector3 shotTraj = GetTraj(shotTarget, 10.0f, 10.0f, regMissChance + distMissChance);
		LaunchBall(shotTraj);
		StopMonster();
	}

	void Block()
	{
		print("Block");
		target.GetComponent<Monster>().StopMonster();
		StopMonster();
		Vector3 pushBack = target.transform.position - transform.position;
		pushBack /= pushBack.magnitude;
		pushBack *= 0.5f;
		transform.position += pushBack * -1.0f;
		target.transform.position += pushBack;
	}

	void ClearBall()
	{
		Vector3 clearTraj;
		float velZ = 10.0f;
		

		if (Vector3.Distance(ball.transform.position, ownGoal.position) < Vector3.Distance(transform.position, ownGoal.position) && Mathf.Abs(transform.position.x - ball.transform.position.x)<0.5f)
		{
			velZ = 8.0f;
		}

		if (playerID == 1)
		{
			velZ *= -1.0f;
		}

		if (ball.transform.position.x - transform.position.x < 0)
		{
			clearTraj = new Vector3(10.0f, 10.0f, velZ);
		}
		else
		{
			clearTraj = new Vector3(-10.0f, 10.0f, velZ);
		}

		LaunchBall(clearTraj);

		StopMonster();
	}


	void StopMonster()
	{
		agent.isStopped = true;
		state = "idle";
		distTrav = 0.0f;
		orders = "none";
		targetLastPosition = Vector3.zero;

	}

	void LaunchBall(Vector3 traj)
	{
		ball.GetComponent<Rigidbody>().velocity = traj;
	}

	Vector3 GetTraj(Vector3 endPoint, float shotSpeed, float maxMiss, float missChance)
	{
		if (Random.Range(0.0f, 1.0f) < missChance)
		{
			endPoint += Random.insideUnitSphere * Random.Range(0.0f, maxMiss);
		}
		
		Vector3 startPoint = ball.transform.position;
		float travelTime = Vector3.Distance(endPoint, startPoint) / shotSpeed;
		float velX = (endPoint.x - startPoint.x) / travelTime;
		float velZ = (endPoint.z - startPoint.z) / travelTime;
		float velY = (9.81f * 0.5f * travelTime * travelTime + (startPoint.y - endPoint.y)) / travelTime;
		velY += 1.0f;

		return new Vector3(velX, velY, velZ);
	}

	public void SetGoals()
	{
		if (playerID == 0)
		{
			ownGoal = GameObject.Find("Goal1").transform;
			enemyGoal = GameObject.Find("Goal2").transform;
		}else
		{
			ownGoal = GameObject.Find("Goal2").transform;
			enemyGoal = GameObject.Find("Goal1").transform;
		}
	}

	public void SetPlayerID(int theID)
	{
		playerID = theID;
	}

	
}
