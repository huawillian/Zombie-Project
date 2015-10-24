using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Zombie_Health : NetworkBehaviour
{
	[SyncVar]
	private int health = 100;

	[SyncVar, SerializeField]
	private string uniqueName;

	public AudioClip damageSound;
	public AudioClip deathSound;

	public bool isDead = false;

	public GameObject corpsePrefab;
	Player_ZombieManager zombieManagerWithAuth;

	public int Health
	{
		get{
			return health;
		}
		set {
			if(value > 100) health = 100;
			else if(value <= 0)
			{
				health= 0;

				if(!isDead && isServer)
				{
					isDead = true;
					SetDeath();
				}
			}
			else 
			{
				health = value;
			}
		}
	}

	void Start()
	{
		NetworkProximityChecker prox = this.GetComponent<NetworkProximityChecker> ();
		prox.visRange = 25;
		prox.visUpdateInterval = Random.Range (5f,10f);

		foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
		{
			if(player.GetComponent<NetworkIdentity>().isLocalPlayer)
			{
				zombieManagerWithAuth = player.GetComponent<Player_ZombieManager>();
			}
		}
	}

	public override void OnStartServer()
	{
		uniqueName = "Zombie_" + this.gameObject.GetInstanceID ().ToString ();
		this.name = uniqueName;
	}
	
	public override void OnStartClient()
	{
		this.name = uniqueName;
		
		if (!isServer)
		{
			this.GetComponent<Rigidbody> ().useGravity = false;
			this.GetComponent<Rigidbody> ().isKinematic = true;
		}
	}

	public void damageZombie(int damage)
	{
		zombieManagerWithAuth.SyncDamageZombie (uniqueName, damage);
	}
	
	public void SetDeath()
	{
		zombieManagerWithAuth.SyncDeathZombie (uniqueName);
	}
}
