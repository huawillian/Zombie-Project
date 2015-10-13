using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Pistol_Weapon : MonoBehaviour
{
	public GameObject bulletPrefab;
	public bool isShooting;
	public bool isEquipped;

	public GameObject weaponUI;

	public Pistol_Ammo ammoScript;

	public AudioClip gunshotSound;
	public GameObject pistolObject;
	
	// Use this for initialization
	void Start ()
	{
		pistolObject = this.GetComponentInChildren<Animator> ().gameObject;
		pistolObject.transform.parent = this.transform.parent.GetComponentInChildren<Camera> ().transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!isEquipped) {

			pistolObject.SetActive(false);

			MeshRenderer[] renderers = this.GetComponentsInChildren<MeshRenderer> ();
			
			foreach (MeshRenderer r in renderers) {
				r.enabled = false;
			}
		} else {
			pistolObject.SetActive(true);

			MeshRenderer[] renderers = this.GetComponentsInChildren<MeshRenderer> ();
			
			foreach (MeshRenderer r in renderers) {
				r.enabled = true;
			}
		}

		if (isEquipped) {
			weaponUI.GetComponent<Text>().text = "Weapon: Pistol";
		}
	}

	IEnumerator Attack()
	{
		if (!isShooting && isEquipped && ammoScript.Ammo > 0) {

			AudioSource.PlayClipAtPoint(gunshotSound, this.transform.position);
			this.GetComponentInParent<Player_Noise>().GenerateNoiseAtPlayerWithDistance(30f);
			ammoScript.AddAmmo(-1);
			isShooting = true;
			GameObject bullet = GameObject.Instantiate (bulletPrefab, this.transform.parent.GetComponentInChildren<Camera>().gameObject.transform.position + Vector3.down, Quaternion.identity) as GameObject;
			bullet.transform.parent = this.transform.parent;
			bullet.transform.localEulerAngles = this.transform.parent.GetComponentInChildren<Camera>().gameObject.transform.localEulerAngles;
			yield return new WaitForSeconds (0.1f);
			isShooting = false;
		} else {
			yield return new WaitForSeconds (0.01f);
		}
	}
}
