using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_BasicRotation : NetworkBehaviour
{
	// Player Rotating on the y axis
	// Depends on mouse input y
	// Use this for initialization

	public float rotateSensitivity;

	[SyncVar, SerializeField]
	private Vector3 rot;

	void Start ()
	{
		if (!isLocalPlayer)
			return;

		rotateSensitivity = 5.0f;
		Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!isLocalPlayer) {
			this.transform.localEulerAngles = rot;
			return;
		}
		Vector3 playerRotation = this.transform.localEulerAngles;

		this.transform.localEulerAngles = new Vector3(playerRotation.x, 
		                                              playerRotation.y + Input.GetAxis("Mouse X") * rotateSensitivity, 
		                                                playerRotation.z);

		if (isClient) {
			CmdSetRotOnServer (this.transform.localEulerAngles);
		} else {
			rot = this.transform.localEulerAngles;
		}
	}

	[Command]
	void CmdSetRotOnServer(Vector3 rotation)
	{
		rot = rotation;
		this.transform.localEulerAngles = rot;
	}
}
