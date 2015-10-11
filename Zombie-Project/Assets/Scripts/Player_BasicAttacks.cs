using UnityEngine;
using System.Collections;

public class Player_BasicAttacks : MonoBehaviour
{
	public bool isBatEquipped;
	public bool isPistolEquipped;
	public bool isShoveEquipped;

	public GameObject baseballbatWeapon;
	public GameObject pistolWeapon;
	public GameObject shoveWeapon;

	public AudioClip pistolEquipSound;

	// Use this for initialization
	void Start ()
	{
		baseballbatWeapon = GameObject.Find ("Baseball Bat Weapon");
		pistolWeapon = GameObject.Find ("Pistol Weapon");
		shoveWeapon = GameObject.Find ("Shove Weapon");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Debug.Log("Primary Attack.");

			if(isBatEquipped)
			{
				baseballbatWeapon.GetComponent<BaseballBat_Weapon>().StartCoroutine("Attack");
			}

			if(isPistolEquipped)
			{
				pistolWeapon.GetComponent<Pistol_Weapon>().StartCoroutine("Attack");
			}
		}

		if (Input.GetMouseButtonDown (1)) {
			Debug.Log("Right Click");
			shoveWeapon.GetComponent<Shove_Weapon>().StartCoroutine("Attack");
		}

		/*
		if (Input.GetKeyDown (KeyCode.Q)) {
			if(isPistolEquipped)
			{
				isPistolEquipped = false;
				pistolWeapon.GetComponent<Pistol_Weapon>().isEquipped = false;
				isBatEquipped = true;
				baseballbatWeapon.GetComponent<BaseballBat_Weapon>().isEquipped = true;
			}
			else
			{
				isBatEquipped = false;
				baseballbatWeapon.GetComponent<BaseballBat_Weapon>().isEquipped = false;
				isPistolEquipped = true;
				pistolWeapon.GetComponent<Pistol_Weapon>().isEquipped = true;
			}
		}*/

	}

	public void EquipPistol()
	{
		if(!isPistolEquipped)
			AudioSource.PlayClipAtPoint (pistolEquipSound, this.transform.position);

		isBatEquipped = false;
		baseballbatWeapon.GetComponent<BaseballBat_Weapon>().isEquipped = false;
		isPistolEquipped = true;
		pistolWeapon.GetComponent<Pistol_Weapon>().isEquipped = true;
	}

	public void EquipBat()
	{
		isPistolEquipped = false;
		pistolWeapon.GetComponent<Pistol_Weapon>().isEquipped = false;
		isBatEquipped = true;
		baseballbatWeapon.GetComponent<BaseballBat_Weapon>().isEquipped = true;
	}

	public void Unequip()
	{
		isPistolEquipped = false;
		pistolWeapon.GetComponent<Pistol_Weapon>().isEquipped = false;
		isBatEquipped = false;
		baseballbatWeapon.GetComponent<BaseballBat_Weapon>().isEquipped = false;
	}


}
