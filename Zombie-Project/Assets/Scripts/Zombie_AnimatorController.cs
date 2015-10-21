using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Zombie_AnimatorController : NetworkBehaviour
{
	Animator anim;
	NavMeshAgent agent;

	float velMag;
	Vector3 currPos;
	Vector3 prevPos;

	[SyncVar]
	public bool isAttacking;
	public GameObject zombieModel;

	// Use this for initialization
	void Start ()
	{
		anim = this.GetComponentInChildren<Animator> ();
		agent = zombieModel.GetComponentInParent<NavMeshAgent> ();

		currPos = zombieModel.transform.parent.transform.position;
		prevPos = zombieModel.transform.parent.transform.position;

		anim.SetBool ("setHands", true);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!isServer)
			return;

		velMag = ((zombieModel.transform.parent.transform.position - prevPos).magnitude / Time.deltaTime); 

		if (anim.enabled) {
			anim.SetFloat ("Speed", velMag);
			RpcSyncSpeed (velMag);
		}

		prevPos = currPos;
		currPos = zombieModel.transform.parent.transform.position;
	}

	[ClientRpc]
	void RpcSyncSpeed(float sp)
	{
			anim.SetFloat ("Speed", sp);
	}

	[ClientRpc]
	void RpcSyncHands(bool h)
	{
		anim.SetBool ("setHands", h);
	}

	[ClientRpc]
	void RpcSyncHurt(bool h)
	{
		anim.SetBool ("setHurt", h);
	}

	[ClientRpc]
	void RpcSyncDeath()
	{
		anim.SetBool ("setHands", false);
		anim.Play ("Death");
	}

	[ClientRpc]
	void RpcSyncDeathSpeed()
	{
		anim.speed = 0;
	}

	public IEnumerator setHurt()
	{
		anim.SetBool ("setHands", false);
		anim.SetBool ("setHurt", true);
		yield return new WaitForSeconds (0.5f);
		anim.SetBool ("setHurt", false);
		anim.SetBool ("setHands", true);
	}

	public IEnumerator setDeath()
	{
		anim.SetBool ("setHands", false);
		anim.Play ("Death");
		yield return new WaitForSeconds (2.0f);
		anim.speed = 0;
	}
	
}
