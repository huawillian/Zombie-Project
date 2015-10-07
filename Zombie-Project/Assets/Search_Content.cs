using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Search_Content : MonoBehaviour
{
	LinkedList<string> itemsInSearch;
	public bool isSearched;

	public Dictionary<int, string> itemIndex;

	// Use this for initialization
	void Start ()
	{
		itemsInSearch = new LinkedList<string> ();
		itemIndex = new Dictionary<int, string> ();
		isSearched = false;

		itemIndex.Add (0, "Pistol");
		itemIndex.Add (1, "Baseball Bat");
		itemIndex.Add (2, "Food");
		itemIndex.Add (3, "Ammo");
		itemIndex.Add (4, "Ammo");
		itemIndex.Add (5, "Medkit");
		itemIndex.Add (6, "Medkit");
		itemIndex.Add (7, "Wood");
		itemIndex.Add (8, "Ammo");
		itemIndex.Add (9, "Medkit");

		if (this.gameObject.name == "Fridge") 
		{
			int amountFood = Random.Range(1,5);

			for(int i=0; i<amountFood; i++)
			{
				itemsInSearch.AddFirst("Food");
			}
		}
		else
		if (this.gameObject.name == "Drawers") 
		{
			int amountItems = Random.Range(1,3);
			
			for(int i=0; i<amountItems; i++)
			{
				itemsInSearch.AddFirst(itemIndex[Random.Range(0,9)]);
			}
		}
		else
		if (this.gameObject.name == "Corpse") 
		{
			int amountItems = Random.Range(0,1);
			
			for(int i=0; i<amountItems; i++)
			{
				itemsInSearch.AddFirst(itemIndex[Random.Range(0,9)]);
			}
		}
	}

	public void setSearched()
	{
		isSearched = true;
	}

	public LinkedList<string> getContent()
	{
		return itemsInSearch;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
