using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Pistol_Ammo : NetworkBehaviour
{
	public Pistol_Weapon pistolWeaponScript;
	public GameObject ammoPlacementUI;
	public GameObject ammoUIPrefab;

	public LinkedList<GameObject> bullets;

	[SerializeField, SyncVar]
	private int ammo;

	public int Ammo
	{
		get {
			return ammo;
		}

		set {
			if(value < 0) ammo = 0;
			else ammo = value;
		}
	}

	public GameObject pistolWeapon;

	// Bullets starts at (-405, -170, 0) and increments by 10 in x
	/*
	 * 	GameObject temp = (GameObject)Instantiate (ammoUIPrefab, Vector3.zero, Quaternion.identity);
		temp.transform.parent = ammoPlacementUI.transform;
		temp.GetComponent<RectTransform> ().localPosition = new Vector3 (-405, -170, 0);
	 */ 

	// Use this for initialization
	void Start ()
	{
		if (!isLocalPlayer)
			return;

		pistolWeaponScript = this.GetComponent<Pistol_Weapon> ();
		bullets = new LinkedList<GameObject>();
		AddAmmo (5);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!isLocalPlayer)
			return;

		if (pistolWeaponScript.isEquipped == false)
		{
			foreach(GameObject b in bullets)
			{
				b.GetComponent<Image>().enabled = false;
			}

		} else
		{
			foreach(GameObject b in bullets)
			{
				b.GetComponent<Image>().enabled = true;
			}
		}
	}

	public void AddAmmo(int amount)
	{
		if (!isLocalPlayer)
			return;

		float bulletXPos = -405 + Ammo * 10;

		Ammo += amount;

		for (int i=0; i<amount; i++)
		{
			GameObject temp = (GameObject)Instantiate (ammoUIPrefab, Vector3.zero, Quaternion.identity);
			temp.transform.SetParent(ammoPlacementUI.transform);
			temp.GetComponent<RectTransform> ().localScale = new Vector3(1,1,1);
			temp.GetComponent<RectTransform> ().localPosition = new Vector3 (bulletXPos, -170, 0);

			//NetworkServer.Spawn(temp);

			bullets.AddLast(temp);
			bulletXPos += 10;
		}
	}

	public void UseAmmo(int amount)
	{
		if (!isLocalPlayer)
			return;

		Ammo -= amount;

		for(int i=0; i<amount; i++)
		{
			GameObject.Destroy(bullets.Last.Value);
			bullets.RemoveLast();
		}
	}
}
