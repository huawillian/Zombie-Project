using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player_Health : MonoBehaviour
{
	public GameObject healthUI;
	public Player_Hunger hungerScript;
	public AudioClip deathSound;
	public AudioClip gruntSound;

	[SerializeField]
	private int health = 100;
	
	public int Health{
		get{
			return health;
		}
		set {
			if(value > 100)
			{
				health = 100;
			}
			else if(value <= 0)
			{
				if(health != 0)
				{
					health = 0;

					camera1.transform.SetParent(head1.transform);

					this.gameObject.GetComponent<Player_Death>().setDeath();

					this.gameObject.GetComponent<Player_BasicAttacks>().enabled = false;
					this.gameObject.GetComponent<Player_BasicMovement>().enabled = false;
					this.gameObject.GetComponent<Player_BasicRotation>().enabled = false;
					this.gameObject.GetComponent<Player_Camera_BasicRotation>().enabled = false;

					this.gameObject.transform.localEulerAngles = new Vector3(0,0,-90);
					this.gameObject.GetComponent<Rigidbody>().isKinematic = true;

					this.gameObject.GetComponent<Player_Timer>().StopTimer();

					this.gameObject.GetComponent<Player_Stamina>().enabled = false;
					this.gameObject.GetComponent<Player_ActionUI>().enabled = false;
					this.gameObject.GetComponent<Player_Search>().enabled = false;
					this.gameObject.GetComponent<Inventory_PickUp>().enabled = false;
					this.gameObject.GetComponent<Player_Hunger>().enabled = false;
					this.gameObject.GetComponent<Player_Inventory>().enabled = false;

					this.gameObject.GetComponentInChildren<Person_AnimationController>().SetDeath();
					AudioSource.PlayClipAtPoint (deathSound, this.gameObject.transform.position);
				}
			}
			else 
			{
				health = value;
			}

			healthUI.GetComponent<Slider>().value = health;
		}
	}

	public GameObject head1;
	public GameObject camera1;

	void Start()
	{
		hungerScript = this.GetComponent<Player_Hunger> ();
		StartCoroutine ("StartHealthRegen");
	}

	IEnumerator StartHealthRegen()
	{
		while (Health > 0)
		{
			if(hungerScript.Hunger > 50)
			{
				Health += 5;
			}
			else
			if(hungerScript.Hunger > 25)
			{
				Health += 2;
			}
			else
			{
				Health += 1;
			}

			yield return new WaitForSeconds(10.0f);
		}
	}

	void Update()
	{
	}

	public void damagePlayer(int damage)
	{
		Health -= damage;
		if(Health > 0)
			AudioSource.PlayClipAtPoint (gruntSound, this.gameObject.transform.position);
	}

	public void healPlayer(int amount)
	{
		Health += amount;
	}
}
