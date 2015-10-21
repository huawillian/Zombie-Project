using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Zombie_Health : NetworkBehaviour
{
	public GameObject corpsePrefab;

	[SyncVar, SerializeField]
	private int health = 100;

	public AudioClip damageSound;
	public AudioClip deathSound;
	
	public int Health {
		get{
			return health;
		}
		set {
			if(value > 100) health = 100;
			else if(value <= 0 && health != 0)
			{
				health= 0;
				this.setDeath();
			}
			else 
			{
				health = value;
			}
		}
	}
	
	[Command]
	void CmdSyncHealth(int hp)
	{
		Health = hp;
	}

	public void damageZombie(int damage)
	{
		Health -= damage;
		AudioSource.PlayClipAtPoint(damageSound, this.transform.position);


		if (isServer) {
			Debug.Log("isServer dmg zombie");
			RpcSyncDmg ();
		}

		if (isClient)
		{
			//CmdSyncHealth (Health);

			foreach(GameObject o in GameObject.FindGameObjectsWithTag("Player"))
			{
				o.GetComponent<Player_ZombieManager>().SyncZombieHealth(this.gameObject, Health);
				o.GetComponent<Player_ZombieManager>().SyncZombieDmg(this.gameObject);
			}

			//CmdSyncDmg ();
		}
	}

	[Command]
	void CmdSyncDmg()
	{
		RpcSyncDmg ();
	}

	[ClientRpc]
	void RpcSyncDmg()
	{
		AudioSource.PlayClipAtPoint(damageSound, this.transform.position);
		this.GetComponentInChildren<Zombie_AnimatorController>().StartCoroutine("setHurt");
		this.GetComponent<Zombie_BasicMovement> ().isDamaged = true;
		this.GetComponent<Zombie_BasicMovement> ().StopCoroutine ("ResetDamaged");
		this.GetComponent<Zombie_BasicMovement> ().StartCoroutine ("ResetDamaged");
	}


	public void setDeath()
	{
		AudioSource.PlayClipAtPoint(deathSound, this.transform.position);

		foreach(GameObject o in GameObject.FindGameObjectsWithTag("Player"))
		{
			o.GetComponent<Player_ZombieManager>().SyncZombieDeath(this.gameObject);
		}

		/*
		if (isServer)
			RpcSyncDeath ();
		*/	
	}

	[ClientRpc]
	void RpcSyncDeath()
	{
		AudioSource.PlayClipAtPoint(deathSound, this.transform.position);
		
		this.GetComponent<Zombie_AnimatorController>().StartCoroutine("setDeath");
		this.GetComponent<NavMeshAgent>().enabled = false;
		this.GetComponent<Zombie_BasicMovement>().StopAllCoroutines();
		
		CapsuleCollider[] cols = this.GetComponentsInChildren<CapsuleCollider>();
		
		foreach(CapsuleCollider col in cols)
		{
			if(col.gameObject.activeInHierarchy)
				col.gameObject.SetActive(false);
		}
		
		this.GetComponent<Rigidbody>().isKinematic = true;

		this.gameObject.name = "Corpse";
		gameObject.AddComponent<Search_Content>();
		gameObject.AddComponent<BoxCollider>().isTrigger = true;
		this.gameObject.tag = "Search";
	}

	[Command]
	void CmdSyncDeath()
	{
		RpcSyncDeath ();
	}
}
