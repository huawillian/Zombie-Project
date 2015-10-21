using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Player_BasicAttacks : NetworkBehaviour
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
		baseballbatWeapon = this.GetComponent<BaseballBat_Weapon> ().gameObject;
		pistolWeapon = this.GetComponent<Pistol_Weapon> ().gameObject;
		shoveWeapon = this.GetComponent<Shove_Weapon> ().gameObject;
		equip = WeaponEquipped.None;

		if (!isLocalPlayer)
			return;

		Unequip ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!isLocalPlayer)
			return;

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
					if(isClient)
					{
						CmdAttackBat();
					}
					if(isServer)
					{
						RpcAttackBat();
					}
					break;
				case WeaponEquipped.Pistol:
					pistolWeapon.GetComponent<Pistol_Weapon>().StartCoroutine("Attack");
					break;
				case WeaponEquipped.None:
					shoveWeapon.GetComponent<Shove_Weapon>().StartCoroutine("Attack");
					if(isClient)
					{
						CmdAttackShove();
					}
					if(isServer)
					{
						RpcAttackShove();
					}
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
			if(isClient)
			{
				CmdAttackShove();
			}
			if(isServer)
			{
				RpcAttackShove();
			}
		}
	}

	public void EquipPistol()
	{
		if (!isLocalPlayer)
			return;

		equip = WeaponEquipped.Pistol;

		baseballbatWeapon.GetComponent<BaseballBat_Weapon>().isEquipped = false;
		pistolWeapon.GetComponent<Pistol_Weapon>().isEquipped = true;
		batUI.SetActive (false);
		pistolUI.SetActive (true);

		if (isClient) {
			CmdEquipPistol ();
		} else {
			RpcEquipPistol();
		}

	}

	public void EquipBat()
	{
		if (!isLocalPlayer)
			return;

		equip = WeaponEquipped.Bat;

		pistolWeapon.GetComponent<Pistol_Weapon>().isEquipped = false;
		baseballbatWeapon.GetComponent<BaseballBat_Weapon>().isEquipped = true;
		batUI.SetActive (true);
		pistolUI.SetActive (false);

		if (isClient) {
			CmdEquipBat ();
		} else {
			RpcEquipBat();
		}
	}

	public void Unequip()
	{
		equip = WeaponEquipped.None;

		pistolWeapon.GetComponent<Pistol_Weapon>().isEquipped = false;
		baseballbatWeapon.GetComponent<BaseballBat_Weapon>().isEquipped = false;
		batUI.SetActive (false);
		pistolUI.SetActive (false);

		if (isClient) {
			CmdUnequip ();
		} else {
			RpcUnequip();
		}
	}

	[Command]
	void CmdAttackShove()
	{
		shoveWeapon.GetComponent<Shove_Weapon>().StartCoroutine("Attack");
	}
	
	[ClientRpc]
	void RpcAttackShove()
	{
		shoveWeapon.GetComponent<Shove_Weapon>().StartCoroutine("Attack");
	}

	[Command]
	void CmdAttackBat()
	{
		baseballbatWeapon.GetComponent<BaseballBat_Weapon>().StartCoroutine("Attack");
	}
	
	[ClientRpc]
	void RpcAttackBat()
	{
		baseballbatWeapon.GetComponent<BaseballBat_Weapon>().StartCoroutine("Attack");
	}

	[Command]
	void CmdEquipBat()
	{
		equip = WeaponEquipped.Bat;
		
		baseballbatWeapon.GetComponent<BaseballBat_Weapon>().isEquipped = true;
		pistolWeapon.GetComponent<Pistol_Weapon>().isEquipped = false;
	}
	
	[ClientRpc]
	void RpcEquipBat()
	{
		equip = WeaponEquipped.Bat;
		
		baseballbatWeapon.GetComponent<BaseballBat_Weapon>().isEquipped = true;
		pistolWeapon.GetComponent<Pistol_Weapon>().isEquipped = false;
	}

	[Command]
	void CmdEquipPistol()
	{
		equip = WeaponEquipped.Pistol;
		
		baseballbatWeapon.GetComponent<BaseballBat_Weapon>().isEquipped = false;
		pistolWeapon.GetComponent<Pistol_Weapon>().isEquipped = true;
	}

	[ClientRpc]
	void RpcEquipPistol()
	{
		equip = WeaponEquipped.Pistol;
		
		baseballbatWeapon.GetComponent<BaseballBat_Weapon>().isEquipped = false;
		pistolWeapon.GetComponent<Pistol_Weapon>().isEquipped = true;
	}

	[Command]
	void CmdUnequip()
	{
		equip = WeaponEquipped.None;
		
		pistolWeapon.GetComponent<Pistol_Weapon>().isEquipped = false;
		baseballbatWeapon.GetComponent<BaseballBat_Weapon>().isEquipped = false;
	}

	[ClientRpc]
	void RpcUnequip()
	{
		equip = WeaponEquipped.None;
		
		pistolWeapon.GetComponent<Pistol_Weapon>().isEquipped = false;
		baseballbatWeapon.GetComponent<BaseballBat_Weapon>().isEquipped = false;
	}

}
