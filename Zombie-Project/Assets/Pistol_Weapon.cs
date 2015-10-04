using UnityEngine;
using System.Collections;

public class Pistol_Weapon : MonoBehaviour
{
	public GameObject bulletPrefab;
	public bool isShooting;
	public bool isEquipped;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!isEquipped) {
			MeshRenderer[] renderers = this.GetComponentsInChildren<MeshRenderer> ();
			
			foreach (MeshRenderer r in renderers) {
				r.enabled = false;
			}
		} else {
			MeshRenderer[] renderers = this.GetComponentsInChildren<MeshRenderer> ();
			
			foreach (MeshRenderer r in renderers) {
				r.enabled = true;
			}
		}
	}

	IEnumerator Attack()
	{
		if (!isShooting && isEquipped) {
			isShooting = true;
			GameObject bullet = GameObject.Instantiate (bulletPrefab, this.transform.position, Quaternion.identity) as GameObject;
			bullet.transform.parent = this.transform;

			yield return new WaitForSeconds (0.1f);
			isShooting = false;
		} else {
			yield return new WaitForSeconds (0.01f);
		}
	}
}
