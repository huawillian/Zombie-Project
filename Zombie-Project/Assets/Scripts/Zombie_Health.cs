using UnityEngine;
using System.Collections;

public class Zombie_Health : MonoBehaviour
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
				Destroy(this.gameObject);
			}
			else 
			{
				health = value;
			}
		}
	}

	public void damageZombie(int damage)
	{
		Health -= damage;
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
