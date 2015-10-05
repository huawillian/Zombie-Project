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
		if (collider.name == "Renderer and Collider" && collider.transform.parent.name == "Player") {	
			collider.gameObject.transform.parent.gameObject.GetComponent<Rigidbody>().AddForce(this.transform.forward * 500f);
			collider.gameObject.transform.parent.gameObject.GetComponent<Player_Health>().damagePlayer(5);
		}
	}
}
