using UnityEngine;
using System.Collections;

public class Player_Camera_BasicRotation : MonoBehaviour
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
		playerCamera = this.gameObject.GetComponentInChildren<Camera> ().gameObject;
		flashLight = this.gameObject.GetComponentInChildren<Light> ().gameObject;
		pistol = this.gameObject.GetComponentInChildren<Pistol_Weapon> ().gameObject;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 cameraRotation = playerCamera.transform.localEulerAngles;
		this.Rotation = cameraRotation.x - Input.GetAxis ("Mouse Y") * 3;
		playerCamera.transform.localEulerAngles = new Vector3(this.Rotation, 
		                                                      cameraRotation.y, 
		                                                	  cameraRotation.z);

		flashLight.transform.localEulerAngles = playerCamera.transform.localEulerAngles;

		pistol.transform.localEulerAngles = playerCamera.transform.localEulerAngles;
	}
}
