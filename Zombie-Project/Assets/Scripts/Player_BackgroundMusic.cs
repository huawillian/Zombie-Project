using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_BackgroundMusic : NetworkBehaviour {

	public AudioClip otherClip;
	
	IEnumerator Start()
	{
		if (!isLocalPlayer)
		yield break;

		AudioSource audio = this.gameObject.AddComponent<AudioSource>();
		audio.clip = otherClip;

		while (true) {
			audio.Play();
			yield return new WaitForSeconds(audio.clip.length);
		}

	}
}
