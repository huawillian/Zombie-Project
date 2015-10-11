using UnityEngine;
using System.Collections;

public class Shove_Weapon : MonoBehaviour
{
	public bool isShoving;
	public bool isEquipped;

	public AudioClip hitSound;

	public Player_Stamina staminaScript;

	// Use this for initialization
	void Start ()
	{
		isShoving = false;
		staminaScript = this.GetComponentInParent<Player_Stamina> ();
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
		if (!isShoving && staminaScript.state != Player_Stamina.StaminaState.Recover)
		{
			staminaScript.UseStamina(20.0f);

			isShoving = true;
			yield return new WaitForSeconds (0.5f);
			isShoving = false;
		} else
			yield return new WaitForSeconds (0.01f);
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.name == "zombie") {
			if(isShoving)
			{	
				AudioSource.PlayClipAtPoint(hitSound, this.transform.position);
				Debug.Log("Adding force, shoving zombie away from player");
				collider.gameObject.transform.parent.gameObject.GetComponent<Rigidbody>().AddForce(this.transform.forward * 8000f);
			}
		
		}
	}
}
