using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player_BasicAttacks : MonoBehaviour
{
	private enum WeaponEquipped{None, Bat, Pistol};
	private WeaponEquipped equip;

	public GameObject baseballbatWeapon;
	public GameObject pistolWeapon;
	public GameObject shoveWeapon;

	public AudioClip pistolEquipSound;

	public GameObject pistolUI;
	public GameObject batUI;

	// Use this for initialization
	void Start ()
	{
		baseballbatWeapon = this.GetComponentInChildren<BaseballBat_Weapon> ().gameObject;
		pistolWeapon = this.GetComponentInChildren<Pistol_Weapon> ().gameObject;
		shoveWeapon = this.GetComponentInChildren<Shove_Weapon> ().gameObject;
		equip = WeaponEquipped.None;
		Unequip ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		LeftClick ();
		RightClick ();
	}

	private void LeftClick()
	{
		if (Input.GetMouseButtonDown(0))
		{
			switch(equip)
			{
				case WeaponEquipped.Bat:
					baseballbatWeapon.GetComponent<BaseballBat_Weapon>().StartCoroutine("Attack");
					break;
				case WeaponEquipped.Pistol:
					pistolWeapon.GetComponent<Pistol_Weapon>().StartCoroutine("Attack");
					break;
				case WeaponEquipped.None:
					shoveWeapon.GetComponent<Shove_Weapon>().StartCoroutine("Attack");
					break;
				default:
					break;
			}
		}
	}

	private void RightClick()
	{
		if (Input.GetMouseButtonDown (1)) {
			shoveWeapon.GetComponent<Shove_Weapon>().StartCoroutine("Attack");
		}
	}

	public void EquipPistol()
	{
		equip = WeaponEquipped.Pistol;

		baseballbatWeapon.GetComponent<BaseballBat_Weapon>().isEquipped = false;
		pistolWeapon.GetComponent<Pistol_Weapon>().isEquipped = true;
		batUI.SetActive (false);
		pistolUI.SetActive (true);

	}

	public void EquipBat()
	{
		equip = WeaponEquipped.Bat;

		pistolWeapon.GetComponent<Pistol_Weapon>().isEquipped = false;
		baseballbatWeapon.GetComponent<BaseballBat_Weapon>().isEquipped = true;
		batUI.SetActive (true);
		pistolUI.SetActive (false);	}

	public void Unequip()
	{
		equip = WeaponEquipped.None;

		pistolWeapon.GetComponent<Pistol_Weapon>().isEquipped = false;
		baseballbatWeapon.GetComponent<BaseballBat_Weapon>().isEquipped = false;
		batUI.SetActive (false);
		pistolUI.SetActive (false);
	}


}
