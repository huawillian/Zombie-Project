using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player_Health : MonoBehaviour
{
	public GameObject healthUI;
	public Player_Hunger hungerScript;
	public Player_Death deathScript;

	public AudioClip gruntSound;

	[SerializeField]
	private int health = 100;
	
	public int Health {
		get {
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
					deathScript.setDeath();
				}
			}
			else 
			{
				health = value;
			}

			healthUI.GetComponent<Slider>().value = health;
		}
	}

	void Start()
	{
		hungerScript = this.GetComponent<Player_Hunger> ();
		deathScript = this.GetComponent<Player_Death> ();
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
