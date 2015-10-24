using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Zombie_BasicMovement : NetworkBehaviour
{
	public enum ZombieState{Idle, Move, Attack, Death};
	public ZombieState state;

	public AudioClip idleSound;
	public AudioClip attackSound;

	public NavMeshAgent agent;
	public Vector3 destination;
	public GameObject playerToAttack;

	[SyncVar]
	private Vector3 pos;
	[SyncVar]
	private Vector3 rot;
	
	[SyncVar]
	private float qx;
	[SyncVar]
	private float qy;
	[SyncVar]
	private float qz;
	[SyncVar]
	private float qw;

	// Use this for initialization
	void Start ()
	{
		this.StartCoroutine ("PlayIdleSound");
	}

	public override void OnStartServer()
	{
		state = ZombieState.Idle;
		agent = this.GetComponent<NavMeshAgent> ();
		agent.speed = 1.0f;
		destination = this.transform.position;

		StartCoroutine ("SetPlayerDestination");
	}

	IEnumerator SetPlayerDestination()
	{
		while (true) {
			yield return new WaitForSeconds (20f);
			if (state == ZombieState.Idle) {
				GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");

				MoveToPos (players [Random.Range (0, players.Length)].transform.position);
			}
		}
	}


	void Update()
	{
		if (isServer)
		{
			SetDestinationBasedState ();

			pos = this.transform.position;
			rot = this.transform.localEulerAngles;
			qx = this.transform.localRotation.x;
			qy = this.transform.localRotation.y;
			qz = this.transform.localRotation.z;
			qw = this.transform.localRotation.w;
		} else
		{
			this.transform.position = Vector3.MoveTowards(this.transform.position, pos, Time.deltaTime * 5f);
			this.transform.localRotation =  Quaternion.Lerp(this.transform.localRotation, new Quaternion(qx,qy,qz,qw), Time.deltaTime * 5f);
		}
	}

	private void SetDestinationBasedState()
	{
		if (state == ZombieState.Death) {
			return;
		}
		
		if (state == ZombieState.Move) {
			agent.SetDestination (destination);
			
			if (Vector3.Distance (this.transform.position, destination) < 1.0f) {
				agent.speed = 1.0f;
				state = ZombieState.Idle;
			}
		}
		
		if (state == ZombieState.Idle) {
			agent.SetDestination (destination);
			
			if (Vector3.Distance (this.transform.position, destination) < 1.0f) {
				destination = new Vector3 (this.transform.position.x + Random.Range (-10, 10), this.transform.position.y, this.transform.position.z + Random.Range (-10, 10));
			}
		}
		
		if (state == ZombieState.Attack) {
			if (playerToAttack != null) {
				agent.SetDestination (playerToAttack.transform.position);
			} else {
				agent.speed = 1.0f;
				state = ZombieState.Idle;
			}
		}
	}

	[Server]
	public void SetDeath()
	{
		if (state == ZombieState.Death)
			return;

		state = ZombieState.Death;
		agent.Stop(true);
		agent.updatePosition = false;
		agent.updateRotation = false;
		agent.enabled = false;
	}

	IEnumerator PlayIdleSound()
	{
		while(true)
		{
			yield return new WaitForSeconds((float)Random.Range(0,10));

			if(this.state == ZombieState.Idle)
			{
				AudioSource.PlayClipAtPoint(idleSound, this.transform.position);
			}
		}
	}

	[Server]
	public void StartAttack(GameObject pl)
	{
		if (state == ZombieState.Attack) 
			return;

		AudioSource.PlayClipAtPoint(attackSound, this.transform.position);
		this.agent.speed = 4.0f;
		playerToAttack = pl;
		state = ZombieState.Attack;
	}

	[Server]
	public void MoveToPos(Vector3 pos)
	{
		if (state == ZombieState.Attack) return;
		agent.speed = 3.0f;
		destination = pos;
		state = ZombieState.Move;
	}
	
	void OnTriggerEnter(Collider collider)
	{
		if (collider.name.StartsWith ("Player"))
		{
			if(state != ZombieState.Death && isServer)
				StartAttack(collider.gameObject);

			if(!isServer)
			{
				AudioSource.PlayClipAtPoint(attackSound, this.transform.position);
			}
		}
	}
}
