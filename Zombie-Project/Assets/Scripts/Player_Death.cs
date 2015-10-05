using UnityEngine;
using System.Collections;

public class Player_Death : MonoBehaviour
{
	public bool isDead = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {

		if (isDead)
		{
			// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
			if(GUI.Button(new Rect(20,40,80,20), "Respawn"))
			{
				this.gameObject.GetComponent<Player_Health>().Health = 100;
				this.gameObject.transform.position = Vector3.zero;

				this.gameObject.GetComponent<Player_Death>().isDead = false;
				
				this.gameObject.GetComponent<Player_BasicAttacks>().enabled = true;
				this.gameObject.GetComponent<Player_BasicMovement>().enabled = true;
				this.gameObject.GetComponent<Player_BasicRotation>().enabled = true;
				this.gameObject.GetComponent<Player_Camera_BasicRotation>().enabled = true;
			
			}
		}
	}
}
