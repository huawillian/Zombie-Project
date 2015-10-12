using UnityEngine;
using System.Collections;

public class Zombie_BasicMovement : MonoBehaviour
{
	enum ZombieState{Idle, Move, Attack};
	ZombieState state;

	GameObject zombie;
	GameObject player;
	Rigidbody zombieRigidbody;

	public AudioClip idleSound;
	public AudioClip attackSound;

	public NavMeshAgent agent;

	// Use this for initialization
	void Start ()
	{
		zombie = this.gameObject;
		zombieRigidbody = zombie.GetComponent<Rigidbody> ();
		state = ZombieState.Idle;

		StartCoroutine ("StartPatrol");
		StartCoroutine ("PlayIdleSound");

		agent = this.GetComponent<NavMeshAgent> ();
	}

	IEnumerator PlayIdleSound()
	{
		yield return new WaitForSeconds((float)Random.Range(0,10));

		while(true)
		{
			if(this.state == ZombieState.Idle)
			{
				AudioSource.PlayClipAtPoint(idleSound, this.transform.position);
			}

			yield return new WaitForSeconds((float)Random.Range(0,10));
		}
	}

	IEnumerator StartPatrol()
	{
		while (state == ZombieState.Idle)
		{
			int move = Random.Range(0, 3);

			switch(move)
			{
				case 0:
					yield return StartCoroutine("MoveForward");
					break;
				case 1:
					yield return StartCoroutine("TurnRight");
					break;
				case 2:
					yield return StartCoroutine("TurnLeft");
					break;
				case 3:
					yield return StartCoroutine("StayStill");
					break;
				default:
					Debug.Log(this.gameObject.ToString() + " has invalid move.");
					break;
			}
		}
	}

	IEnumerator StartAttack()
	{
		Debug.Log ("Start Attack");
		AudioSource.PlayClipAtPoint(attackSound, this.transform.position);

		while (state == ZombieState.Attack)
		{
			//zombieRigidbody.velocity = zombie.transform.forward;
			//zombie.transform.LookAt(player.transform);
			//zombie.transform.localEulerAngles = new Vector3(0, zombie.transform.localEulerAngles.y ,0);

			agent.SetDestination(player.transform.position);

			if(Vector3.Distance(zombie.transform.position, player.transform.position) > 20f)
			{
				agent.SetDestination(this.transform.position);
				state = ZombieState.Idle;
				StartCoroutine("StartPatrol");
			}

			yield return new WaitForSeconds(0.1f);
		}
	}

	IEnumerator MoveForward()
	{
		float startTime = Time.time;
		float endTime = startTime + 5.0f;
		
		while (Time.time < endTime)
		{
			zombieRigidbody.velocity = zombie.transform.forward;
			yield return new WaitForSeconds (0.01f);
		}

		zombieRigidbody.velocity = Vector3.zero;
	}

	IEnumerator TurnRight()
	{
		Vector3 startRotation = zombie.transform.localEulerAngles;
		Vector3 endRotation = startRotation + new Vector3 (0, 90, 0);
		float startTime = Time.time;
		float endTime = startTime + 2.0f;

		while (Time.time < endTime)
		{
			zombie.transform.localEulerAngles = Vector3.Lerp(startRotation, endRotation, (Time.time - startTime)/2.0f);
			yield return new WaitForSeconds (0.01f);
		}
	}

	IEnumerator TurnLeft()
	{
		Vector3 startRotation = zombie.transform.localEulerAngles;
		Vector3 endRotation = startRotation + new Vector3 (0, -90, 0);
		float startTime = Time.time;
		float endTime = startTime + 2.0f;
		
		while (Time.time < endTime)
		{
			zombie.transform.localEulerAngles = Vector3.Lerp(startRotation, endRotation, (Time.time - startTime)/2.0f);
			yield return new WaitForSeconds (0.01f);

		}
	}

	IEnumerator StayStill()
	{
		yield return new WaitForSeconds (2.0f);
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.name.Equals ("Player") && (state == ZombieState.Idle || state == ZombieState.Move)) {
			state = ZombieState.Attack;
			player = collider.gameObject;

			StopCoroutine("MoveToTarget");
			agent.SetDestination(this.transform.position);

			StartCoroutine("StartAttack");
		}
	}

	public void MoveToPos(Vector3 pos)
	{
		if (state == ZombieState.Move) {
			StopCoroutine("MoveToTarget");
		}

		state = ZombieState.Move;
		player = null;

		StartCoroutine ("MoveToTarget", pos);
	}


	public IEnumerator MoveToTarget(Vector3 pos)
	{
		while (state == ZombieState.Move && state != ZombieState.Attack)
		{
			/*
			zombieRigidbody.velocity = zombie.transform.forward;
			zombie.transform.LookAt(pos);
			zombie.transform.localEulerAngles = new Vector3(0, zombie.transform.localEulerAngles.y ,0);
			*/

			agent.SetDestination(pos);

			if(Vector3.Distance(zombie.transform.position, pos) < 2f)
			{
				agent.SetDestination(this.transform.position);
				state = ZombieState.Idle;
				StartCoroutine("StartPatrol");
			}
			
			yield return new WaitForSeconds(0.1f);
		}
	}


	// Update is called once per frame
	void Update ()
	{
		
	}
}
