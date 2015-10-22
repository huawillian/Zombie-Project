using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Person_AnimationController : NetworkBehaviour
{
	Animator anim;
	Rigidbody playerRigidbody;

	public GameObject modelObject;

	
	[SyncVar, SerializeField]
	private Vector3 vel;
	[SyncVar, SerializeField]
	private bool deathFlag;
	[SyncVar, SerializeField]
	private bool pickupFlag;

	// Use this for initialization
	void Start ()
	{
		anim = modelObject.GetComponent<Animator> ();
		playerRigidbody = modelObject.GetComponentInParent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!isLocalPlayer)
		{
			anim.SetFloat("Vel", vel.magnitude);

			if(!deathFlag && anim.GetBool("setDeath"))
			{
				anim.Play ("Standing", 0);
				anim.SetBool ("setDeath", deathFlag);
			}

			if(deathFlag)
				anim.SetBool ("setDeath", true);

			if(!anim.GetBool("setPicking") && pickupFlag)
			{
				anim.Play("Picking", 1);
			}

			anim.SetBool ("setPicking", pickupFlag);

			return;
		}

		anim.SetFloat ("Vel", playerRigidbody.velocity.magnitude);

		if (isClient) {
			CmdSetVelOnServer (playerRigidbody.velocity);
		} else {
			vel = playerRigidbody.velocity;
		}
	}
	
	[Command]
	void CmdSetVelOnServer(Vector3 velocity)
	{
		vel = velocity;
		anim.SetFloat ("Vel", vel.magnitude);
	}

	[Command]
	void CmdSetDeathOnServer(bool isDeath)
	{
		deathFlag = isDeath;
		anim.SetBool ("setDeath", isDeath);

		if (!isDeath)
			anim.Play ("Standing", 0);

	}

	[Command]
	void CmdSetPickupOnServer(bool isPick)
	{
		pickupFlag = isPick;
		anim.SetBool ("setPicking", isPick);

		if (isPick)
			anim.Play ("Picking", 1);
	}

	public void SetDeath()
	{
		if (!isLocalPlayer)
			return;

		anim.SetBool ("setDeath", true);

		if (isClient) {
			CmdSetDeathOnServer (true);
		} else {
			deathFlag = true;
		}
	}

	public IEnumerator SetPickUp()
	{
		if (!isLocalPlayer)
			yield break;

		anim.SetBool ("setPicking", true);
		anim.Play ("Picking", 1);

		if (isClient)
			CmdSetPickupOnServer (true);
		else
			pickupFlag = true;

		yield return new WaitForSeconds(1.0f);
		anim.SetBool ("setPicking", false);

		if (isClient)
			CmdSetPickupOnServer (false);
		else
			pickupFlag = false;
	}

	public void SetRespawn()
	{
		if (!isLocalPlayer)
			return;

		anim.SetBool ("setDeath", false);
		anim.SetBool ("setPicking", false);
		anim.Play ("Standing", 0);

		if (isClient) {
			CmdSetDeathOnServer (false);
			CmdSetPickupOnServer (false);
		} else {
			deathFlag = false;
			pickupFlag = false;
		}
	}
}
