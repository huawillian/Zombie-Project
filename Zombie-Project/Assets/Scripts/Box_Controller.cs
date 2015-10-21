using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Box_Controller : NetworkBehaviour
{
	Search_Content searchScript;

	public GameObject baseballBatPrefab;
	public GameObject pistolPrefab;
	public GameObject ammoPrefab;
	public GameObject foodPrefab;
	public GameObject medkitPrefab;
	public GameObject woodPrefab;

	public GameObject player;
	
	[SyncVar, SerializeField]
	private int health = 100;
	
	public int Health{
		get{
			return health;
		}
		set {
			if(value > 100) health = 100;
			else if(value <= 0)
			{
				health= 0;
			}
			else 
			{
				health = value;
			}

			if(!isServer)
				CmdSyncHealth(Health);
		}
	}

	[Command]
	void CmdSyncHealth(int hp)
	{
		Health = hp;
	}

	// Use this for initialization
	void Start ()
	{
		searchScript = this.GetComponent<Search_Content> ();
	}

	void OnTriggerEnter(Collider obj)
	{
		if (obj.name == "Player")
			player = obj.gameObject;
	}
}
