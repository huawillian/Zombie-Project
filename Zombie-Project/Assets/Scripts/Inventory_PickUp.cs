using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Inventory_PickUp : NetworkBehaviour
{
	public GameObject actionUI;
	
	public LinkedList<GameObject> itemsInRange;
	GameObject closestItem;

	Player_Inventory inventoryScript;

	public bool scriptActive = true;

	public AudioClip pickupSound;

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
		if (!isLocalPlayer)
			return;

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

			if(closestItem != null && inventoryScript.numFree > 0)
			{
				//actionUI.SetActive (true);
				actionUI.GetComponent<Text>().text = "Press 'E' to Pickup " + closestItem.name;
			}

			if(closestItem != null && inventoryScript.numFree == 0)
			{
				//actionUI.SetActive (true);
				actionUI.GetComponent<Text>().text = "Inventory is full";
			}

			if(Input.GetKeyDown(KeyCode.E))
			{
				if(closestItem != null && inventoryScript.numFree > 0)
				{
					this.GetComponentInChildren<Person_AnimationController>().StartCoroutine("SetPickUp");
					AudioSource.PlayClipAtPoint(pickupSound, this.transform.position);
					inventoryScript.PickUpItem(closestItem.name);
					itemsInRange.Remove(closestItem);

					if(isServer)
					{
						RpcDestroy(closestItem.gameObject);
						Destroy(closestItem.gameObject);
					}
					else
					{
						CmdDestroy(closestItem.gameObject);
					}

					closestItem = null;
				}
			}
		} 
		else
		{
			//actionUI.SetActive (false);
			closestItem = null;
		}
	}

	[ClientRpc]
	void RpcDestroy(GameObject obj)
	{
		Destroy(obj);
	}

	[Command]
	void CmdDestroy(GameObject obj)
	{
		RpcDestroy (obj);
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
