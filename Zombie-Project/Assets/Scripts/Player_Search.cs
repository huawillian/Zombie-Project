using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Player_Search : NetworkBehaviour
{
	public Inventory_PickUp pickupScript;
	public GameObject actionUI;

	public GameObject baseballBatPrefab;
	public GameObject pistolPrefab;
	public GameObject ammoPrefab;
	public GameObject foodPrefab;
	public GameObject woodPrefab;
	public GameObject medkitPrefab;

	public LinkedList<GameObject> searchInRange;
	GameObject closestSearch;

	private float timeDown;
	private bool isCurrentlySearching;

	// Use this for initialization
	void Start ()
	{
		pickupScript = this.gameObject.GetComponent<Inventory_PickUp> ();
		searchInRange = new LinkedList<GameObject> ();
		closestSearch = null;
		timeDown = Time.time;
		isCurrentlySearching = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!isLocalPlayer)
			return;

		if (searchInRange.Count > 0)
		{
			float range = 999f;
			closestSearch = null;
			
			foreach (GameObject obj in searchInRange)
			{
				if(obj.GetComponent<Search_Content>().isSearched) 
					continue;

				if(obj != null)
					if(Vector3.Distance(obj.transform.position, this.gameObject.transform.position) < range)
				{
					range = Vector3.Distance(obj.transform.position, this.gameObject.transform.position);
					closestSearch = obj;
				}
			}

			if(closestSearch != null)
				pickupScript.disableScript();

			if(closestSearch != null && !isCurrentlySearching)
			{
				actionUI.GetComponent<Text>().text = "Press 'E' to Search " + closestSearch.name;
			}
			else 
			if(closestSearch != null && isCurrentlySearching)
			{
				actionUI.GetComponent<Text>().text = "Currently Searching... (" + (timeDown + 2 - Time.time) + ")";
			}

			if(Input.GetKeyDown(KeyCode.E))
			{
				if(closestSearch != null)
				{
					isCurrentlySearching = true;
					timeDown = Time.time;
				}
			}



			if((Input.GetKeyUp(KeyCode.E) && isCurrentlySearching) || (isCurrentlySearching && Input.GetKey(KeyCode.E) && Time.time > timeDown + 2))
			{
				if(closestSearch != null)
				{
					if(Time.time > timeDown + 2)
					{
						closestSearch.GetComponent<Search_Content>().setSearched();
						instantiateItems(closestSearch.GetComponent<Search_Content>().getContent(), closestSearch.transform.position + closestSearch.transform.forward);
						searchInRange.Remove(closestSearch);

						
						if(isServer)
						{
							RpcDestroys(closestSearch.gameObject);
							Destroy(closestSearch.gameObject);
						}
						else
						{
							CmdDestroys(closestSearch.gameObject);
						}
				}
					else
					{
						pickupScript.enableScript();
						isCurrentlySearching = false;
						
						this.GetComponent<Player_BasicAttacks>().enabled = true;
						this.GetComponent<Player_BasicMovement>().enabled = true;
						this.GetComponent<Player_BasicRotation>().enabled = true;
						this.GetComponent<Player_Camera_BasicRotation>().enabled = true;
					}
				}
			}
		} 
		else
		{
			isCurrentlySearching = false;
			pickupScript.enableScript();
			closestSearch = null;
		}

		if(Input.GetKeyDown(KeyCode.E))
		{
			this.GetComponent<Player_BasicAttacks>().enabled = false;
			this.GetComponent<Player_BasicMovement>().enabled = false;
			this.GetComponent<Player_BasicRotation>().enabled = false;
			this.GetComponent<Player_Camera_BasicRotation>().enabled = false;
		}
		
		if(Input.GetKeyUp(KeyCode.E))
		{
			this.GetComponent<Player_BasicAttacks>().enabled = true;
			this.GetComponent<Player_BasicMovement>().enabled = true;
			this.GetComponent<Player_BasicRotation>().enabled = true;
			this.GetComponent<Player_Camera_BasicRotation>().enabled = true;
			
		}
	}

	[ClientRpc]
	void RpcDestroys(GameObject obj)
	{
		Destroy(obj);
	}
	
	[Command]
	void CmdDestroys(GameObject obj)
	{
		RpcDestroys (obj);
	}
	
	[Command]
	void CmdSpawns(GameObject obj)
	{
		NetworkServer.Spawn (obj);
	}

	void instantiateItems (LinkedList<string> listOfItems, Vector3 pos)
	{
		foreach (string item in listOfItems)
		{
			GameObject tempObj = null;

			switch(item)
			{
				case "Baseball Bat":
				if(isServer)
				{
					tempObj = Instantiate(baseballBatPrefab, pos, Quaternion.identity) as GameObject;
					tempObj.name = baseballBatPrefab.name;
					NetworkServer.Spawn(tempObj);
				}
				else
				{
					CmdSpawn("Baseball Bat", pos);
				}
					break;
				case "Pistol":
				if(isServer)
				{
					tempObj = Instantiate(pistolPrefab, pos, Quaternion.identity) as GameObject;
					tempObj.name = pistolPrefab.name;
					NetworkServer.Spawn(tempObj);
				}
				else
				{
					CmdSpawn("Pistol", pos);
				}
					break;
				case "Ammo":
				if(isServer)
				{
					tempObj = Instantiate(ammoPrefab, pos, Quaternion.identity) as GameObject;
					tempObj.name = ammoPrefab.name;
					NetworkServer.Spawn(tempObj);
				}
				else
				{
					CmdSpawn("Ammo", pos);
				}
					break;
				case "Food":
				if(isServer)
				{
					tempObj = Instantiate(foodPrefab, pos, Quaternion.identity) as GameObject;
					tempObj.name = foodPrefab.name;
					NetworkServer.Spawn(tempObj);
				}
				else
				{
					CmdSpawn("Food", pos);
				}
					break;
				case "Medkit":
				if(isServer)
				{
					tempObj = Instantiate(medkitPrefab, pos, Quaternion.identity) as GameObject;
					tempObj.name = medkitPrefab.name;
					NetworkServer.Spawn(tempObj);
				}
				else
				{
					CmdSpawn("Medkit", pos);
				}
					break;
				case "Wood":
				if(isServer)
				{
					tempObj = Instantiate(woodPrefab, pos, Quaternion.identity) as GameObject;
					tempObj.name = woodPrefab.name;
					NetworkServer.Spawn(tempObj);
				}
				else
				{
					CmdSpawn("Wood", pos);
				}
					break;
				default:
					break;
			}
		}
	}

	[Command]
	void CmdSpawn(string nm, Vector3 pos)
	{
		GameObject temp = new GameObject ();
		
		if (nm == "Ammo")
			temp = (Instantiate(ammoPrefab, pos, Quaternion.identity) as GameObject);
		if (nm == "Food")
			temp = (Instantiate(foodPrefab, pos, Quaternion.identity) as GameObject);
		if (nm == "Baseball Bat")
			temp = (Instantiate(baseballBatPrefab, pos, Quaternion.identity) as GameObject);
		if (nm == "Medkit")
			temp = (Instantiate(medkitPrefab, pos, Quaternion.identity) as GameObject);
		if (nm == "Pistol")
			temp = (Instantiate(pistolPrefab, pos, Quaternion.identity) as GameObject);
		if (nm == "Wood")
			temp = (Instantiate(woodPrefab, pos, Quaternion.identity) as GameObject);
		
		temp.name = nm;
		NetworkServer.Spawn (temp);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Search") {
			
			if(!searchInRange.Contains(other.gameObject))
				searchInRange.AddFirst(other.gameObject);
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Search") {
			searchInRange.Remove(other.gameObject);
		}
	}
}
