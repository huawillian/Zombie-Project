using UnityEngine;
using System.Collections;

public class Player_BasicRotation : MonoBehaviour
{
	// Player Rotating on the y axis
	// Depends on mouse input y

	public GameObject player;

	// Use this for initialization
	void Start ()
	{
		if (!player)
			player = this.gameObject;

		Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 playerRotation = player.transform.localEulerAngles;

		player.transform.localEulerAngles = new Vector3(playerRotation.x, 
		                                                playerRotation.y + Input.GetAxis("Mouse X") * 20, 
		                                                playerRotation.z);
	}
}
