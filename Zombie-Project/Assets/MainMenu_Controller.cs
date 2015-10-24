using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;

public class MainMenu_Controller : MonoBehaviour
{
	private enum MainMenuState{Main, LAN, LAN_Host, LAN_Join, Matchmaking, Matchmaking_Create, Matchmaking_Find, Load};
	private MainMenuState state = MainMenuState.Main;

	public GameObject NetworkManagerObject;
	private NetworkManager networkmanager;

	public GameObject Menu0;
	public GameObject Menu1;
	public GameObject Menu2;
	public GameObject Menu3;
	public GameObject Menu4;
	public GameObject Menu5;
	public GameObject Menu6;
	public GameObject Menu7;

	public GameObject menu2IpText;
	public GameObject menu3IpInputField;
	public GameObject menu5IpInputField;
	public GameObject menu7Slider;
	public GameObject menu6Content;
	public GameObject menu6TemplateButton;
	public List<GameObject> matchButtons = new List<GameObject>();

	private string ip;

	// Use this for initialization
	void Start ()
	{
		Menu0.SetActive (true);

		networkmanager = NetworkManagerObject.GetComponent<NetworkManager> ();
		menu2IpText.GetComponent<Text>().text = "Your IP Address is: \n" + Network.player.ipAddress;

		StartCoroutine ("CameraAnimate");
		Cursor.visible = true;
	}

	IEnumerator CameraAnimate()
	{
		GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Animation> ().Play ();

		while (true) {
			yield return new WaitForSeconds(GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Animation> ().clip.length );
			GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Animation> ().Play ();
		}
	}

	public void OnButtonLAN()
	{
		state = MainMenuState.LAN;
	}

	public void OnButtonLANHost()
	{
		state = MainMenuState.LAN_Host;
	}

	public void OnButtonLANJoin()
	{
		state = MainMenuState.LAN_Join;
	}

	public void OnButtonMatch()
	{
		state = MainMenuState.Matchmaking;
		this.networkmanager.StartMatchMaker ();
		this.networkmanager.matchMaker.ListMatches (0,20,"",this.networkmanager.OnMatchList);
	}

	public void OnButtonMatchCreate()
	{
		state = MainMenuState.Matchmaking_Create;
	}

	public void OnButtonMatchFind()
	{
		if (matchButtons.Count > 0) {
			foreach(GameObject obj in matchButtons)
				Destroy(obj);

			matchButtons.Clear();
		}

		state = MainMenuState.Matchmaking_Find;
		menu6TemplateButton.SetActive (true);
		float yPosOffset = menu6TemplateButton.GetComponent<RectTransform> ().localPosition.y;

		if (this.networkmanager.matches.Count > 0) {
			foreach (UnityEngine.Networking.Match.MatchDesc match in this.networkmanager.matches) {
				GameObject button = GameObject.Instantiate (menu6TemplateButton) as GameObject;

				button.GetComponentsInChildren<Text> () [0].text = match.name.ToString();
				button.GetComponentsInChildren<Text> () [1].text = match.networkId.ToString();
				button.GetComponentsInChildren<Text> () [2].text = match.currentSize.ToString();

				button.GetComponent<RectTransform> ().SetParent (menu6Content.GetComponent<RectTransform> ());
				matchButtons.Add (button);
			}
		}
		foreach (GameObject button in matchButtons)
		{
			button.GetComponent<RectTransform>().localPosition = new Vector3(0,yPosOffset,0);
			yPosOffset -= 50;
		}

		menu6TemplateButton.SetActive (false);
	}

	public void OnButtonLoad()
	{
		state = MainMenuState.Load;
		StartCoroutine ("AnimateSlider");
	}

	public void OnButtonReturnMainMenu()
	{
		switch (state) {
		case MainMenuState.LAN:
			state = MainMenuState.Main;
			break;
		case MainMenuState.LAN_Host:
			state = MainMenuState.LAN;
			break;
		case MainMenuState.LAN_Join:
			state = MainMenuState.LAN;
			break;
		case MainMenuState.Matchmaking:
			state = MainMenuState.Main;
			break;
		case MainMenuState.Matchmaking_Create:
			state = MainMenuState.Matchmaking;
			break;
		case MainMenuState.Matchmaking_Find:
			state = MainMenuState.Matchmaking;
			break;
		default:
			break;
		}
	}

	public void OnButtonCreateGameLAN()
	{
		this.networkmanager.StartHost ();
		OnButtonLoad ();
	}

	public void OnButtonJoinGameLAN()
	{
		this.networkmanager.StartClient ();
		this.networkmanager.networkAddress = menu3IpInputField.GetComponent<Text> ().text;
		OnButtonLoad ();
	}

	public void OnButtonCreateGameMatch()
	{
		this.networkmanager.matchMaker.CreateMatch(menu5IpInputField.GetComponent<Text>().text, this.networkmanager.matchSize, true, "", this.networkmanager.OnMatchCreate);
		OnButtonLoad ();
	}

	public void OnButtonJoinGameMatch(GameObject but)
	{
		this.networkmanager.matchName = but.GetComponentsInChildren<Text> () [0].text;
		this.networkmanager.matchSize = uint.Parse(but.GetComponentsInChildren<Text> () [2].text);

		UnityEngine.Networking.Types.NetworkID id = UnityEngine.Networking.Types.NetworkID.Invalid;

		foreach(UnityEngine.Networking.Match.MatchDesc match in this.networkmanager.matches)
		{
			if(but.GetComponentsInChildren<Text> () [1].text == match.networkId.ToString())
			{
				id = match.networkId;
			}
		}

		this.networkmanager.matchMaker.JoinMatch (id, "", this.networkmanager.OnMatchJoined);

		OnButtonLoad ();
	}

	private IEnumerator AnimateSlider()
	{
		Slider sl = menu7Slider.GetComponent<Slider> ();

		while (sl.value < 100) {
			sl.value += Random.Range(5,15);
			yield return new WaitForSeconds(0.1f);
		}

		yield return new WaitForSeconds(10.0f);
		Application.LoadLevel (Application.loadedLevelName);

	}

	void Update()
	{
		switch (state) {
		case MainMenuState.Main:
			Menu0.SetActive(true);
			Menu1.SetActive(false);
			Menu2.SetActive(false);
			Menu3.SetActive(false);
			Menu4.SetActive(false);
			Menu5.SetActive(false);
			Menu6.SetActive(false);
			Menu7.SetActive(false);
			break;
		case MainMenuState.LAN:
			Menu0.SetActive(false);
			Menu1.SetActive(true);
			Menu2.SetActive(false);
			Menu3.SetActive(false);
			Menu4.SetActive(false);
			Menu5.SetActive(false);
			Menu6.SetActive(false);
			Menu7.SetActive(false);
			break;
		case MainMenuState.LAN_Host:
			Menu0.SetActive(false);
			Menu1.SetActive(false);
			Menu2.SetActive(true);
			Menu3.SetActive(false);
			Menu4.SetActive(false);
			Menu5.SetActive(false);
			Menu6.SetActive(false);
			Menu7.SetActive(false);
			break;
		case MainMenuState.LAN_Join:
			Menu0.SetActive(false);
			Menu1.SetActive(false);
			Menu2.SetActive(false);
			Menu3.SetActive(true);
			Menu4.SetActive(false);
			Menu5.SetActive(false);
			Menu6.SetActive(false);
			Menu7.SetActive(false);
			break;
		case MainMenuState.Matchmaking:
			Menu0.SetActive(false);
			Menu1.SetActive(false);
			Menu2.SetActive(false);
			Menu3.SetActive(false);
			Menu4.SetActive(true);
			Menu5.SetActive(false);
			Menu6.SetActive(false);
			Menu7.SetActive(false);
			break;
		case MainMenuState.Matchmaking_Create:
			Menu0.SetActive(false);
			Menu1.SetActive(false);
			Menu2.SetActive(false);
			Menu3.SetActive(false);
			Menu4.SetActive(false);
			Menu5.SetActive(true);
			Menu6.SetActive(false);
			Menu7.SetActive(false);
			break;
		case MainMenuState.Matchmaking_Find:
			Menu0.SetActive(false);
			Menu1.SetActive(false);
			Menu2.SetActive(false);
			Menu3.SetActive(false);
			Menu4.SetActive(false);
			Menu5.SetActive(false);
			Menu6.SetActive(true);
			Menu7.SetActive(false);
			break;
		case MainMenuState.Load:
			Menu0.SetActive(false);
			Menu1.SetActive(false);
			Menu2.SetActive(false);
			Menu3.SetActive(false);
			Menu4.SetActive(false);
			Menu5.SetActive(false);
			Menu6.SetActive(false);
			Menu7.SetActive(true);
			break;
		default:
			break;
		}
	}




}
