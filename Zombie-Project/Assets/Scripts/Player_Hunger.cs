using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player_Hunger : MonoBehaviour
{
	public GameObject hungerUI;

	private int hunger;

	public int Hunger
	{
		get {
			return hunger;
		}
		
		set {
			if(value < 0) hunger = 0;
			else if (value > 100) hunger = 100;
			else hunger = value;

			hungerUI.GetComponent<Slider>().value = hunger;
		}
	}

	// Use this for initialization
	void Start ()
	{
		hunger = 100;
		StartCoroutine ("HungerStart");
	}

	IEnumerator HungerStart()
	{
		while (true) {
			Hunger--;
			yield return new WaitForSeconds(10.0f);
		}
	}

	public void AddHunger(int amount)
	{
		Hunger += amount;
	}

	// Update is called once per frame
	void Update ()
	{
	}
}
