using UnityEngine;
using System.Collections;

public class Zombie_BasicMovement : MonoBehaviour
{
	GameObject zombie;
	GameObject player;
	Rigidbody zombieRigidbody;
	bool patrol;
	bool attack;

	// Use this for initialization
	void Start ()
	{
		if (!zombie)
			zombie = this.gameObject;

		if (!zombieRigidbody)
			zombieRigidbody = zombie.GetComponent<Rigidbody> ();

		patrol = true;

		StartCoroutine ("StartPatrol");
	}

	IEnumerator StartPatrol()
	{
		while (patrol)
		{
			int move = Random.Range(0, 3);

			switch(move)
			{
				case 0:
					yield return StartCoroutine("MoveForward");
					break;
				case 1:
					yield return StartCoroutine("TurnRight");
					break;
				case 2:
					yield return StartCoroutine("TurnLeft");
					break;
				case 3:
					yield return StartCoroutine("StayStill");
					break;
				default:
					Debug.Log(this.gameObject.ToString() + " has invalid move.");
					break;
			}
		}
	}

	IEnumerator StartAttack()
	{
		while (attack)
		{
			zombieRigidbody.velocity = zombie.transform.forward;
			zombie.transform.LookAt(player.transform);
			zombie.transform.localEulerAngles = new Vector3(0, zombie.transform.localEulerAngles.y ,0);

			yield return new WaitForSeconds(0.01f);
		}
	}

	IEnumerator MoveForward()
	{
		float startTime = Time.time;
		float endTime = startTime + 5.0f;
		
		while (Time.time < endTime)
		{
			zombieRigidbody.velocity = zombie.transform.forward;
			yield return new WaitForSeconds (0.01f);
		}

		zombieRigidbody.velocity = Vector3.zero;
	}

	IEnumerator TurnRight()
	{
		Vector3 startRotation = zombie.transform.localEulerAngles;
		Vector3 endRotation = startRotation + new Vector3 (0, 90, 0);
		float startTime = Time.time;
		float endTime = startTime + 2.0f;

		while (Time.time < endTime)
		{
			zombie.transform.localEulerAngles = Vector3.Lerp(startRotation, endRotation, (Time.time - startTime)/2.0f);
			yield return new WaitForSeconds (0.01f);
		}
	}

	IEnumerator TurnLeft()
	{
		Vector3 startRotation = zombie.transform.localEulerAngles;
		Vector3 endRotation = startRotation + new Vector3 (0, -90, 0);
		float startTime = Time.time;
		float endTime = startTime + 2.0f;
		
		while (Time.time < endTime)
		{
			zombie.transform.localEulerAngles = Vector3.Lerp(startRotation, endRotation, (Time.time - startTime)/2.0f);
			yield return new WaitForSeconds (0.01f);

		}
	}

	IEnumerator StayStill()
	{
		yield return new WaitForSeconds (2.0f);
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.name.Equals ("Player")) {
			patrol = false;
			attack = true;
			player = collider.gameObject;
			StartCoroutine("StartAttack");
		}
	}



	// Update is called once per frame
	void Update ()
	{
		
	}
}
