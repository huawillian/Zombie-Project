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
			Instantiate(zombiePrefab, this.transform.position, Quaternion.identity);
			yield return new WaitForSeconds((float) Random.Range(5,10));
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
