using UnityEngine;
using System.Collections;

public class Player_Health : MonoBehaviour
{

	[SerializeField]
	private int health = 100;
	
	public int Health{
		get{
			return health;
		}
		set {
			if(value > 100) health = 100;
			else if(value <= 0)
			{
				health= 0;
				this.gameObject.GetComponent<Player_Death>().isDead = true;

				this.gameObject.GetComponent<Player_BasicAttacks>().enabled = false;
				this.gameObject.GetComponent<Player_BasicMovement>().enabled = false;
				this.gameObject.GetComponent<Player_BasicRotation>().enabled = false;
				this.gameObject.GetComponent<Player_Camera_BasicRotation>().enabled = false;

			}
			else 
			{
				health = value;
			}
		}
	}
	
	public void damagePlayer(int damage)
	{
		Health -= damage;
	}
}
