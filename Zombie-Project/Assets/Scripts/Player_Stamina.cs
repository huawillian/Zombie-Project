using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Player_Stamina : NetworkBehaviour 
{
	// Idle, regenerate stamina based off hunger
	// Lose stamina based off action (only if in Idle state prior), then wait a bit before returning to Idle state
	// Recover state initiated when stamina first becomes 0, then slowly regenerates back to full, then returns to Idle state
	private enum StaminaState {Idle, Use, Recover};
	[SyncVar]
	private StaminaState state;
	private bool transition;

	[SerializeField, SyncVar]
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

		if (!isLocalPlayer)
			return;

		StartCoroutine ("IdleRecover");
	}

	IEnumerator IdleRecover()
	{
		if (!isLocalPlayer)
			yield break;

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
		if (!isLocalPlayer)
			yield break;

		state = StaminaState.Recover;

		if (isClient)
			CmdSyncState (state);

		yield return new WaitForSeconds (1.0f);

		while (Stamina < 100)
		{
			Stamina += 0.25f;
			yield return new WaitForSeconds(0.01f);
		}

		state = StaminaState.Idle;

		if (isClient)
			CmdSyncState (state);

		yield return new WaitForSeconds (0.01f);
	}

	[Command]
	void CmdSyncState(StaminaState st)
	{
		state = st;
	}


	public void UseStamina(float amount)
	{
		if (!isLocalPlayer)
			return;

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
		if (!isLocalPlayer)
			return;

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
