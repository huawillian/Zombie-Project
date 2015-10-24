using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_Proximity : NetworkBehaviour
{
	GameObject[] players;
	GameObject[] boxes;
	GameObject[] zombies;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isServer)
		{
			players = GameObject.FindGameObjectsWithTag ("Player");
			boxes = GameObject.FindGameObjectsWithTag ("Box");
			zombies = GameObject.FindGameObjectsWithTag ("Zombie");
		}
	}

	void CheckBoxes()
	{

	}
}
