using UnityEngine;
using System.Collections;

public class Zombie_BasicMovement : MonoBehaviour
{
	public enum ZombieState{Idle, Move, Attack};
	public ZombieState state;

	GameObject zombie;
	GameObject player;
	Rigidbody zombieRigidbody;

	public AudioClip idleSound;
	public AudioClip attackSound;

	public NavMeshAgent agent;

	public bool isDamaged;
	public bool isRecovering;

	// Use this for initialization
	void Start ()
	{
		zombie = this.gameObject;
		zombieRigidbody = zombie.GetComponent<Rigidbody> ();
		state = ZombieState.Idle;
		agent = this.GetComponent<NavMeshAgent> ();
		agent.speed = 1.0f;

		this.StartCoroutine ("StartPatrol");
		this.StartCoroutine ("PlayIdleSound");


		isDamaged = false;
		isRecovering = false;
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
			if(isDamaged)
			{
				yield return new WaitForSeconds(1.0f);
				continue;
			}

			int move = Random.Range(0, 3);

			switch(move)
			{
				case 0:
				yield return this.StartCoroutine("MoveForward");
					break;
				case 1:
				yield return this.StartCoroutine("TurnRight");
					break;
				case 2:
				yield return this.StartCoroutine("TurnLeft");
					break;
				case 3:
				yield return this.StartCoroutine("StayStill");
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
		this.agent.speed = 3.5f;

		while (state == ZombieState.Attack)
		{
			//zombieRigidbody.velocity = zombie.transform.forward;
			//zombie.transform.LookAt(player.transform);
			//zombie.transform.localEulerAngles = new Vector3(0, zombie.transform.localEulerAngles.y ,0);


			if(isDamaged)
				agent.SetDestination(this.transform.position);
			else
				agent.SetDestination(player.transform.position);

			if(Vector3.Distance(zombie.transform.position, player.transform.position) > 50f)
			{
				this.agent.speed = 1.0f;
				agent.SetDestination(this.transform.position);
				state = ZombieState.Idle;
				this.StartCoroutine("StartPatrol");
			}

			yield return new WaitForSeconds(0.1f);
		}
	}

	IEnumerator MoveForward()
	{
		this.agent.SetDestination (this.transform.position + this.transform.forward * 5);

		yield return new WaitForSeconds (3.0f);

		this.agent.SetDestination (this.transform.position);
		/*
		float startTime = Time.time;
		float endTime = startTime + 2.0f;
		
		while (Time.time < endTime || !isDamaged)
		{
			zombieRigidbody.velocity = zombie.transform.forward;
			yield return new WaitForSeconds (0.01f);
		}

		zombieRigidbody.velocity = Vector3.zero;*/
	}

	IEnumerator TurnRight()
	{
		this.agent.SetDestination (this.transform.position + this.transform.right * 5);
		
		yield return new WaitForSeconds (3.0f);
		
		this.agent.SetDestination (this.transform.position);

		/*
		Vector3 startRotation = zombie.transform.localEulerAngles;
		Vector3 endRotation = startRotation + new Vector3 (0, 90, 0);
		float startTime = Time.time;
		float endTime = startTime + 1.0f;

		while (Time.time < endTime || !isDamaged)
		{
			zombie.transform.localEulerAngles = Vector3.Lerp(startRotation, endRotation, (Time.time - startTime)/2.0f);
			yield return new WaitForSeconds (0.01f);
		}
		*/
	}

	IEnumerator TurnLeft()
	{
		this.agent.SetDestination (this.transform.position + this.transform.right * -5);
		
		yield return new WaitForSeconds (3.0f);
		
		this.agent.SetDestination (this.transform.position);

		/*
		Vector3 startRotation = zombie.transform.localEulerAngles;
		Vector3 endRotation = startRotation + new Vector3 (0, -90, 0);
		float startTime = Time.time;
		float endTime = startTime + 1.0f;
		
		while (Time.time < endTime || !isDamaged)
		{
			zombie.transform.localEulerAngles = Vector3.Lerp(startRotation, endRotation, (Time.time - startTime)/2.0f);
			yield return new WaitForSeconds (0.01f);

		}*/
	}

	IEnumerator StayStill()
	{
		yield return new WaitForSeconds (1.0f);
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.name.Equals ("Player") && (state == ZombieState.Idle || state == ZombieState.Move)) {
			state = ZombieState.Attack;
			player = collider.gameObject;

			this.StopCoroutine("MoveToTarget");
			this.StopCoroutine("TurnLeft");
			this.StopCoroutine("TurnRight");
			this.StopCoroutine("MoveForward");
			this.StopCoroutine("StartPatrol");

			agent.SetDestination(this.transform.position);

			this.StartCoroutine("StartAttack");
		}
	}

	public void MoveToPos(Vector3 pos)
	{
		if (state == ZombieState.Move) {
			this.StopCoroutine("MoveToTarget");
		}

		if (state == ZombieState.Attack)
			return;

		player = null;
		state = ZombieState.Move;

		this.StartCoroutine ("MoveToTarget", pos);
	}


	public IEnumerator MoveToTarget(Vector3 pos)
	{
		while (state == ZombieState.Move)
		{
			/*
			zombieRigidbody.velocity = zombie.transform.forward;
			zombie.transform.LookAt(pos);
			zombie.transform.localEulerAngles = new Vector3(0, zombie.transform.localEulerAngles.y ,0);
			*/
			agent.speed = 2.5f;
			agent.SetDestination(pos);

			if(Vector3.Distance(zombie.transform.position, pos) < 2f || isDamaged)
			{
				agent.speed = 1.0f;
				agent.SetDestination(this.transform.position);
				state = ZombieState.Idle;
				this.StartCoroutine("StartPatrol");
			}
			
			yield return new WaitForSeconds(0.1f);
		}
	}


	// Update is called once per frame
	void Update ()
	{
	}

	public IEnumerator ResetDamaged()
	{
		isRecovering = true;
		this.StopCoroutine("MoveToTarget");
		this.StopCoroutine("TurnLeft");
		this.StopCoroutine("TurnRight");
		this.StopCoroutine("MoveForward");
		this.StopCoroutine("StartPatrol");

		yield return new WaitForSeconds (1.0f);

		isDamaged = false;
		isRecovering = false;

		if (!(state == ZombieState.Attack))
		{
			state = ZombieState.Idle;
			this.StartCoroutine ("StartPatrol");
		}
	}
}
