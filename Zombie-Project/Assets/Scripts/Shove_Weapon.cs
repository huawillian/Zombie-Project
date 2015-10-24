using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Shove_Weapon : NetworkBehaviour
{
	[SyncVar]
	public bool isShoving;
	[SyncVar]
	public bool isEquipped;

	public Vector3 originalPosition;
	public Vector3 originalRotation;

	public AudioClip hitSound;

	public Player_Stamina staminaScript;

	public GameObject weaponObject;

	// Use this for initialization
	void Start ()
	{
		isShoving = false;
		staminaScript = weaponObject.GetComponentInParent<Player_Stamina> ();

		originalPosition = weaponObject.transform.localPosition; // -0.3 0.1 0.2 pos 0 0 0 rot
		originalRotation = weaponObject.transform.localEulerAngles; // -0.3 0.1 0.2 pos 0 0 0 rot
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isShoving)
		{
			MeshRenderer[] renderers = weaponObject.GetComponentsInChildren<MeshRenderer> ();
			
			foreach (MeshRenderer r in renderers) {
				r.enabled = true;
			}
		} else {
			MeshRenderer[] renderers = weaponObject.GetComponentsInChildren<MeshRenderer> ();
			
			foreach (MeshRenderer r in renderers) {
				r.enabled = false;
			}
		}
	}

	[Command]
	void CmdSyncShove(bool s)
	{
		isShoving = s;
	}

	IEnumerator Attack()
	{
		if (!isLocalPlayer && !staminaScript.getRecoverStatus()) {
			CmdSyncShove(true);
			isShoving = true;
			weaponObject.GetComponent<Animation>().Play();
			
			yield return new WaitForSeconds (weaponObject.GetComponent<Animation>().clip.length);
			weaponObject.transform.localPosition = originalPosition;
			weaponObject.transform.localEulerAngles = originalRotation;
			CmdSyncShove(false);

			isShoving = false;
			yield break;
		}

		if (!isShoving && !staminaScript.getRecoverStatus())
		{
			this.GetComponent<Player_Stamina>().UseStamina(10.0f);
			CmdSyncShove(true);

			isShoving = true;
			weaponObject.GetComponent<Animation>().Play();
			
			yield return new WaitForSeconds (weaponObject.GetComponent<Animation>().clip.length);
			weaponObject.transform.localPosition = originalPosition;
			weaponObject.transform.localEulerAngles = originalRotation;
			CmdSyncShove(false);

			isShoving = false;
		} else
			yield return new WaitForSeconds (0.01f);
	}


	public void hit(Collider collider)
	{
		if (!isLocalPlayer)
			return;

		if (collider.name == "Renderer and Collider" && collider.transform.parent.name.StartsWith("Zombie")) {
			if (isShoving) {	
				AudioSource.PlayClipAtPoint (hitSound, weaponObject.transform.position);
				Debug.Log ("Adding force, shoving zombie away from player");
				collider.transform.parent.gameObject.GetComponent<Zombie_Health> ().damageZombie (20);
				collider.transform.parent.gameObject.GetComponent<Rigidbody> ().AddForce (weaponObject.transform.forward * 300f);
			}
		} 
	}
}
