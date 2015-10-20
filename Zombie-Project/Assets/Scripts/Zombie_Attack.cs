using UnityEngine;
using System.Collections;

public class Zombie_Attack : MonoBehaviour
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.name == "Renderer and Collider" && collider.transform.parent.name == "Player")
		{	
			if(!this.gameObject.transform.parent.gameObject.GetComponentInChildren<Zombie_AnimatorController>().isAttacking && !this.gameObject.transform.parent.gameObject.GetComponent<Zombie_BasicMovement>().isDamaged)
			{
				this.transform.parent.gameObject.GetComponent<Rigidbody>().AddForce(this.transform.forward * -300f);
				collider.gameObject.transform.parent.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
				collider.gameObject.transform.parent.gameObject.GetComponent<Rigidbody>().AddForce(this.transform.forward * 300f);
				collider.gameObject.transform.parent.gameObject.GetComponent<Player_Health>().damagePlayer(5);
			}
		}
	}
}
