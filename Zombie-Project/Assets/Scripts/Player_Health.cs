using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Player_Health :NetworkBehaviour
{
	public GameObject healthUI;
	public Player_Hunger hungerScript;
	public Player_Death deathScript;

	public AudioClip gruntSound;

	[SerializeField, SyncVar]
	private int health = 100;
	
	public int Health {
		get {
			return health;
		}
		set {
			if(value > 100)
			{
				health = 100;
			}
			else if(value <= 0)
			{
				if(health != 0)
				{
					health = 0;
					deathScript.setDeath();
				}
			}
			else 
			{
				health = value;
			}

			healthUI.GetComponent<Slider>().value = health;
		}
	}

	void Start()
	{
		hungerScript = this.GetComponent<Player_Hunger> ();
		deathScript = this.GetComponent<Player_Death> ();
		if (!isLocalPlayer)
			return;
		StartCoroutine ("StartHealthRegen");
	}

	IEnumerator StartHealthRegen()
	{
		while (Health > 0)
		{
			if(hungerScript.Hunger > 50)
			{
				Health += 5;
			}
			else
			if(hungerScript.Hunger > 25)
			{
				Health += 2;
			}
			else
			{
				Health += 1;
			}

			yield return new WaitForSeconds(10.0f);
		}
	}

	public void damagePlayer(int damage)
	{
		if (!isLocalPlayer)
			return;

		Health -= damage;
		if(Health > 0)
			AudioSource.PlayClipAtPoint (gruntSound, this.gameObject.transform.position);

		if (isClient)
			CmdSyncHealth (Health);
	}

	[Command]
	void CmdSyncHealth(int hp)
	{
		Health = hp;
	}


	public void healPlayer(int amount)
	{
		if (!isLocalPlayer)
			return;

		Health += amount;

		if (isClient)
			CmdSyncHealth (Health);
	}
}
