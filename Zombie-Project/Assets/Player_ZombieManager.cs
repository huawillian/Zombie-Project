using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_ZombieManager : NetworkBehaviour
{
	public AudioClip damageSound;
	public AudioClip deathSound;
	public GameObject corpsePrefab;

	public void SyncDamageZombie(string zombieName, int damage)
	{
		if (isServer)
		{
			ServerSyncDamageZombie (zombieName, damage);
		} else
		{
			CmdSyncDamageZombie(zombieName, damage);
		}
	}
	
	[ClientRpc]
	private void RpcSyncDamageZombie(string zombieName)
	{
		GameObject zombie = GameObject.Find(zombieName);
		AudioSource.PlayClipAtPoint(damageSound, zombie.transform.position);
		zombie.GetComponent<Zombie_AnimatorController> ().SetHurt ();
	}

	[Command]
	private void CmdSyncDamageZombie(string zombieName, int damage)
	{
		ServerSyncDamageZombie (zombieName, damage);
	}

	[Server]
	private void ServerSyncDamageZombie(string zombieName, int damage)
	{
		GameObject zombie = GameObject.Find(zombieName);
		zombie.GetComponent<Zombie_Health>().Health -= damage;
		AudioSource.PlayClipAtPoint(damageSound, zombie.transform.position);
		zombie.GetComponent<Zombie_AnimatorController> ().SetHurt ();
		RpcSyncDamageZombie(zombieName);
	}

	public void SyncDeathZombie(string zombieName)
	{
		if (isServer)
		{
			ServerSyncDeathZombie (zombieName);
		} else
		{
			CmdSyncDeathZombie(zombieName);
		}
	}
	
	[ClientRpc]
	private void RpcSyncDeathZombie(string zombieName)
	{
		GameObject zombie = GameObject.Find(zombieName);
		AudioSource.PlayClipAtPoint(deathSound, zombie.transform.position);
		zombie.GetComponent<Zombie_AnimatorController> ().SetDeath ();
		foreach(CapsuleCollider col in zombie.GetComponentsInChildren<CapsuleCollider>())
		{
			if(col.gameObject.activeInHierarchy)
				col.gameObject.SetActive(false);
		}
	}
	
	[Command]
	private void CmdSyncDeathZombie(string zombieName)
	{
		ServerSyncDeathZombie (zombieName);
	}

	[Server]
	private void ServerSyncDeathZombie(string zombieName)
	{
		GameObject zombie = GameObject.Find(zombieName);
		AudioSource.PlayClipAtPoint(deathSound, zombie.transform.position);
		zombie.GetComponent<Zombie_AnimatorController> ().SetDeath ();
		zombie.GetComponent<Zombie_BasicMovement> ().SetDeath ();
		foreach(CapsuleCollider col in zombie.GetComponentsInChildren<CapsuleCollider>())
		{
			if(col.gameObject.activeInHierarchy)
				col.gameObject.SetActive(false);
		}
		zombie.GetComponent<Rigidbody> ().isKinematic = true;
		zombie.transform.position += Vector3.down;
		StartCoroutine ("Despawn", zombieName);
		
		RpcSyncDeathZombie(zombieName);
	}

	[Server]
	private IEnumerator Despawn(string zombieName)
	{
		GameObject zombie = GameObject.Find(zombieName);
		yield return new WaitForSeconds(1.0f);
		NetworkServer.Spawn((GameObject)Instantiate (corpsePrefab, zombie.transform.position + Vector3.up / 2.0f, Quaternion.identity));
		Destroy (zombie.gameObject);
	}






















}
