using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Zombie_Spawner : NetworkBehaviour
{
	public GameObject zombiePrefab;

	// Use this for initialization
	void Start () 
	{
		StartCoroutine ("StartSpawning");
	}

	[Server]
	IEnumerator StartSpawning()
	{
		while (true)
		{

			Collider[] hitColliders = Physics.OverlapSphere(this.transform.position , 10f);
			int zombies = 0;
			bool playerNear = false;
			foreach (Collider col in hitColliders)
			{
				if(col.name == "Zombie" || col.name == "Zombie(Clone)")
				{
					zombies++;
				}

				if(col.tag == "Player")
				{
					playerNear = true;
				}
			}

			if(!(zombies > 10) && !playerNear)
			{
				GameObject temp = (Instantiate(zombiePrefab, this.transform.position, Quaternion.identity) as GameObject);
				temp.name = zombiePrefab.name;
				NetworkServer.Spawn(temp);
			}

			yield return new WaitForSeconds((float) Random.Range(10,15));
		}
	}
}
