using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Zombie_BasicMovement : NetworkBehaviour
{
	public enum ZombieState{Idle, Move, Attack};
	public ZombieState state;

	GameObject zombie;
	GameObject player;
	Rigidbody zombieRigidbody;

	public AudioClip idleSound;
	public AudioClip attackSound;

	public NavMeshAgent agent;

	[SyncVar]
	public bool isDamaged;
	public bool isRecovering;

	[SyncVar]
	public Vector3 pos;
	[SyncVar]
	public Vector3 rot;
	
	// Use this for initialization
	void Start ()
	{
		zombie = this.gameObject;
		zombieRigidbody = zombie.GetComponent<Rigidbody> ();
		state = ZombieState.Idle;
		agent = this.GetComponent<NavMeshAgent> ();
		agent.speed = 1.0f;

		isDamaged = false;
		isRecovering = false;

		if (!isServer)
			return;
		
		this.StartCoroutine ("StartPatrol");
		this.StartCoroutine ("PlayIdleSound");
	}

	IEnumerator PlayIdleSound()
	{
		if (!isServer)
			yield break;

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
		if (!isServer)
			yield break;

		while (state == ZombieState.Idle)
		{
			if (!agent.enabled)
				yield break;

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
				//yield return this.StartCoroutine("StayStill");
					break;
				default:
					Debug.Log(this.gameObject.ToString() + " has invalid move.");
					break;
			}
		}
	}

	IEnumerator StartAttack()
	{
		if (!isServer)
			yield break;


		if (!agent.enabled)
			yield break;

		Debug.Log ("Start Attack");
		AudioSource.PlayClipAtPoint(attackSound, this.transform.position);
		this.agent.speed = 4.5f;

		while (state == ZombieState.Attack)
		{
			if (!agent.enabled)
				yield break;

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

			yield return new WaitForSeconds(2.0f);
		}
	}

	IEnumerator MoveForward()
	{
		if (!isServer)
			yield break;


		if (!agent.enabled)
			yield break;
		this.agent.SetDestination (this.transform.position + this.transform.forward * 5);

		yield return new WaitForSeconds (3.0f);

		this.agent.SetDestination (this.transform.position);
	}

	IEnumerator TurnRight()
	{
		if (!isServer)
			yield break;

		if (!agent.enabled)
			yield break;
		this.agent.SetDestination (this.transform.position + this.transform.right * 5);
		
		yield return new WaitForSeconds (3.0f);
		
		this.agent.SetDestination (this.transform.position);
	}

	IEnumerator TurnLeft()
	{
		if (!isServer)
			yield break;

		if (!agent.enabled)
			yield break;

		this.agent.SetDestination (this.transform.position + this.transform.right * -5);
		
		yield return new WaitForSeconds (3.0f);
		
		this.agent.SetDestination (this.transform.position);
	}

	IEnumerator StayStill()
	{
		if (!isServer)
			yield break;

		yield return new WaitForSeconds (1.0f);
	}

	void OnTriggerEnter(Collider collider)
	{
		if (!isServer)
			return;

		if (collider.name.Equals ("Player(Clone)") && (state == ZombieState.Idle || state == ZombieState.Move)) {
			state = ZombieState.Attack;
			player = collider.gameObject;

			this.StopCoroutine("MoveToTarget");
			this.StopCoroutine("TurnLeft");
			this.StopCoroutine("TurnRight");
			this.StopCoroutine("MoveForward");
			this.StopCoroutine("StartPatrol");

			if (!agent.enabled)
				return;
			agent.SetDestination(this.transform.position);

			this.StartCoroutine("StartAttack");
		}
	}

	public void MoveToPos(Vector3 pos)
	{
		if (!isServer)
			return;

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
		if (!isServer)
			yield break;

		while (state == ZombieState.Move)
		{
			agent.speed = 4.5f;
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
		if (!isServer) {
			this.gameObject.transform.position = pos;
			this.gameObject.transform.localEulerAngles = rot;
		} else {
			pos = this.transform.position;
			rot = this.transform.localEulerAngles;
		}
	}

	public IEnumerator ResetDamaged()
	{
		if (!isServer)
			yield break;

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
