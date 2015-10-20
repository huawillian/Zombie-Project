using UnityEngine;
using System.Collections;

public class Player_Noise : MonoBehaviour
{
	public void GenerateNoiseAtPlayer()
	{
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
		Collider[] hitColliders = Physics.OverlapSphere(pos , dist);
		
		foreach (Collider col in hitColliders) {
			if(col.name == "Zombie" || col.name == "Zombie(Clone)")
			{
				col.gameObject.GetComponent<Zombie_BasicMovement>().MoveToPos(pos);
			}
		}
	}



}
