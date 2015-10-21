using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_Noise : NetworkBehaviour
{
	void Start()
	{

	}

	public void GenerateNoiseAtPlayer()
	{
		if (!isLocalPlayer)
			return;

		Collider[] hitColliders = Physics.OverlapSphere(this.transform.position , 20f);

		foreach (Collider col in hitColliders) {
			if(col.name == "Zombie" || col.name == "Zombie(Clone)")
			{
				col.gameObject.GetComponent<Zombie_BasicMovement>().MoveToPos(this.transform.position);
			}
		}
	}

	public void GenerateNoiseAtPos(Vector3 pos)
	{
		if (!isLocalPlayer)
			return;

		Collider[] hitColliders = Physics.OverlapSphere(pos , 20f);
		
		foreach (Collider col in hitColliders) {
			if(col.name == "Zombie" || col.name == "Zombie(Clone)")
			{
				col.gameObject.GetComponent<Zombie_BasicMovement>().MoveToPos(pos);
			}
		}
	}

	public void GenerateNoiseAtPlayerWithDistance(float dist)
	{
		if (!isLocalPlayer)
			return;

		Collider[] hitColliders = Physics.OverlapSphere(this.transform.position , dist);
		
		foreach (Collider col in hitColliders) {
			if(col.name == "Zombie" || col.name == "Zombie(Clone)")
			{
				col.gameObject.GetComponent<Zombie_BasicMovement>().MoveToPos(this.transform.position);
			}
		}
	}

	public void GenerateNoiseAtPosWithDistance(Vector3 pos, float dist)
	{
		if (!isLocalPlayer)
			return;

		Collider[] hitColliders = Physics.OverlapSphere(pos , dist);
		
		foreach (Collider col in hitColliders) {
			if(col.name == "Zombie" || col.name == "Zombie(Clone)")
			{
				col.gameObject.GetComponent<Zombie_BasicMovement>().MoveToPos(pos);
			}
		}
	}



}
