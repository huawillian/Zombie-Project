using UnityEngine;
using System.Collections;

public class Player_BasicMovement : MonoBehaviour
{
	// Shift for sprinting
	// Ctrl for crouching
	// Space for jumping
	// Alt for tilting
	// WASD for basic movement
	public GameObject player;
	public float defaultSpeed;
	public float sprintSpeed;
	public float crouchSpeed;
	public float jumpForce;
	public float tiltSpeed;

	public bool isJumping;
	public bool isCrouching;
	public bool isSprinting;
	public bool isTilting;

	public GameObject camera;

	// Use this for initialization
	void Start ()
	{
		if (!player)
			player = this.gameObject;

		if (defaultSpeed == 0)
			defaultSpeed = 10;

		if (sprintSpeed == 0)
			sprintSpeed = 30;

		if (crouchSpeed == 0)
			crouchSpeed = 6;

		if (jumpForce == 0)
			jumpForce = 200;

		if (tiltSpeed == 0)
			tiltSpeed = 0;

		if(!this.camera)
			this.camera = this.gameObject.GetComponentInChildren<Camera> ().gameObject;

	}
	
	// Update is called once per frame
	void Update ()
	{
		// WASD controller
		if(Input.GetKey(KeyCode.W))
		{
			//player.transform.position += this.GetComponent<Camera>().transform.forward;

			if(isSprinting)
			{
				player.GetComponent<Rigidbody> ().AddForce(player.transform.forward * sprintSpeed);
			}
			else
			if(isCrouching)
			{
				player.GetComponent<Rigidbody> ().AddForce(player.transform.forward * crouchSpeed);
			}
			else
			if(isJumping || isTilting)
			{
				//player.GetComponent<Rigidbody> ().AddForce(Vector3.zero);
			}
			else
			{
				player.GetComponent<Rigidbody> ().AddForce(player.transform.forward * defaultSpeed);
			}
		}
		if(Input.GetKey(KeyCode.A))
		{
			//player.transform.position -= this.GetComponent<Camera>().transform.right;

			if(isSprinting)
			{
				player.GetComponent<Rigidbody> ().AddForce(player.transform.right * sprintSpeed * -1.0f);
			}
			else
				if(isCrouching)
			{
				player.GetComponent<Rigidbody> ().AddForce(player.transform.right * crouchSpeed  * -1.0f);
			}
			else
				if(isJumping || isTilting)
			{
				//player.GetComponent<Rigidbody> ().AddForce(Vector3.zero);
			}
			else
			{
				player.GetComponent<Rigidbody> ().AddForce(player.transform.right * defaultSpeed * -1.0f);
			}
		}
		if(Input.GetKey(KeyCode.S))
		{
			//player.transform.position -= this.GetComponent<Camera>().transform.forward;

			if(isSprinting)
			{
				player.GetComponent<Rigidbody> ().AddForce(player.transform.forward * sprintSpeed * -1.0f);
			}
			else
				if(isCrouching)
			{
				player.GetComponent<Rigidbody> ().AddForce(player.transform.forward * crouchSpeed  * -1.0f);
			}
			else
				if(isJumping || isTilting)
			{
				//player.GetComponent<Rigidbody> ().AddForce(Vector3.zero);
			}
			else
			{
				player.GetComponent<Rigidbody> ().AddForce(player.transform.forward * defaultSpeed * -1.0f);
			}
		}
		if(Input.GetKey(KeyCode.D))
		{
			//player.transform.position += this.GetComponent<Camera>().transform.right;
			if(isSprinting)
			{
				player.GetComponent<Rigidbody> ().AddForce(player.transform.right * sprintSpeed);
			}
			else
				if(isCrouching)
			{
				player.GetComponent<Rigidbody> ().AddForce(player.transform.right * crouchSpeed);
			}
			else
				if(isJumping || isTilting)
			{
				//player.GetComponent<Rigidbody> ().AddForce(Vector3.zero);
			}
			else
			{
				player.GetComponent<Rigidbody> ().AddForce(player.transform.right * defaultSpeed);
			}
		}

		// Shift
		if (Input.GetKey (KeyCode.LeftShift)) {
			if (!isCrouching && !isTilting && !isJumping)
				isSprinting = true;
		} else {
			isSprinting = false;
		}

		// Ctrl
		if (Input.GetKey (KeyCode.LeftControl)) 
		{
			if(!isSprinting && !isTilting && !isJumping)
				isCrouching = true;
		} else {
			isCrouching = false;
		}

		// Alt
		if (Input.GetKey (KeyCode.LeftAlt)) 
		{
			if(!isSprinting && !isCrouching && !isJumping)
				isTilting = true;
		} else {
			isTilting = false;
		}

		if (isCrouching) {
			player.transform.localScale = new Vector3(1, 0.5f, 1);
		} else {
			player.transform.localScale = new Vector3(1, 1, 1);
		}

		// Space
		if (Input.GetKeyDown (KeyCode.Space)) {
			player.GetComponent<Rigidbody> ().AddForce(player.transform.up * jumpForce);
		}


		if (isTilting) {
			this.camera.transform.localPosition = new Vector3 (0.5f, -0.2f, 0);
		} else {
			this.camera.transform.localPosition = Vector3.zero;
		}
	}
}
