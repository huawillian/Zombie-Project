using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Pistol_Ammo : MonoBehaviour
{
	public Pistol_Weapon pistolWeaponScript;
	public GameObject ammoPlacementUI;
	public GameObject ammoUIPrefab;

	public LinkedList<GameObject> bullets;

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

	// Bullets starts at (-405, -170, 0) and increments by 10 in x
	/*
	 * 	GameObject temp = (GameObject)Instantiate (ammoUIPrefab, Vector3.zero, Quaternion.identity);
		temp.transform.parent = ammoPlacementUI.transform;
		temp.GetComponent<RectTransform> ().localPosition = new Vector3 (-405, -170, 0);
	 */ 

	// Use this for initialization
	void Start ()
	{
		ammo = 5;
		bullets = new LinkedList<GameObject>();

		float bulletXPos = -405;
		for (int i=0; i<ammo; i++)
		{
			GameObject temp = (GameObject)Instantiate (ammoUIPrefab, Vector3.zero, Quaternion.identity);
			temp.transform.SetParent(ammoPlacementUI.transform);
			temp.GetComponent<RectTransform> ().localPosition = new Vector3 (bulletXPos, -170, 0);
			bullets.AddLast(temp);

			bulletXPos += 10;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
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
		float bulletXPos = -415 + Ammo * 10;

		Ammo += amount;

		for (int i=0; i<amount; i++)
		{
			GameObject temp = (GameObject)Instantiate (ammoUIPrefab, Vector3.zero, Quaternion.identity);
			temp.transform.SetParent(ammoPlacementUI.transform);
			temp.GetComponent<RectTransform> ().localPosition = new Vector3 (bulletXPos, -170, 0);
			bullets.AddLast(temp);
			bulletXPos += 10;
		}
	}

	public void UseAmmo(int amount)
	{
		Ammo -= amount;

		for(int i=0; i<amount; i++)
		{
			GameObject.Destroy(bullets.Last.Value);
			bullets.RemoveLast();
		}
	}
}
