using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player_Death : MonoBehaviour
{

	Player_Inventory inventoryScript;
	public GameObject deathUI;
	public GameObject deathText;
	public Player_Timer timerScript;

	public AudioClip deathSound;
	public bool isDead = false;

	// Use this for initialization
	void Start ()
	{
		inventoryScript = this.GetComponent<Player_Inventory> ();
		timerScript = this.GetComponent<Player_Timer> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isDead) {
			deathUI.SetActive (true);
			Cursor.visible = true;

			int minutes = Mathf.FloorToInt(timerScript.getTimeAlive() / 60);
			int seconds = Mathf.FloorToInt(timerScript.getTimeAlive() - minutes * 60);
			string r = (minutes < 10) ? "0" + minutes.ToString() : minutes.ToString();
			r += ":";
			r += (seconds < 10) ? "0" + seconds.ToString() : seconds.ToString();

			if(minutes == 1)
			{
				deathText.GetComponent<Text>().text = "You lasted " + minutes + " minute and " + seconds + " seconds";
			}
			else
			{
				deathText.GetComponent<Text>().text = "You lasted " + minutes + " minutes and " + seconds + " seconds";
			}
		} else 
		{
			deathUI.SetActive(false);
		}
	}

	public void RespawnOnClick()
	{
		deathUI.SetActive(false);
		this.gameObject.GetComponent<Player_Health>().Health = 100;
		this.gameObject.transform.position = Vector3.zero;
		
		this.gameObject.GetComponent<Player_Death>().isDead = false;
		Cursor.visible = false;
		
		this.gameObject.GetComponent<Player_BasicAttacks>().enabled = true;
		this.gameObject.GetComponent<Player_BasicMovement>().enabled = true;
		this.gameObject.GetComponent<Player_BasicRotation>().enabled = true;
		this.gameObject.GetComponent<Player_Camera_BasicRotation>().enabled = true;
		
		this.gameObject.transform.localEulerAngles = new Vector3(0,0,0);
		this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
		
		this.gameObject.GetComponent<Player_Timer>().StartTimer();

		this.gameObject.GetComponent<Player_Stamina>().enabled = true;
		this.gameObject.GetComponent<Player_ActionUI>().enabled = true;
		this.gameObject.GetComponent<Player_Search>().enabled = true;
		this.gameObject.GetComponent<Inventory_PickUp>().enabled = true;
		this.gameObject.GetComponent<Player_Hunger>().enabled = true;
		this.gameObject.GetComponent<Player_Inventory>().enabled = true;

		this.gameObject.GetComponent<Player_Hunger> ().Hunger = 100;
		this.gameObject.GetComponent<Player_Stamina>().Stamina = 100;
		this.gameObject.GetComponent<Inventory_PickUp> ().itemsInRange.Clear ();
		this.gameObject.GetComponent<Player_BasicAttacks> ().Unequip ();
		this.gameObject.GetComponentInChildren<Pistol_Ammo> ().UseAmmo (this.gameObject.GetComponentInChildren<Pistol_Ammo> ().Ammo);

		this.gameObject.GetComponent<Instruction_Disable> ().ShowInstructions ();
		this.gameObject.GetComponentInChildren<Person_AnimationController>().SetRespawn();

	}

	public void setDeath()
	{
		this.gameObject.GetComponent<Player_BasicAttacks>().enabled = false;
		this.gameObject.GetComponent<Player_BasicMovement>().enabled = false;
		this.gameObject.GetComponent<Player_BasicRotation>().enabled = false;
		this.gameObject.GetComponent<Player_Camera_BasicRotation>().enabled = false;
		
		this.gameObject.transform.localEulerAngles = new Vector3(0,0,-90);
		this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
		
		this.gameObject.GetComponent<Player_Timer>().StopTimer();
		
		this.gameObject.GetComponent<Player_Stamina>().enabled = false;
		this.gameObject.GetComponent<Player_ActionUI>().enabled = false;
		this.gameObject.GetComponent<Player_Search>().enabled = false;
		this.gameObject.GetComponent<Inventory_PickUp>().enabled = false;
		this.gameObject.GetComponent<Player_Hunger>().enabled = false;
		this.gameObject.GetComponent<Player_Inventory>().enabled = false;
		
		this.gameObject.GetComponentInChildren<Person_AnimationController>().SetDeath();
		AudioSource.PlayClipAtPoint (deathSound, this.gameObject.transform.position);

		isDead = true;
		inventoryScript.DropAllItems ();
	}
}
