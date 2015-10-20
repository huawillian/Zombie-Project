using UnityEngine;
using System.Collections;

public class Player_BasicRotation : MonoBehaviour
{
	// Player Rotating on the y axis
	// Depends on mouse input y
	// Use this for initialization

	public float rotateSensitivity;

	void Start ()
	{
		rotateSensitivity = 5.0f;
		Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 playerRotation = this.transform.localEulerAngles;

		this.transform.localEulerAngles = new Vector3(playerRotation.x, 
		                                              playerRotation.y + Input.GetAxis("Mouse X") * rotateSensitivity, 
		                                                playerRotation.z);
	}
}
