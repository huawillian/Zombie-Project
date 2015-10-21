using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_Camera_BasicRotation : NetworkBehaviour
{
	// Player Camera's Basic Rotation
	// Camera based from player inherts x rotation from player's transform
	// Camera needs to set y rotation based on mouse

	public GameObject playerCamera;
	public GameObject flashLight;
	public GameObject pistol;

	private float rotation;
	public float Rotation
	{
		get {
			return rotation;
		}
		set  {
			if(value >= 310 || value <= 60)
				rotation = value;
		}
	}

	// Use this for initialization
	void Start ()
	{
		if (!isLocalPlayer) 
		{
			playerCamera = this.gameObject.GetComponentInChildren<Camera> ().gameObject;
			playerCamera.SetActive(false);
			return;
		}
		playerCamera = this.gameObject.GetComponentInChildren<Camera> ().gameObject;
		flashLight = this.gameObject.GetComponentInChildren<Light> ().gameObject;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!isLocalPlayer)
			return;

		Vector3 cameraRotation = playerCamera.transform.localEulerAngles;
		this.Rotation = cameraRotation.x - Input.GetAxis ("Mouse Y") * 3;
		playerCamera.transform.localEulerAngles = new Vector3(this.Rotation, 
		                                                      cameraRotation.y, 
		                                                	  cameraRotation.z);

		flashLight.transform.localEulerAngles = playerCamera.transform.localEulerAngles;

		pistol.transform.localEulerAngles = playerCamera.transform.localEulerAngles;
	}
}
