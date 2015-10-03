using UnityEngine;
using System.Collections;

public class Camera_FirstPersonView : MonoBehaviour
{
	// Set the Camera and the Player
	public GameObject camera;
	public GameObject player;


	// Use this for initialization
	void Start ()
	{
		player.GetComponent<Rigidbody> ().useGravity = false;
		MeshRenderer[] playerRenderers = player.GetComponentsInChildren<MeshRenderer> ();

		foreach (MeshRenderer r in playerRenderers) {
			r.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Set Camera at Player's location every frame
		if (this.camera && this.player) 
		{
			this.camera.transform.position = this.player.transform.position;

			this.camera.transform.localEulerAngles = new Vector3(this.camera.transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * 10, this.camera.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * 10, this.camera.transform.localEulerAngles.z);

			if(Input.GetKey(KeyCode.W))
			{
				player.transform.position += this.camera.transform.forward;
			}
			if(Input.GetKey(KeyCode.A))
			{
				player.transform.position -= this.camera.transform.right;
			}
			if(Input.GetKey(KeyCode.S))
			{
				player.transform.position -= this.camera.transform.forward;
			}
			if(Input.GetKey(KeyCode.D))
			{
				player.transform.position += this.camera.transform.right;
			}


		} else 
		{
			Debug.Log(this.ToString() + "Camera or Player is not set...");
		}
	}
}
