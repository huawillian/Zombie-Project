using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_ZombieManager : NetworkBehaviour
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SyncZombieHealth(GameObject zombie, int hp)
	{
		if(isServer)
		zombie.GetComponent<Zombie_Health> ().Health = hp;
	}

	public void SyncZombieDmg(GameObject zombie)
	{
		if (!isServer) {
			Debug.Log ("SyncZOmbiedmg client");

			CmdSyncZombieDmgToServer (zombie);
			return;
		} else {
			zombie.GetComponentInChildren<Zombie_AnimatorController>().StartCoroutine("setHurt");
			zombie.GetComponent<Zombie_BasicMovement> ().isDamaged = true;
			zombie.GetComponent<Zombie_BasicMovement> ().StopCoroutine ("ResetDamaged");
			zombie.GetComponent<Zombie_BasicMovement> ().StartCoroutine ("ResetDamaged");
		}
	}

	[Command]
	void CmdSyncZombieDmgToServer(GameObject zombie)
	{
		Debug.Log ("SyncZOmbiedmg Server");

		RpcSyncDmg (zombie);
		
		zombie.GetComponentInChildren<Zombie_AnimatorController>().StartCoroutine("setHurt");
		zombie.GetComponent<Zombie_BasicMovement> ().isDamaged = true;
		zombie.GetComponent<Zombie_BasicMovement> ().StopCoroutine ("ResetDamaged");
		zombie.GetComponent<Zombie_BasicMovement> ().StartCoroutine ("ResetDamaged");
	}

	[ClientRpc]
	void RpcSyncDmg(GameObject zombie)
	{
		Debug.Log ("SyncZOmbiedmg client1");

		zombie.GetComponentInChildren<Zombie_AnimatorController>().StartCoroutine("setHurt");
		zombie.GetComponent<Zombie_BasicMovement> ().isDamaged = true;
		zombie.GetComponent<Zombie_BasicMovement> ().StopCoroutine ("ResetDamaged");
		zombie.GetComponent<Zombie_BasicMovement> ().StartCoroutine ("ResetDamaged");
	}

	public void SyncZombieDeath(GameObject zombie)
	{
		if (!isServer) {
			Debug.Log ("SyncZOmbieDeath Client");
			CmdSyncZombieDeathToServer (zombie);
			return;
		} else {
			Debug.Log ("SyncZOmbieDeath server");
			zombie.GetComponent<Zombie_AnimatorController>().StartCoroutine("setDeath");
			zombie.GetComponent<NavMeshAgent>().enabled = false;
			zombie.GetComponent<Zombie_BasicMovement>().StopAllCoroutines();
			
			CapsuleCollider[] cols = zombie.GetComponentsInChildren<CapsuleCollider>();
			
			foreach(CapsuleCollider col in cols)
			{
				if(col.gameObject.activeInHierarchy)
					col.gameObject.SetActive(false);
			}
			
			zombie.GetComponent<Rigidbody>().isKinematic = true;
			zombie.gameObject.name = "Corpse";
			zombie.AddComponent<Search_Content>();
			zombie.gameObject.tag = "Search";
			zombie.AddComponent<BoxCollider>().isTrigger = true;
		}
	}

	[Command]
	void CmdSyncZombieDeathToServer(GameObject zombie)
	{
		RpcSyncDeath (zombie);

		Debug.Log ("SyncZOmbieDeath server");
		
		RpcSyncDeath (zombie);
		
		zombie.GetComponent<Zombie_AnimatorController>().StartCoroutine("setDeath");
		zombie.GetComponent<NavMeshAgent>().enabled = false;
		zombie.GetComponent<Zombie_BasicMovement>().StopAllCoroutines();
		
		CapsuleCollider[] cols = zombie.GetComponentsInChildren<CapsuleCollider>();
		
		foreach(CapsuleCollider col in cols)
		{
			if(col.gameObject.activeInHierarchy)
				col.gameObject.SetActive(false);
		}

		zombie.GetComponent<Rigidbody>().isKinematic = true;
		zombie.gameObject.name = "Corpse";
		zombie.AddComponent<Search_Content>();
		zombie.gameObject.tag = "Search";
		zombie.AddComponent<BoxCollider>().isTrigger = true;
	}


	[ClientRpc]
	void RpcSyncDeath(GameObject zombie)
	{
		zombie.GetComponent<Zombie_AnimatorController>().StartCoroutine("setDeath");
		zombie.GetComponent<NavMeshAgent>().enabled = false;
		zombie.GetComponent<Zombie_BasicMovement>().StopAllCoroutines();
		
		CapsuleCollider[] cols = zombie.GetComponentsInChildren<CapsuleCollider>();
		
		foreach(CapsuleCollider col in cols)
		{
			if(col.gameObject.activeInHierarchy)
				col.gameObject.SetActive(false);
		}

		zombie.GetComponent<Rigidbody>().isKinematic = true;
		zombie.gameObject.name = "Corpse";
		zombie.AddComponent<Search_Content>();
		zombie.gameObject.tag = "Search";
		zombie.AddComponent<BoxCollider>().isTrigger = true;
	}
}
