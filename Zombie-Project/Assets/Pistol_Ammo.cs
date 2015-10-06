using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Pistol_Ammo : MonoBehaviour
{
	public GameObject ammoUI;
	public Pistol_Weapon pistolWeaponScript;

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

	// Use this for initialization
	void Start ()
	{
		ammo = 10;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (pistolWeaponScript.isEquipped == false) {
			ammoUI.SetActive(false);
		} else {
			ammoUI.SetActive(true);

			ammoUI.GetComponent<Text>().text = "Ammo: " + Ammo;

		}
	}

	public void AddAmmo(int amount)
	{
		Ammo += amount;
	}
}
