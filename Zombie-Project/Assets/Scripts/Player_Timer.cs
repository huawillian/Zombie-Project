using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Player_Timer : NetworkBehaviour
{
	private float timeAlive;
	private bool timerActive;
	private float timeStart;

	public GameObject timerUI;

	// Use this for initialization
	void Start ()
	{
		if (!isLocalPlayer)
			return;

		StartTimer ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!isLocalPlayer)
			return;

		if (timerActive)
		{
			timeAlive = Time.time - timeStart;
		}

		int minutes = Mathf.FloorToInt(timeAlive / 60);
		int seconds = Mathf.FloorToInt(timeAlive - minutes * 60);
		string r = (minutes < 10) ? "0" + minutes.ToString() : minutes.ToString();
		r += ":";
		r += (seconds < 10) ? "0" + seconds.ToString() : seconds.ToString();



		timerUI.GetComponent<Text> ().text = r;
	}

	public void StartTimer()
	{
		if (!isLocalPlayer)
			return;

		timerActive = true;
		timeStart = Time.time;
	}

	public void StopTimer()
	{
		if (!isLocalPlayer)
			return;

		timerActive = false;
	}

	public float getTimeAlive()
	{
		return timeAlive;
	}

}
