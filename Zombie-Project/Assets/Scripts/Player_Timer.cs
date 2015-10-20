using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player_Timer : MonoBehaviour
{
	private float timeAlive;
	private bool timerActive;
	private float timeStart;

	public GameObject timerUI;

	// Use this for initialization
	void Start ()
	{
		StartTimer ();
	}
	
	// Update is called once per frame
	void Update ()
	{
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
		timerActive = true;
		timeStart = Time.time;
	}

	public void StopTimer()
	{
		timerActive = false;
	}

	public float getTimeAlive()
	{
		return timeAlive;
	}

}
