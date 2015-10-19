using UnityEngine;
using System.Collections;

public class Shove_Weapon : MonoBehaviour
{
	public bool isShoving;
	public bool isEquipped;

	public Vector3 originalPosition;
	public Vector3 originalRotation;
	
	public Vector3 startPosition;
	public Vector3 startRotation;
	
	public Vector3 endPosition;
	public Vector3 endRotation;

	public AudioClip hitSound;

	public Player_Stamina staminaScript;

	// Use this for initialization
	void Start ()
	{
		isShoving = false;
		staminaScript = this.GetComponentInParent<Player_Stamina> ();

		originalPosition = this.transform.localPosition; // -0.3 0.1 0.2 pos 0 0 0 rot
		originalRotation = this.transform.localEulerAngles; // -0.3 0.1 0.2 pos 0 0 0 rot
		
		startPosition = new Vector3 (0, 0, -2.0f);
		startRotation = new Vector3 (0, 0, 0);
		
		endPosition = new Vector3 (0, 0, 0.1f);
		endRotation = new Vector3 (0, 0, 0);	
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
			staminaScript.UseStamina(10.0f);

			isShoving = true;

			float startTime = Time.time;
			float endTime = startTime + 0.2f;
			
			while (Time.time < endTime)
			{
				float currTime = Time.time;
				float progress = (currTime - startTime) / 0.2f;
				this.transform.localPosition = Vector3.Lerp (startPosition, endPosition, progress);
				this.transform.localEulerAngles = Vector3.Lerp (startRotation, endRotation, progress);
				yield return new WaitForSeconds (0.01f);
			}

			yield return new WaitForSeconds(0.3f);

			this.transform.localPosition = originalPosition;
			this.transform.localEulerAngles = originalRotation;

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

				StartCoroutine("ShoveZombie", collider.gameObject);
			}
		}
		else
		if (collider.name == "Box")
		{
			if (isShoving) {	
				AudioSource.PlayClipAtPoint (hitSound, this.transform.position);
				collider.gameObject.GetComponent<Rigidbody>().AddExplosionForce(1000,  collider.transform.position - this.transform.forward, 5);
				this.GetComponentInParent<Player_Noise> ().GenerateNoiseAtPlayerWithDistance (3f);
				collider.gameObject.GetComponent<Box_Controller> ().Health -= 15;
			}
		}
	}

	IEnumerator ShoveZombie(GameObject zombie)
	{
		zombie.transform.parent.gameObject.GetComponent<Zombie_Health>().damageZombie(5);
		zombie.transform.parent.gameObject.GetComponent<Rigidbody>().AddForce(this.transform.forward * 500f);

		yield return new WaitForSeconds (0.8f);
		if (zombie)
		{
			zombie.transform.parent.gameObject.GetComponent<Zombie_Health>().damageZombie(5);
			zombie.transform.parent.gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		}

		yield return new WaitForSeconds (0.8f);
		if (zombie)
		{
			zombie.transform.parent.gameObject.GetComponent<Zombie_Health>().damageZombie(5);
			zombie.transform.parent.gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		}
	}

}
