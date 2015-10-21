using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_Noise : NetworkBehaviour
{
	public void GenerateNoiseAtPlayer()
	{
		if (!isLocalPlayer)
			return;

		if (isServer) {
			Collider[] hitColliders = Physics.OverlapSphere(this.transform.position , 20f);
			
			foreach (Collider col in hitColliders) {
				if(col.name == "Zombie" || col.name == "Zombie(Clone)")
				{
					col.gameObject.GetComponent<Zombie_BasicMovement>().MoveToPos(this.transform.position);
				}
			}
		} else {
			CmdGenerateNoise(this.transform.position, 20f);
		}
	}

	[Command]
	void CmdGenerateNoise(Vector3 pos, float range)
	{
		Collider[] hitColliders = Physics.OverlapSphere(pos , range);
		
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

		if (isServer) {
			Collider[] hitColliders = Physics.OverlapSphere(pos , 20f);
			
			foreach (Collider col in hitColliders) {
				if(col.name == "Zombie" || col.name == "Zombie(Clone)")
				{
					col.gameObject.GetComponent<Zombie_BasicMovement>().MoveToPos(this.transform.position);
				}
			}
		} else {
			CmdGenerateNoise(pos, 20f);
		}
	}

	public void GenerateNoiseAtPlayerWithDistance(float dist)
	{
		if (!isLocalPlayer)
			return;

		if (isServer) {
			Collider[] hitColliders = Physics.OverlapSphere(this.transform.position , dist);
			
			foreach (Collider col in hitColliders) {
				if(col.name == "Zombie" || col.name == "Zombie(Clone)")
				{
					col.gameObject.GetComponent<Zombie_BasicMovement>().MoveToPos(this.transform.position);
				}
			}
		} else {
			CmdGenerateNoise(this.transform.position, dist);
		}
	}

	public void GenerateNoiseAtPosWithDistance(Vector3 pos, float dist)
	{
		if (!isLocalPlayer)
			return;

		if (isServer) {
			Collider[] hitColliders = Physics.OverlapSphere(pos, dist);
			
			foreach (Collider col in hitColliders) {
				if(col.name == "Zombie" || col.name == "Zombie(Clone)")
				{
					col.gameObject.GetComponent<Zombie_BasicMovement>().MoveToPos(this.transform.position);
				}
			}
		} else {
			CmdGenerateNoise(pos, dist);
		}
	}



}
