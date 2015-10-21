using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_BasicMovement : NetworkBehaviour
{
	#pragma warning disable 0108
	// Rigidbody on Player
	private Rigidbody rigidbody;

	// Movement states
	private enum MovementState {Walking, Sprinting, Crouching};
	private MovementState state;

	// Default speed for movement
	public float walkSpeed;
	public float sprintSpeed;
	public float crouchSpeed;
	public float jumpForce;

	// Player flags for jumping
	private bool isGrounded;
	private bool isJumpReady;

	// Player walking and running sound variables
	public AudioClip walkSound;
	public AudioClip runSound;
	public AudioSource walkSource;
	public AudioSource runSource;

	// References to other scripts/objects on the player
	public Player_Stamina staminaScript;
	#pragma warning restore 0108

	[SyncVar, SerializeField]
	private Vector3 Pos;

	// Use this for initialization
	void Start ()
	{
		if (!isLocalPlayer) {
			return;
		}

		this.rigidbody = this.GetComponent<Rigidbody> ();

		state = MovementState.Walking;

		walkSpeed = 6;
		sprintSpeed = 10;
		crouchSpeed = 2;
		jumpForce = 400;

		isGrounded = true;
		isJumpReady = true;

		walkSource = this.gameObject.AddComponent<AudioSource>();
		runSource = this.gameObject.AddComponent<AudioSource>();
		walkSource.clip = walkSound;
		runSource.clip = runSound;
		walkSource.loop = true;
		runSource.loop = true;

		staminaScript = this.GetComponent <Player_Stamina>();
		StartCoroutine ("SprintStamina");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!isLocalPlayer)
		{
			this.transform.position = Pos;
			return;
		}

		PlayMovementSounds ();
		MoveWASD ();
		SetMovementState ();
		MoveJump ();
		SetGrounded ();
		MoveCrouch ();

		if (isClient) {
			CmdSetPosOnServer (this.transform.position);
		} else {
			Pos = this.transform.position;
		}
	}

	[Command]
	void CmdSetPosOnServer(Vector3 posix)
	{
		Pos = posix;
		this.transform.position = Pos;
	}
	
	// Crouching sets player height to lower value
	private void MoveCrouch()
	{
		if (state == MovementState.Crouching) {
			this.transform.localScale = new Vector3(1, 0.5f, 1);
		} else {
			this.transform.localScale = new Vector3(1, 1, 1);
		}
	}

	// Set is Grounded
	private void SetGrounded()
	{
		if (Physics.Raycast (transform.position, Vector3.down, 2.5f)) {
			isGrounded = true;
		}
	}

	// Space
	private void MoveJump()
	{
		if (Input.GetKeyDown (KeyCode.Space))
		{
			if(!staminaScript.getRecoverStatus() && isJumpReady && isGrounded)
			{
				isJumpReady = false;
				// Set isJumpReady to true after 0.5f second
				StartCoroutine("ResetJump");
				staminaScript.UseStamina(10.0f);
				this.rigidbody.AddForce(this.transform.up * jumpForce);
			}
		}
	}

	// Play Walking and Running Sound when grounded and moving
	private void PlayMovementSounds()
	{
		if (!isGrounded || !(Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.D))) {
			walkSource.Stop();
			runSource.Stop();
			return;
		}

		if (state == MovementState.Walking || state == MovementState.Crouching) {
			runSource.Stop();
			if(!walkSource.isPlaying) walkSource.Play();
		}

		if (state == MovementState.Sprinting) {
			walkSource.Stop();
			if(!runSource.isPlaying) runSource.Play();
		}
	}

	// Basic Movement for the player when WASD is pressed
	// Movement speed depends on Player state, crouching, sprinting, jumping, walking
	private void MoveWASD()
	{
		if (!isGrounded)
			return;

		// WASD controller
		if (Input.GetKey (KeyCode.W)) {
			switch (state)
			{
			case MovementState.Sprinting:
				this.rigidbody.velocity = this.transform.forward * sprintSpeed + new Vector3(0, this.rigidbody.velocity.y, 0);
				break;
			case MovementState.Crouching:
				this.rigidbody.velocity = this.transform.forward * crouchSpeed + new Vector3(0, this.rigidbody.velocity.y, 0);
				break;
			case MovementState.Walking:
				this.rigidbody.velocity = this.transform.forward * walkSpeed + new Vector3(0, this.rigidbody.velocity.y, 0);
				break;
			default:
				break;
			}
		}
		if (Input.GetKey (KeyCode.A)) {
			switch (state)
			{
			case MovementState.Sprinting:
				this.rigidbody.velocity = this.transform.right * sprintSpeed * -1.0f + new Vector3(0, this.rigidbody.velocity.y, 0);
				break;
			case MovementState.Crouching:
				this.rigidbody.velocity = this.transform.right * crouchSpeed  * -1.0f + new Vector3(0, this.rigidbody.velocity.y, 0);
				break;
			case MovementState.Walking:
				this.rigidbody.velocity = this.transform.right * walkSpeed * -1.0f + new Vector3(0, this.rigidbody.velocity.y, 0);
				break;
			default:
				break;
			}
		}
		if (Input.GetKey (KeyCode.S)) {
			switch (state)
			{
			case MovementState.Sprinting:
				this.rigidbody.velocity = this.transform.forward * sprintSpeed * -1.0f + new Vector3(0, this.rigidbody.velocity.y, 0);
				break;
			case MovementState.Crouching:
				this.rigidbody.velocity = this.transform.forward * crouchSpeed * -1.0f + new Vector3(0, this.rigidbody.velocity.y, 0);
				break;
			case MovementState.Walking:
				this.rigidbody.velocity = this.transform.forward * walkSpeed * -1.0f + new Vector3(0, this.rigidbody.velocity.y, 0);
				break;
			default:
				break;
			}
		}
		if (Input.GetKey (KeyCode.D)) {
			switch (state)
			{
			case MovementState.Sprinting:
				this.rigidbody.velocity = this.transform.right * sprintSpeed + new Vector3(0, this.rigidbody.velocity.y, 0);
				break;
			case MovementState.Crouching:
				this.rigidbody.velocity = this.transform.right * crouchSpeed + new Vector3(0, this.rigidbody.velocity.y, 0);
				break;
			case MovementState.Walking:
				this.rigidbody.velocity = this.transform.right * walkSpeed + new Vector3(0, this.rigidbody.velocity.y, 0);
				break;
			default:
				break;
			}
		}
	}

	private void SetMovementState()
	{
		// Shift
		if (Input.GetKey (KeyCode.LeftShift) && !Input.GetKey (KeyCode.LeftControl) && !staminaScript.getRecoverStatus()) {
			state = MovementState.Sprinting;
		}
		
		// Ctrl
		if (Input.GetKey (KeyCode.LeftControl) && !Input.GetKey (KeyCode.LeftShift)) {
			state = MovementState.Crouching;
		}
		
		// Shift and Ctrl
		if(Input.GetKey (KeyCode.LeftControl) && Input.GetKey (KeyCode.LeftShift)) {
			state = MovementState.Crouching;
		}
		
		// Not Shift or Ctrl
		if (!Input.GetKey (KeyCode.LeftControl) && !Input.GetKey (KeyCode.LeftShift)) {
			state = MovementState.Walking;
		}
	}
	
	// Set Jump Ready as 0.5f second cooldown
	IEnumerator ResetJump()
	{
		yield return new WaitForSeconds (0.5f);
		isJumpReady = true;
	}

	// Use stamina when sprinting by calling function in stamina script
	IEnumerator SprintStamina()
	{
		while (true) {
			yield return new WaitForSeconds(0.1f);
			if(state == MovementState.Sprinting)
			{
				staminaScript.UseStamina(2.0f);
			}
		}
	}
}
