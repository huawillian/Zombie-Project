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
		if (!isLocalPlayer)
			return;

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
				actionUI.GetComponent<Text>().text = "Currently Searching... (" + (timeDown + 3 - Time.time) + ")";
			}

			if(Input.GetKeyDown(KeyCode.E))
			{
				if(closestSearch != null)
				{
					isCurrentlySearching = true;
					timeDown = Time.time;

					this.GetComponent<Player_BasicAttacks>().enabled = false;
					this.GetComponent<Player_BasicMovement>().enabled = false;
					this.GetComponent<Player_BasicRotation>().enabled = false;
					this.GetComponent<Player_Camera_BasicRotation>().enabled = false;
				}
			}

			if((Input.GetKeyUp(KeyCode.E) && isCurrentlySearching) || (isCurrentlySearching && Input.GetKey(KeyCode.E) && Time.time > timeDown + 3))
			{
				if(closestSearch != null)
				{
					if(Time.time > timeDown + 3)
					{
						closestSearch.GetComponent<Search_Content>().setSearched();
						instantiateItems( closestSearch.GetComponent<Search_Content>().getContent(), closestSearch.transform.position + closestSearch.transform.forward);

						if(closestSearch.name == "Corpse")
						{
							searchInRange.Remove(closestSearch);
							Destroy(closestSearch);
						}

						closestSearch = null;
						pickupScript.enableScript();
						isCurrentlySearching = false;

						this.GetComponent<Player_BasicAttacks>().enabled = true;
						this.GetComponent<Player_BasicMovement>().enabled = true;
						this.GetComponent<Player_BasicRotation>().enabled = true;
						this.GetComponent<Player_Camera_BasicRotation>().enabled = true;
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
			pickupScript.enableScript();
			closestSearch = null;
		}

	}

	void instantiateItems (LinkedList<string> listOfItems, Vector3 pos)
	{
		if (!isLocalPlayer)
			return;

		foreach (string item in listOfItems)
		{
			GameObject tempObj = null;

			switch(item)
			{
				case "Baseball Bat":
					tempObj = Instantiate(baseballBatPrefab, pos, Quaternion.identity) as GameObject;
					tempObj.name = baseballBatPrefab.name;
					break;
				case "Pistol":
					tempObj = Instantiate(pistolPrefab, pos, Quaternion.identity) as GameObject;
					tempObj.name = pistolPrefab.name;
					break;
				case "Ammo":
					tempObj = Instantiate(ammoPrefab, pos, Quaternion.identity) as GameObject;
					tempObj.name = ammoPrefab.name;
					break;
				case "Food":
					tempObj = Instantiate(foodPrefab, pos, Quaternion.identity) as GameObject;
					tempObj.name = foodPrefab.name;
					break;
				case "Medkit":
					tempObj = Instantiate(medkitPrefab, pos, Quaternion.identity) as GameObject;
					tempObj.name = medkitPrefab.name;
					break;
				case "Wood":
					tempObj = Instantiate(woodPrefab, pos, Quaternion.identity) as GameObject;
					tempObj.name = woodPrefab.name;
					break;
				default:
					break;
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (!isLocalPlayer)
			return;

		if (other.tag == "Search") {
			
			if(!searchInRange.Contains(other.gameObject))
				searchInRange.AddFirst(other.gameObject);
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (!isLocalPlayer)
			return;

		if (other.tag == "Search") {
			searchInRange.Remove(other.gameObject);
		}
	}
}
