using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventory_PickUp : MonoBehaviour
{
	public GameObject actionUI;
	
	LinkedList<GameObject> itemsInRange;
	GameObject closestItem;

	Player_Inventory inventoryScript;

	public bool scriptActive = true;

	// Use this for initialization
	void Start () 
	{
		inventoryScript = this.gameObject.GetComponent<Player_Inventory> ();
		itemsInRange = new LinkedList<GameObject> ();
		closestItem = null;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!scriptActive)
			return;

		if (itemsInRange.Count > 0)
		{
			float range = 999f;

			closestItem = null;

			foreach (GameObject obj in itemsInRange) {

				if(obj != null)
				if(Vector3.Distance(obj.transform.position, this.gameObject.transform.position) < range)
				{
					range = Vector3.Distance(obj.transform.position, this.gameObject.transform.position);
					closestItem = obj;
				}
			}

			if(closestItem != null)
			{
				actionUI.SetActive (true);
				actionUI.GetComponent<Text>().text = "Press 'E' to Pickup " + closestItem.name;
			}

			if(Input.GetKeyDown(KeyCode.E))
			{
				if(closestItem != null)
				{
					inventoryScript.PickUpItem(closestItem.name);
					itemsInRange.Remove(closestItem);
					Destroy(closestItem.gameObject);
					closestItem = null;
				}
			}
		} 
		else
		{
			actionUI.SetActive (false);
			closestItem = null;
		}
	}

	public void disableScript ()
	{
		scriptActive = false;
	}

	public void enableScript()
	{
		scriptActive = true;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Item") {

			if(!itemsInRange.Contains(other.gameObject))
			itemsInRange.AddFirst(other.gameObject);
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Item") {
			itemsInRange.Remove(other.gameObject);
		}
	}

}
