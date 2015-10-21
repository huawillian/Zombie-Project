using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Player_Hunger : NetworkBehaviour
{
	public GameObject hungerUI;

	[SerializeField, SyncVar]
	private int hunger;

	public int Hunger
	{
		get {
			return hunger;
		}
		
		set {
			if(value < 0) hunger = 0;
			else if (value > 100) hunger = 100;
			else hunger = value;

			hungerUI.GetComponent<Slider>().value = hunger;
		}
	}

	// Use this for initialization
	void Start ()
	{
		if (!isLocalPlayer)
			return;

		hunger = 100;
		StartCoroutine ("HungerStart");
	}

	IEnumerator HungerStart()
	{
		while (true) {
			Hunger--;
			if (isClient)
				CmdSyncHunger (Hunger);
			yield return new WaitForSeconds(6.5f);
		}
	}

	[Command]
	void CmdSyncHunger(int hg)
	{
		Hunger = hg;
	}

	public void AddHunger(int amount)
	{
		if (!isLocalPlayer)
			return;

		Hunger += amount;

		if (isClient)
			CmdSyncHunger (Hunger);
	}
}
