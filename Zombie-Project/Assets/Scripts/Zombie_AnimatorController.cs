using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Zombie_AnimatorController : NetworkBehaviour
{
	Animator anim;
	NavMeshAgent agent;

	[SyncVar]
	float velMag;

	[SyncVar]
	bool hurt;

	[SyncVar]
	bool dead;

	Vector3 currPos;
	Vector3 prevPos;

	public GameObject zombieModel;

	// Use this for initialization
	void Start ()
	{
		anim = zombieModel.GetComponent<Animator> ();
		agent = this.GetComponent<NavMeshAgent> ();

		currPos = zombieModel.transform.parent.transform.position;
		prevPos = zombieModel.transform.parent.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isServer) {
			velMag = ((zombieModel.transform.parent.transform.position - prevPos).magnitude / Time.deltaTime);
			hurt = anim.GetBool("setHurt");
			dead = anim.GetBool("setDeath");
			anim.SetFloat ("Speed", velMag);
			prevPos = currPos;
			currPos = zombieModel.transform.parent.transform.position;
		} else {
			anim.SetFloat("Speed", velMag);
			anim.SetBool("setDeath", dead);
			anim.SetBool("setHurt", hurt);
		}
	}
	
	public void SetHurt()
	{
		anim.SetBool ("setHurt", true);
		anim.Play ("Hurt", 1);
		anim.SetBool ("setHurt", false);

	}

	public void SetDeath()
	{
		anim.SetBool ("setDeath", true);
		anim.Play ("Death", 0);
	}
	
}
