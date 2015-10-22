using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MainMenu_Controller : MonoBehaviour
{
	public GameObject NetworkManagerObject;
	private NetworkManager networkmanager;

	public GameObject Menu1;
	public GameObject Menu2;
	public GameObject Menu3;

	public GameObject menu2IpText;
	public GameObject menu3IpInputField;

	private string ip;

	// Use this for initialization
	void Start ()
	{
		Menu1.SetActive (true);
		Menu2.SetActive (false);
		Menu3.SetActive (false);

		networkmanager = NetworkManagerObject.GetComponent<NetworkManager> ();

		menu2IpText.GetComponent<Text>().text = "Your IP Address is: \n" + Network.player.ipAddress;

		StartCoroutine ("CameraAnimate");
	}

	IEnumerator CameraAnimate()
	{
		GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Animation> ().Play ();

		while (true) {
			yield return new WaitForSeconds(GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Animation> ().clip.length );
			GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Animation> ().Play ();
		}
	}

	public void OnButtonMenu1HostGame()
	{
		Menu1.SetActive (false);
		Menu2.SetActive (true);
		Menu3.SetActive (false);
	}

	public void OnButtonMenu1JoinGame()
	{
		Menu1.SetActive (false);
		Menu2.SetActive (false);
		Menu3.SetActive (true);
	}

	public void OnButtonReturnMainMenu()
	{
		Menu1.SetActive (true);
		Menu2.SetActive (false);
		Menu3.SetActive (false);
	}

	public void OnButtonMenu2CreateGame()
	{
		this.networkmanager.StartHost ();
	}

	public void OnButtonMenu3JoinGame()
	{
		this.networkmanager.StartClient ();
		this.networkmanager.networkAddress = menu3IpInputField.GetComponent<Text> ().text;
	}
}
