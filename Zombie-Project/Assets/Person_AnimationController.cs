using UnityEngine;
using System.Collections;

public class Person_AnimationController : MonoBehaviour
{
	Animator anim;
	Rigidbody playerRigidbody;

	// Use this for initialization
	void Start ()
	{
		anim = this.GetComponent<Animator> ();
		playerRigidbody = this.GetComponentInParent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		anim.SetFloat ("Vel", playerRigidbody.velocity.magnitude);
	}

	public void SetDeath()
	{
		anim.SetBool ("setDeath", true);
	}

	public IEnumerator SetPickUp()
	{
		anim.SetBool ("setPicking", true);
		anim.Play ("Picking", 1);
		yield return new WaitForSeconds(1.0f);
		anim.SetBool ("setPicking", false);

	}

	public void SetRespawn()
	{
		anim.SetBool ("setDeath", false);
		anim.SetBool ("setPicking", false);
		anim.Play ("Standing", 0);
	}
}
