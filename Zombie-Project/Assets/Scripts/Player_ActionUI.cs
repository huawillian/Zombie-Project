using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player_ActionUI : MonoBehaviour
{
	public Player_Search searchScript;
	public Inventory_PickUp pickupScript;
	public GameObject ActionUI;
	
	// Use this for initialization
	void Start ()
	{
		searchScript = this.gameObject.GetComponent <Player_Search>();
		pickupScript = this.gameObject.GetComponent<Inventory_PickUp> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (pickupScript.itemsInRange.Count > 0)
		{
			ActionUI.SetActive(true);
			return;
		}

		foreach (GameObject obj in searchScript.searchInRange)
		{
			if(!obj.GetComponent<Search_Content>().isSearched)
			{
				ActionUI.SetActive(true);
				return;
			}
		}
	
		ActionUI.SetActive (false);

	}
}
