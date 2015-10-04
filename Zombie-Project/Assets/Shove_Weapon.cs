using UnityEngine;
using System.Collections;

public class Shove_Weapon : MonoBehaviour
{
	public bool isShoving;
	public bool isEquipped;


	// Use this for initialization
	void Start ()
	{
		isShoving = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isShoving) {
			MeshRenderer[] renderers = this.GetComponentsInChildren<MeshRenderer> ();
			
			foreach (MeshRenderer r in renderers) {
				r.enabled = true;
			}
		} else {
			MeshRenderer[] renderers = this.GetComponentsInChildren<MeshRenderer> ();
			
			foreach (MeshRenderer r in renderers) {
				r.enabled = false;
			}
		}
	}

	IEnumerator Attack()
	{
		if (!isShoving) {
			isShoving = true;
			yield return new WaitForSeconds (0.5f);
			isShoving = false;
		} else
			yield return new WaitForSeconds (0.01f);
	}
}
