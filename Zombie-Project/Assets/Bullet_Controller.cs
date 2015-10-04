using UnityEngine;
using System.Collections;

public class Bullet_Controller : MonoBehaviour
{
	public Rigidbody bulletRigidbody;
	public Vector3 bulletForward;
	public float deathTime;
	public float bulletDuration;
	public float bulletSpeed;

	// Use this for initialization
	void Start ()
	{
		bulletRigidbody = this.GetComponent<Rigidbody>();
		bulletForward = this.transform.forward;

		bulletDuration = 1f;
		deathTime = Time.time + bulletDuration;
		bulletSpeed = 20;

		this.transform.localEulerAngles = this.transform.parent.localEulerAngles;
		bulletRigidbody.velocity = this.transform.forward * bulletSpeed;
	}

	void Update()
	{
		if (Time.time > deathTime)
			Destroy (this.gameObject);
	}

}
