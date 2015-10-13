using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BaseballBat_Weapon : MonoBehaviour
{
	public bool isEquipped;
	public bool isAttacking;

	public Vector3 originalPosition;
	public Vector3 originalRotation;

	public Vector3 startPosition;
	public Vector3 startRotation;

	public Vector3 endPosition;
	public Vector3 endRotation;

	public float attackDuration;

	public GameObject weaponUI;

	public AudioClip swingSound;
	public AudioClip hitSound;

	public Player_Stamina staminaScript;

	// Use this for initialization
	void Start ()
	{
		originalPosition = this.transform.localPosition; // -0.3 0.1 0.2 pos 0 0 0 rot
		originalRotation = this.transform.localEulerAngles; // -0.3 0.1 0.2 pos 0 0 0 rot

		startPosition = new Vector3 (0.3f, 0.5f, 0.8f);
		startRotation = new Vector3 (-80f, 0, 0);

		endPosition = new Vector3 (-0.3f, 0.5f, 0.8f);
		endRotation = new Vector3 (-7f, -25f, 0);	

		attackDuration = 0.3f;

		staminaScript = this.GetComponentInParent <Player_Stamina>();
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

		if (isEquipped) {
			weaponUI.GetComponent<Text>().text = "Weapon: Baseball Bat";
		}
	}

	IEnumerator Attack()
	{
		if (!isAttacking && isEquipped && staminaScript.state != Player_Stamina.StaminaState.Recover)
		{
			this.GetComponentInParent<Player_Stamina>().UseStamina(20.0f);
			AudioSource.PlayClipAtPoint(swingSound, this.transform.position);

			isAttacking = true;

			float startTime = Time.time;
			float endTime = startTime + attackDuration;

			while (Time.time < endTime)
			{
				float currTime = Time.time;
				float progress = (currTime - startTime) / attackDuration;
				this.transform.localPosition = Vector3.Lerp (startPosition, endPosition, progress);
				this.transform.localEulerAngles = Vector3.Lerp (startRotation, endRotation, progress);
				yield return new WaitForSeconds (0.01f);
			}

			this.transform.localPosition = originalPosition;
			this.transform.localEulerAngles = originalRotation;

			yield return new WaitForSeconds (0.3f);

			isAttacking = false;

		} else
		{
			yield return new WaitForSeconds (0.01f);
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.name == "zombie") {
			if(isAttacking)
			{	
				AudioSource.PlayClipAtPoint(hitSound, this.transform.position);
				this.GetComponentInParent<Player_Noise>().GenerateNoiseAtPlayerWithDistance(8f);
				collider.gameObject.transform.parent.gameObject.GetComponent<Zombie_Health>().damageZombie(50);
			}
		}
		
	}
}
