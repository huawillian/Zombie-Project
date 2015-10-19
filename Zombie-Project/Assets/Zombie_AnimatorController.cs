using UnityEngine;
using System.Collections;

public class Zombie_AnimatorController : MonoBehaviour
{
	Animator anim;
	NavMeshAgent agent;

	float velMag;
	Vector3 currPos;
	Vector3 prevPos;

	public bool isAttacking;

	// Use this for initialization
	void Start ()
	{
		anim = this.GetComponent<Animator> ();
		agent = this.GetComponentInParent<NavMeshAgent> ();

		currPos = this.transform.parent.transform.position;
		prevPos = this.transform.parent.transform.position;

		anim.SetFloat ("handsOffset", Random.Range(0.0f,5.0f));
		anim.SetBool ("setHands", true);

	}
	
	// Update is called once per frame
	void Update ()
	{
		velMag = ((this.transform.parent.transform.position - prevPos).magnitude / Time.deltaTime); 

		if(anim.enabled) anim.SetFloat ("Speed", velMag);

		prevPos = currPos;
		currPos = this.transform.parent.transform.position;
	}

	public IEnumerator setHurt()
	{
		anim.SetBool ("setHands", false);
		anim.SetBool ("setHurt", true);
		yield return new WaitForSeconds (1.0f);
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
