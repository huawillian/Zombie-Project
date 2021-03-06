﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class BaseballBat_Weapon : NetworkBehaviour
{
	[SyncVar]
	public bool isEquipped;
	[SyncVar]
	public bool isAttacking;

	public Vector3 originalPosition;
	public Vector3 originalRotation;
	
	public AudioClip swingSound;
	public AudioClip hitSound;

	public Player_Stamina staminaScript;

	public GameObject weaponObject;

	// Use this for initialization
	void Start ()
	{
		originalPosition = this.transform.localPosition; // -0.3 0.1 0.2 pos 0 0 0 rot
		originalRotation = this.transform.localEulerAngles; // -0.3 0.1 0.2 pos 0 0 0 rot

		staminaScript = this.GetComponent <Player_Stamina>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!isEquipped) {
			MeshRenderer[] renderers = weaponObject.GetComponentsInChildren<MeshRenderer> ();
			
			foreach (MeshRenderer r in renderers) {
				r.enabled = false;
			}
		} else {
			MeshRenderer[] renderers = weaponObject.GetComponentsInChildren<MeshRenderer> ();
			
			foreach (MeshRenderer r in renderers) {
				r.enabled = true;
			}
		}
	}

	[Command]
	void CmdSyncAttack(bool a)
	{
		isAttacking = a;
	}

	IEnumerator Attack()
	{
		if (!isLocalPlayer && !isAttacking && isEquipped && !staminaScript.getRecoverStatus())
		{
			weaponObject.GetComponent<Animation>().Play();
			yield return new WaitForSeconds (weaponObject.GetComponent<Animation>().clip.length);
			weaponObject.transform.localPosition = originalPosition;
			weaponObject.transform.localEulerAngles = originalRotation;
			Debug.Log("Other Swing");

			yield break;
		}

		if (!isAttacking && isEquipped && !staminaScript.getRecoverStatus())
		{
			this.GetComponent<Player_Stamina>().UseStamina(20.0f);
			AudioSource.PlayClipAtPoint(swingSound, weaponObject.transform.position);
			if(!isServer)
				CmdSyncAttack(true);
			isAttacking = true;
			weaponObject.GetComponent<Animation>().Play();

			yield return new WaitForSeconds (weaponObject.GetComponent<Animation>().clip.length);
			weaponObject.transform.localPosition = originalPosition;
			weaponObject.transform.localEulerAngles = originalRotation;
			if(!isServer)
				CmdSyncAttack(false);
			isAttacking = false;

		} else
		{
			yield return new WaitForSeconds (0.01f);
		}
	}


	public void hit(Collider collider)
	{
		if (!isLocalPlayer)
			return;

		if (collider.name == "Renderer and Collider" && collider.transform.parent.name.StartsWith("Zombie")) {
			if (isAttacking) {	
				AudioSource.PlayClipAtPoint (hitSound, weaponObject.transform.position);
				this.GetComponent<Player_Noise> ().GenerateNoiseAtPlayerWithDistance (8f);
				collider.transform.parent.gameObject.GetComponent<Rigidbody>().AddForce(weaponObject.transform.forward * 300f);
				collider.gameObject.transform.parent.gameObject.GetComponent<Zombie_Health> ().damageZombie (45);
			}
		} 
	}
}
