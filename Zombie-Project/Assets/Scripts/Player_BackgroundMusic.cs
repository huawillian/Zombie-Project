using UnityEngine;
using System.Collections;

public class Player_BackgroundMusic : MonoBehaviour {

	public AudioClip otherClip;
	
	IEnumerator Start() {
		AudioSource audio = this.gameObject.AddComponent<AudioSource>();

		audio.clip = otherClip;

		while (true) {
			audio.Play();
			yield return new WaitForSeconds(audio.clip.length);
		}

	}
}
