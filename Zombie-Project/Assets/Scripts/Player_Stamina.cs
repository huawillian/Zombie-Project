using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player_Stamina : MonoBehaviour 
{
	// Idle, regenerate stamina based off hunger
	// Lose stamina based off action (only if in Idle state prior), then wait a bit before returning to Idle state
	// Recover state initiated when stamina first becomes 0, then slowly regenerates back to full, then returns to Idle state
	private enum StaminaState {Idle, Use, Recover};
	private StaminaState state;
	private bool transition;

	private float stamina;
	public float Stamina
	{
		get
		{
			return stamina;
		}
		set
		{
			if(value > 100) stamina = 100;
			else if(value <= 0)
			{
				stamina = 0;
				StartCoroutine("StartRecover");
			}
			else stamina = value;
		}
	}

	public Player_Hunger hungerScript;
	public GameObject staminaUI;
	public GameObject staminaColorUI;

	// Use this for initialization
	void Start ()
	{
		Stamina = 100;
		hungerScript = this.gameObject.GetComponent<Player_Hunger>();
		state = StaminaState.Idle;
		transition = false;

		StartCoroutine ("IdleRecover");
	}

	IEnumerator IdleRecover()
	{
		while (true) 
		{
			yield return new WaitForSeconds(0.1f);

			if(state == StaminaState.Idle)
			{
				if(hungerScript.Hunger > 80)
				{
					Stamina +=  2f;
				}
				else
				if(hungerScript.Hunger > 60)
				{
					Stamina +=  1f;
				}
				else
				if(hungerScript.Hunger > 40)
				{
					Stamina +=  0.5f;
				}
				else
				if(hungerScript.Hunger > 25)
				{
					Stamina += 0.3f;
				}
				else
				{
					Stamina += 0.2f;
				}
			}
		}
	}

	IEnumerator StartRecover()
	{
		state = StaminaState.Recover;

		yield return new WaitForSeconds (1.0f);

		while (Stamina < 100)
		{
			Stamina += 0.25f;
			yield return new WaitForSeconds(0.01f);
		}

		state = StaminaState.Idle;

		yield return new WaitForSeconds (0.01f);
	}

	public void UseStamina(float amount)
	{
		if(state == StaminaState.Recover) return;

		state = StaminaState.Use;
		Stamina -= amount;

		StopCoroutine ("SetTransition");
		StartCoroutine ("SetTransition");
	}

	IEnumerator SetTransition()
	{
		if (transition == false)
			transition = true;

		yield return new WaitForSeconds (1.0f);
		transition = false;

		if (state == StaminaState.Use) {
			state = StaminaState.Idle;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		staminaUI.GetComponent<Slider> ().value = Stamina;

		if (state == StaminaState.Recover) {
			staminaColorUI.GetComponent<Image> ().color = Color.red;
		} else
		{
			staminaColorUI.GetComponent<Image> ().color = new Color(200,200,0);

		}
	}

	public bool getRecoverStatus()
	{
		if (state == StaminaState.Recover)
			return 	true;
		else
			return false;
	}
}
