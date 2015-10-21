using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Pistol_Weapon : NetworkBehaviour
{
	public GameObject bulletPrefab;
	public bool isShooting;
	[SyncVar]
	public bool isEquipped;
	
	public Pistol_Ammo ammoScript;

	public AudioClip gunshotSound;
	public GameObject pistolObject;

	public GameObject weaponObject;

	// Use this for initialization
	void Start ()
	{
		pistolObject = weaponObject.GetComponentInChildren<Animator> ().gameObject;
		if (!isLocalPlayer)
			return;
		pistolObject.transform.parent = this.GetComponentInChildren<Camera> ().transform;
		ammoScript = this.GetComponent<Pistol_Ammo> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!isEquipped) {
			pistolObject.SetActive(false);
			MeshRenderer[] renderers = weaponObject.GetComponentsInChildren<MeshRenderer> ();
			foreach (MeshRenderer r in renderers) {
				r.enabled = false;
			}
		} else {
			pistolObject.SetActive(true);
			MeshRenderer[] renderers = weaponObject.GetComponentsInChildren<MeshRenderer> ();
			foreach (MeshRenderer r in renderers) {
				r.enabled = true;
			}
		}
	}

	IEnumerator Attack()
	{
		if (!isLocalPlayer) {
			yield break;
		}

		if (!isShooting && isEquipped && ammoScript.Ammo > 0) {

			AudioSource.PlayClipAtPoint(gunshotSound, weaponObject.transform.position);
			weaponObject.GetComponentInParent<Player_Noise>().GenerateNoiseAtPlayerWithDistance(30f);
			ammoScript.UseAmmo(1);
			isShooting = true;
			GameObject bullet = GameObject.Instantiate (bulletPrefab, weaponObject.transform.parent.GetComponentInChildren<Camera>().gameObject.transform.position + Vector3.down, Quaternion.identity) as GameObject;
			bullet.transform.parent = weaponObject.transform.parent;
			bullet.transform.localEulerAngles = weaponObject.transform.parent.GetComponentInChildren<Camera>().gameObject.transform.localEulerAngles;

			if(isServer)
				NetworkServer.Spawn(bullet);
			else
				CmdSpawnBullet(bullet);

			yield return new WaitForSeconds (0.1f);
			isShooting = false;
		} else {
			yield return new WaitForSeconds (0.01f);
		}
	}

	[Command]
	void CmdSpawnBullet(GameObject bul)
	{
		NetworkServer.Spawn(bul);

	}


}
