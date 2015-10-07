using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player_ActionUI : MonoBehaviour
{
	Player_Search searchScript;
	Inventory_PickUp pickupScript;

	public GameObject ActionUI;

	public int countActive;

	// Use this for initialization
	void Start ()
	{
		searchScript = this.gameObject.GetComponent <Player_Search>();
		pickupScript = this.gameObject.GetComponent<Inventory_PickUp> ();
		countActive = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		countActive = 0;

		countActive += pickupScript.itemsInRange.Count;

		foreach (GameObject obj in searchScript.searchInRange) {
			if(!obj.GetComponent<Search_Content>().isSearched) countActive ++;
		}


		if (countActive == 0)
			ActionUI.SetActive (false);
		else
			ActionUI.SetActive (true);
	}
}
