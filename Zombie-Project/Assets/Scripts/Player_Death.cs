using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player_Death : MonoBehaviour
{
	public bool isDead = false;
	Player_Inventory inventoryScript;
	public GameObject deathUI;
	public GameObject deathText;
	public Player_Timer timerScript;

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

			int minutes = Mathf.FloorToInt(timerScript.timeAlive / 60);
			int seconds = Mathf.FloorToInt(timerScript.timeAlive - minutes * 60);
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

	/*
	void OnGUI () {

		if (isDead)
		{
			Cursor.visible = true;

			// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
			if(GUI.Button(new Rect(20,40,80,20), "Respawn"))
			{
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
			}
		}
	}*/

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
	}

	public void setDeath()
	{
		isDead = true;
		inventoryScript.DropAllItems ();
	}
}
