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

	public bool isJumping;
	public bool isCrouching;
	public bool isSprinting;

	public GameObject camera;

	public AudioClip walkSound;
	public AudioClip runSound;

	public AudioSource walkSource;
	public AudioSource runSource;

	public Player_Stamina staminaScript;

	public bool isJumpReady;
	public bool isGrounded;

	// Use this for initialization
	void Start ()
	{
		walkSource = this.gameObject.AddComponent<AudioSource>();
		runSource = this.gameObject.AddComponent<AudioSource>();
		walkSource.clip = walkSound;
		walkSource.loop = true;
		runSource.clip = runSound;
		runSource.loop = true;


		if (!player)
			player = this.gameObject;

		if (defaultSpeed == 0)
			defaultSpeed = 10;

		if (sprintSpeed == 0)
			sprintSpeed = 30;

		if (crouchSpeed == 0)
			crouchSpeed = 6;

		if (jumpForce == 0)
			jumpForce = 400;


		if(!this.camera)
			this.camera = this.gameObject.GetComponentInChildren<Camera> ().gameObject;

		staminaScript = this.GetComponent <Player_Stamina>();
		StartCoroutine ("SprintStamina");

		isJumpReady = true;
		isGrounded = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Play Walking and Running Sound when grounded and moving
		if (isGrounded) {
			if ((Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.D)) && !isSprinting) {

				if (!walkSource.isPlaying)
					walkSource.Play ();
			} else {
				if (walkSource.isPlaying) 
					walkSource.Stop ();
			}

			if (isSprinting) {
				if (walkSource.isPlaying)
					walkSource.Stop ();

				if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.D)) {
					if (!runSource.isPlaying)
						runSource.Play ();
				} else {
					if (runSource.isPlaying) 
						runSource.Stop ();
				}
			} else {
				if (runSource.isPlaying) 
					runSource.Stop ();
			}
		}

		// Basic Movement for the player when WASD is pressed
		// Movement speed depends on Player state, crouching, sprinting, jumping, walking
		if (isGrounded) {
			// WASD controller
			if (Input.GetKey (KeyCode.W)) {
				if (isSprinting) {
					player.GetComponent<Rigidbody> ().velocity = player.transform.forward * 10f + new Vector3(0, player.GetComponent<Rigidbody>().velocity.y, 0);
				} else
			if (isCrouching) {
					player.GetComponent<Rigidbody> ().velocity = player.transform.forward * 2f + new Vector3(0, player.GetComponent<Rigidbody>().velocity.y, 0);
				} else
			if (isJumping) {
				} else {
					player.GetComponent<Rigidbody> ().velocity = player.transform.forward * 5f + new Vector3(0, player.GetComponent<Rigidbody>().velocity.y, 0);
				}
			}
			if (Input.GetKey (KeyCode.A)) {
				if (isSprinting) {
					player.GetComponent<Rigidbody> ().velocity = player.transform.right * -10f + new Vector3(0, player.GetComponent<Rigidbody>().velocity.y, 0);
				} else
				if (isCrouching) {
					player.GetComponent<Rigidbody> ().velocity = player.transform.right * -2f + new Vector3(0, player.GetComponent<Rigidbody>().velocity.y, 0);
				} else
				if (isJumping) {
				} else {
					player.GetComponent<Rigidbody> ().velocity = player.transform.right * -5f + new Vector3(0, player.GetComponent<Rigidbody>().velocity.y, 0);
				}
			}
			if (Input.GetKey (KeyCode.S)) {
				if (isSprinting) {
					player.GetComponent<Rigidbody> ().velocity = player.transform.forward * -10f + new Vector3(0, player.GetComponent<Rigidbody>().velocity.y, 0);
				} else
				if (isCrouching) {
					player.GetComponent<Rigidbody> ().velocity = player.transform.forward * -2f + new Vector3(0, player.GetComponent<Rigidbody>().velocity.y, 0);
				} else
				if (isJumping) {
				} else {
					player.GetComponent<Rigidbody> ().velocity = player.transform.forward * -5f + new Vector3(0, player.GetComponent<Rigidbody>().velocity.y, 0);
				}
			}
			if (Input.GetKey (KeyCode.D)) {
				if (isSprinting) {
					player.GetComponent<Rigidbody> ().velocity = player.transform.right * 10f + new Vector3(0, player.GetComponent<Rigidbody>().velocity.y, 0);
				} else
				if (isCrouching) {
					player.GetComponent<Rigidbody> ().velocity = player.transform.right * 2f + new Vector3(0, player.GetComponent<Rigidbody>().velocity.y, 0);
				} else
				if (isJumping) {
				} else {
					player.GetComponent<Rigidbody> ().velocity = player.transform.right * 5f + new Vector3(0, player.GetComponent<Rigidbody>().velocity.y, 0);
				}
			}
		}

		// Shift
		if (Input.GetKey (KeyCode.LeftShift))
		{
			if (!isCrouching && !isJumping && staminaScript.state != Player_Stamina.StaminaState.Recover)
				isSprinting = true;
		} else {
			isSprinting = false;
		}

		// Ctrl
		if (Input.GetKey (KeyCode.LeftControl)) 
		{
			if(!isSprinting && !isJumping)
				isCrouching = true;
		} else {
			isCrouching = false;
		}

		// Crouching sets player height to lower value
		if (isCrouching) {
			player.transform.localScale = new Vector3(1, 0.5f, 1);
		} else {
			player.transform.localScale = new Vector3(1, 1, 1);
		}

		// Space
		if (Input.GetKeyDown (KeyCode.Space))
		{
			if(staminaScript.state != Player_Stamina.StaminaState.Recover && isJumpReady && isGrounded && !isCrouching)
			{
				isJumpReady = false;
				isJumping = true;

				// Set isJumpReady to true after 1 second
				StartCoroutine("ResetJump");
				staminaScript.UseStamina(10.0f);
				player.GetComponent<Rigidbody> ().AddForce(player.transform.up * jumpForce);
			}
		}

		// Set is Grounded
		if (Physics.Raycast (transform.position, Vector3.down, 2.5f)) {
			isGrounded = true;
			isJumping = false;
		}
	}

	IEnumerator ResetJump()
	{
		yield return new WaitForSeconds (1.0f);
		isJumpReady = true;
	}

	IEnumerator SprintStamina()
	{
		while (true) {
			yield return new WaitForSeconds(0.1f);
			if(isSprinting)
			{
				staminaScript.UseStamina(2.0f);
			}
		}
	}
}
