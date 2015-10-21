using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Bullet_Controller : NetworkBehaviour
{
	public Rigidbody bulletRigidbody;
	public Vector3 bulletForward;
	public float deathTime;
	public float bulletDuration;
	public float bulletSpeed;

	public AudioClip shotZombieSound;

	// Use this for initialization
	void Start ()
	{
		bulletRigidbody = this.GetComponent<Rigidbody>();
		bulletForward = this.transform.forward;
		bulletDuration = 1f;
		deathTime = Time.time + bulletDuration;
		bulletSpeed = 120;
		bulletRigidbody.velocity = this.transform.forward * bulletSpeed;
	}

	void Update()
	{
		if (Time.time > deathTime)
			Destroy (this.gameObject);
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.name == "Renderer and Collider" && (collider.transform.parent.name == "Zombie" || collider.transform.parent.name == "Zombie(Clone)")) {	
			AudioSource.PlayClipAtPoint(shotZombieSound, this.transform.position);
			collider.gameObject.transform.parent.gameObject.GetComponent<Zombie_Health>().damageZombie(30);
			Destroy(this.gameObject);
		}
	}
}
