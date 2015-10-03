using UnityEngine;
using System.Collections;

public class Player_Camera_BasicRotation : MonoBehaviour
{
	// Player Camera's Basic Rotation
	// Camera based from player inherts x rotation from player's transform
	// Camera needs to set y rotation based on mouse

	public GameObject playerCamera;

	// Use this for initialization
	void Start ()
	{
		if (!playerCamera)
			playerCamera = this.gameObject.GetComponentInChildren<Camera> ().gameObject;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 cameraRotation = playerCamera.transform.localEulerAngles;
		
		playerCamera.transform.localEulerAngles = new Vector3(cameraRotation.x - Input.GetAxis("Mouse Y") * 10, 
		                                                      cameraRotation.y, 
		                                                	  cameraRotation.z);

		GameObject.Find ("Head").transform.localEulerAngles = cameraRotation;


	}
}
