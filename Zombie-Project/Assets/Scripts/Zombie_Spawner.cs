using UnityEngine;
using System.Collections;

public class Zombie_Spawner : MonoBehaviour
{
	public GameObject zombiePrefab;

	// Use this for initialization
	void Start () 
	{
		StartCoroutine ("StartSpawning");
	}

	IEnumerator StartSpawning()
	{
		while (true)
		{

			Collider[] hitColliders = Physics.OverlapSphere(this.transform.position , 30f);
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

			if(!(zombies > 5) && !playerNear)
			{
				(Instantiate(zombiePrefab, this.transform.position, Quaternion.identity) as GameObject).name = zombiePrefab.name;
			}

			yield return new WaitForSeconds((float) Random.Range(8,15));
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
