﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Player_Inventory : NetworkBehaviour
{
	public GameObject InventoryUI;
	public GameObject WeaponGrid;
	public GameObject AmmoGrid;

	public GameObject Grid11;
	public GameObject Grid12;
	public GameObject Grid13;
	public GameObject Grid14;

	public GameObject Grid21;
	public GameObject Grid22;
	public GameObject Grid23;
	public GameObject Grid24;

	public GameObject Grid31;
	public GameObject Grid32;
	public GameObject Grid33;
	public GameObject Grid34;

	public GameObject Grid41;
	public GameObject Grid42;
	public GameObject Grid43;
	public GameObject Grid44;

	public GameObject Grid51;
	public GameObject Grid52;
	public GameObject Grid53;
	public GameObject Grid54;

	public GameObject GridTrash;

	private Color gridColor;
	private Sprite gridImage;

	private Color selectedColor;
	private string selectedName;
	private bool isSelected;

	public Sprite ammoSprite;
	public Sprite foodSprite;
	public Sprite batSprite;
	public Sprite medkitSprite;
	public Sprite pistolSprite;
	public Sprite woodSprite;
	public Sprite trashSprite;

	public Dictionary<string,Sprite> items;
	public List<string> grids;

	public GameObject gridAction;

	public GameObject ammoPrefab;
	public GameObject foodPrefab;
	public GameObject batPrefab;
	public GameObject medkitPrefab;
	public GameObject pistolPrefab;
	public GameObject woodPrefab;

	public AudioClip openInventorySound;
	public AudioClip eatFoodSound;
	public AudioClip pickUpSound;

	public int numFree;

	// Use this for initialization
	void Start ()
	{
		gridColor = Grid11.GetComponent<Image> ().color;
		gridImage = Grid11.GetComponent<Image> ().sprite;
		selectedColor = Color.white;

		items = new Dictionary<string,Sprite > ();
		grids = new List<string> ();

		items.Add("Grid 1 1", gridImage);
		items.Add("Grid 1 2", gridImage);
		items.Add("Grid 1 3", gridImage);
		items.Add("Grid 1 4", gridImage);

		items.Add("Grid 2 1", gridImage);
		items.Add("Grid 2 2", gridImage);
		items.Add("Grid 2 3", gridImage);
		items.Add("Grid 2 4", gridImage);

		items.Add("Grid 3 1", gridImage);
		items.Add("Grid 3 2", gridImage);
		items.Add("Grid 3 3", gridImage);
		items.Add("Grid 3 4", gridImage);

		items.Add("Grid 4 1", gridImage);
		items.Add("Grid 4 2", gridImage);
		items.Add("Grid 4 3", gridImage);
		items.Add("Grid 4 4", gridImage);

		items.Add("Grid 5 1", gridImage);
		items.Add("Grid 5 2", gridImage);
		items.Add("Grid 5 3", gridImage);
		items.Add("Grid 5 4", gridImage);

		items.Add("Grid Weapon", gridImage);
		items.Add("Grid Ammo", gridImage);
		items.Add("Grid Trash", trashSprite);

		grids.Add("Grid 1 1");
		grids.Add("Grid 1 2");
		grids.Add("Grid 1 3");
		grids.Add("Grid 1 4");
		
		grids.Add("Grid 2 1");
		grids.Add ("Grid 2 2");
		grids.Add("Grid 2 3");
		grids.Add("Grid 2 4");
		
		grids.Add("Grid 3 1");
		grids.Add("Grid 3 2");
		grids.Add("Grid 3 3");
		grids.Add("Grid 3 4");
		
		grids.Add("Grid 4 1");
		grids.Add("Grid 4 2");
		grids.Add("Grid 4 3");
		grids.Add("Grid 4 4");
		
		grids.Add("Grid 5 1");
		grids.Add("Grid 5 2");
		grids.Add("Grid 5 3");
		grids.Add("Grid 5 4");
		
		grids.Add("Grid Weapon");
		grids.Add("Grid Ammo");
		grids.Add("Grid Trash");

		items ["Grid 1 1"] = ammoSprite;
		items ["Grid 1 3"] = ammoSprite;
		items ["Grid 1 2"] = foodSprite;
		items ["Grid 1 4"] = medkitSprite;

		selectedName = "";
		InventoryUI.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!isLocalPlayer)
			return;

		foreach(string i in grids)
		{
			if(i == "Grid 1 1") Grid11.GetComponent<Image>().sprite = items[i];
			if(i == "Grid 1 2") Grid12.GetComponent<Image>().sprite = items[i];
			if(i == "Grid 1 3") Grid13.GetComponent<Image>().sprite = items[i];
			if(i == "Grid 1 4") Grid14.GetComponent<Image>().sprite = items[i];

			if(i == "Grid 2 1") Grid21.GetComponent<Image>().sprite = items[i];
			if(i == "Grid 2 2") Grid22.GetComponent<Image>().sprite = items[i];
			if(i == "Grid 2 3") Grid23.GetComponent<Image>().sprite = items[i];
			if(i == "Grid 2 4") Grid24.GetComponent<Image>().sprite = items[i];

			if(i == "Grid 3 1") Grid31.GetComponent<Image>().sprite = items[i];
			if(i == "Grid 3 2") Grid32.GetComponent<Image>().sprite = items[i];
			if(i == "Grid 3 3") Grid33.GetComponent<Image>().sprite = items[i];
			if(i == "Grid 3 4") Grid34.GetComponent<Image>().sprite = items[i];
			if(i == "Grid 4 1") Grid41.GetComponent<Image>().sprite = items[i];
			if(i == "Grid 4 2") Grid42.GetComponent<Image>().sprite = items[i];
			if(i == "Grid 4 3") Grid43.GetComponent<Image>().sprite = items[i];
			if(i == "Grid 4 4") Grid44.GetComponent<Image>().sprite = items[i];

			if(i == "Grid 5 1") Grid51.GetComponent<Image>().sprite = items[i];
			if(i == "Grid 5 2") Grid52.GetComponent<Image>().sprite = items[i];
			if(i == "Grid 5 3") Grid53.GetComponent<Image>().sprite = items[i];
			if(i == "Grid 5 4") Grid54.GetComponent<Image>().sprite = items[i];

			if(i == "Grid Weapon") WeaponGrid.GetComponent<Image>().sprite = items[i];
			if(i == "Grid Ammo") AmmoGrid.GetComponent<Image>().sprite = items[i];
			if(i == "Grid Trash") GridTrash.GetComponent<Image>().sprite = items[i];
		}

		// Check number of free grids
		numFree = 0;

		foreach (string i in grids)
		{
			if((i.StartsWith("Grid 1") ||  i.StartsWith("Grid 2") ||i.StartsWith("Grid 3") ||i.StartsWith("Grid 4") ||i.StartsWith("Grid 5"))&& items[i].Equals(gridImage)) numFree++;
		}

		if (Input.GetKeyDown (KeyCode.I))
		{
			if(InventoryUI.gameObject.activeInHierarchy)
			{
				InventoryUI.SetActive(false);

				this.gameObject.GetComponent<Player_BasicAttacks>().enabled = true;
				this.gameObject.GetComponent<Player_BasicMovement>().enabled = true;
				this.gameObject.GetComponent<Player_BasicRotation>().enabled = true;
				this.gameObject.GetComponent<Player_Camera_BasicRotation>().enabled = true;

				Cursor.visible = false;
			}
			else
			{
				AudioSource.PlayClipAtPoint (openInventorySound, this.transform.position);
				InventoryUI.SetActive(true);

				this.gameObject.GetComponent<Player_BasicAttacks>().enabled = false;
				this.gameObject.GetComponent<Player_BasicMovement>().enabled = false;
				this.gameObject.GetComponent<Player_BasicRotation>().enabled = false;
				this.gameObject.GetComponent<Player_Camera_BasicRotation>().enabled = false;

				Cursor.visible = true;
			}
		}

		if (selectedName != "")
		{
			if(items[selectedName].Equals (medkitSprite))
			{
				gridAction.SetActive (true);
				gridAction.GetComponentInChildren<UnityEngine.UI.Text>().text = "Heal";
			}
			else
			if(items[selectedName].Equals (foodSprite))
			{
				gridAction.SetActive (true);
				gridAction.GetComponentInChildren<UnityEngine.UI.Text>().text = "Eat";
			}
			else
			if(items[selectedName].Equals (woodSprite))
			{
				gridAction.SetActive (true);
				gridAction.GetComponentInChildren<UnityEngine.UI.Text>().text = "Throw";
			}
			else
			if(items[selectedName].Equals (ammoSprite) || items[selectedName].Equals (pistolSprite) || items[selectedName].Equals (batSprite)) 
			{
				gridAction.SetActive (true);
				gridAction.GetComponentInChildren<UnityEngine.UI.Text>().text = "Equip";
			}
		}
		else
		{
			gridAction.SetActive (false);
		}
	}

	public void LeftClickGrid(GameObject button)
	{
		if (!isLocalPlayer)
			return;

		if (isSelected) 
		{
			if(button.name == "Grid Trash") 
			{
				ThrowAwayItem(selectedName);
			}
			else if((!(items[selectedName].Equals(batSprite) || items[selectedName].Equals(pistolSprite))) && button.name == "Grid Weapon")
			{

			}
			else if(!items[selectedName].Equals(ammoSprite) && button.name == "Grid Ammo")
			{

			}
			else if(selectedName == "Grid Weapon" && !(items[button.name].Equals(pistolSprite) || items[button.name].Equals(batSprite)))
			{

			}
			else if(!items[button.name].Equals(ammoSprite) && selectedName == "Grid Ammo")
			{

			}
			else
			{
				Sprite tempImag = items[button.name];
				items[button.name] = items[selectedName];
				items[selectedName] = tempImag;
			}

			isSelected = false;
			GameObject.Find(selectedName).GetComponent<Image>().color = gridColor;
			selectedName = "";

		} else 
		{
			// Cannot select trash grid
			if(button.name == "Grid Trash") return;
			// Cannot select ammo grid
			if(button.name == "Grid Ammo") return;
			// Cannot select empty grid
			if(items[button.name].Equals(gridImage)) return;

			isSelected = true;
			selectedName = button.name;
			button.GetComponent<Image>().color = selectedColor;
		}

		if (items ["Grid Weapon"].Equals (pistolSprite))
			this.GetComponent<Player_BasicAttacks>().EquipPistol();

		if(items["Grid Weapon"].Equals(batSprite))
		   this.GetComponent<Player_BasicAttacks>().EquipBat();

		if(items["Grid Weapon"].Equals(gridImage))
			this.GetComponent<Player_BasicAttacks>().Unequip();

		Debug.Log (button.ToString());
	}

	public void ThrowAwayItem(string itemGridName)
	{
		if (!isLocalPlayer)
			return;

		if (items [itemGridName].Equals (trashSprite) || items [itemGridName].Equals (gridImage)) {
			return;
		}

		if (items [itemGridName].Equals (ammoSprite))
		{
			if(isServer)
			{
				GameObject temp = (Instantiate(ammoPrefab, this.transform.position + this.transform.forward, Quaternion.identity) as GameObject);
				temp.name = ammoPrefab.name;
				NetworkServer.Spawn(temp);
			}
			else
			{
				CmdSpawn(ammoPrefab.name, this.gameObject);
			}
		}

		if (items [itemGridName].Equals (foodSprite)) {
			if(isServer)
			{
				GameObject temp = (Instantiate(foodPrefab, this.transform.position + this.transform.forward, Quaternion.identity) as GameObject);
				temp.name = foodPrefab.name;
				NetworkServer.Spawn(temp);
			}
			else
			{
				CmdSpawn(foodPrefab.name, this.gameObject);
			}
		}

		if (items [itemGridName].Equals (medkitSprite)) {
			if(isServer)
			{
				GameObject temp = (Instantiate(medkitPrefab, this.transform.position + this.transform.forward, Quaternion.identity) as GameObject);
				temp.name = medkitPrefab.name;
				NetworkServer.Spawn(temp);
			}
			else
			{
				CmdSpawn(medkitPrefab.name, this.gameObject);
			}
		}

		if (items [itemGridName].Equals (batSprite)) {
			if(isServer)
			{
				GameObject temp = (Instantiate(batPrefab, this.transform.position + this.transform.forward, Quaternion.identity) as GameObject);
				temp.name = batPrefab.name;
				NetworkServer.Spawn(temp);
			}
			else
			{
				CmdSpawn(batPrefab.name, this.gameObject);
			}
		}

		if (items [itemGridName].Equals (woodSprite)) {
			if(isServer)
			{
				GameObject temp = (Instantiate(woodPrefab, this.transform.position + this.transform.forward, Quaternion.identity) as GameObject);
				temp.name = woodPrefab.name;
				NetworkServer.Spawn(temp);
			}
			else
			{
				CmdSpawn(woodPrefab.name, this.gameObject);
			}
		}

		if (items [itemGridName].Equals (pistolSprite)) {
			if(isServer)
			{
				GameObject temp = (Instantiate(pistolPrefab, this.transform.position + this.transform.forward, Quaternion.identity) as GameObject);
				temp.name = pistolPrefab.name;
				NetworkServer.Spawn(temp);
			}
			else
			{
				CmdSpawn(pistolPrefab.name, this.gameObject);
			}
		}

		items [itemGridName] = gridImage;
	}

	[Command]
	void CmdSpawn(string nm, GameObject pl)
	{
		GameObject temp = new GameObject ();

		if (nm == "Ammo")
			temp = (Instantiate(ammoPrefab, pl.transform.position + pl.transform.forward, Quaternion.identity) as GameObject);
		if (nm == "Food")
			temp = (Instantiate(foodPrefab, pl.transform.position + pl.transform.forward, Quaternion.identity) as GameObject);
		if (nm == "Baseball Bat")
			temp = (Instantiate(batPrefab, pl.transform.position + pl.transform.forward, Quaternion.identity) as GameObject);
		if (nm == "Medkit")
			temp = (Instantiate(medkitPrefab, pl.transform.position + pl.transform.forward, Quaternion.identity) as GameObject);
		if (nm == "Pistol")
			temp = (Instantiate(pistolPrefab, pl.transform.position + pl.transform.forward, Quaternion.identity) as GameObject);
		if (nm == "Wood")
			temp = (Instantiate(woodPrefab, pl.transform.position + pl.transform.forward, Quaternion.identity) as GameObject);

		temp.name = nm;
		NetworkServer.Spawn (temp);
	}

	[Command]
	void CmdSpawnWoodThrow(GameObject pl)
	{
		// Throw wood in front of player
		GameObject tempObj =  Instantiate(woodPrefab, pl.transform.position + pl.transform.forward, Quaternion.identity) as GameObject;
		tempObj.name = woodPrefab.name;
		StartCoroutine(ThrowWood(tempObj, pl));

		NetworkServer.Spawn (tempObj);

	}
	
	public void PickUpItem(string itemName)
	{
		if (!isLocalPlayer)
			return;

		AudioSource.PlayClipAtPoint (pickUpSound, this.transform.position);
		Sprite sprite = new Sprite ();

		if (itemName.StartsWith("Ammo") || itemName == "Ammo(Clone)")
			sprite = ammoSprite;
		if (itemName.StartsWith("Food") || itemName == "Food(Clone)")
			sprite = foodSprite;
		if (itemName.StartsWith("Baseball Bat") || itemName == "Baseball Bat(Clone)")
			sprite = batSprite;
		if (itemName.StartsWith("Medkit") || itemName == "Medkit(Clone)")
			sprite = medkitSprite;
		if (itemName.StartsWith("Pistol") || itemName == "Pistol(Clone)")
			sprite = pistolSprite;
		if (itemName.StartsWith("Wood") || itemName == "Wood(Clone)")
			sprite = woodSprite;

		foreach (string i in grids) {
			if(items[i].Equals(gridImage))
			{
				items[i] = sprite;
				return;
			}
		}

		Debug.Log ("Inventory is full...");
	}

	public void ActionGrid()
	{
		if (!isLocalPlayer)
			return;

		if (items [selectedName].Equals (medkitSprite))
		{
			// Heal Player
			this.GetComponent<Player_Health>().healPlayer(40);
			items[selectedName] = gridImage;
		}

		if (items [selectedName].Equals (foodSprite))
		{
			// Decrease Hunger Level
			AudioSource.PlayClipAtPoint(eatFoodSound, this.transform.position);
			this.gameObject.GetComponent<Player_Hunger>().AddHunger(20);
			items[selectedName] = gridImage;
		}

		if (items [selectedName].Equals (woodSprite))
		{
			if(isServer)
			{
				// Throw wood in front of player
				GameObject tempObj =  Instantiate(woodPrefab, this.transform.position + this.transform.forward, Quaternion.identity) as GameObject;
				tempObj.name = woodPrefab.name;
				StartCoroutine("ThrowWood", tempObj);
				NetworkServer.Spawn(tempObj);
			}
			else
			{
				CmdSpawnWoodThrow(this.gameObject);
			}

			items[selectedName] = gridImage;

			isSelected = false;
			GameObject.Find(selectedName).GetComponent<Image>().color = gridColor;
			selectedName = "";

			this.gameObject.GetComponent<Player_BasicAttacks>().enabled = true;
			this.gameObject.GetComponent<Player_BasicMovement>().enabled = true;
			this.gameObject.GetComponent<Player_BasicRotation>().enabled = true;
			this.gameObject.GetComponent<Player_Camera_BasicRotation>().enabled = true;
			
			Cursor.visible = false;

			InventoryUI.SetActive(false);

			return;
		}

		if (items [selectedName].Equals (pistolSprite) || items [selectedName].Equals (batSprite))
		{
			// Swap items with the current weapon

			Sprite tempImag = items["Grid Weapon"];
			items["Grid Weapon"] = items[selectedName];
			items[selectedName] = tempImag;

			if (items ["Grid Weapon"].Equals (pistolSprite))
				this.GetComponent<Player_BasicAttacks>().EquipPistol();
			
			if(items["Grid Weapon"].Equals(batSprite))
				this.GetComponent<Player_BasicAttacks>().EquipBat();
			
			if(items["Grid Weapon"].Equals(gridImage))
				this.GetComponent<Player_BasicAttacks>().Unequip();
		}

		if (items [selectedName].Equals (ammoSprite))
		{
			// Increase Player Ammo Supply
			this.gameObject.GetComponentInChildren<Pistol_Ammo>().AddAmmo(10);

			if(items["Grid Ammo"].Equals(gridImage))
			{
				items["Grid Ammo"] = ammoSprite;
			}

			items[selectedName] = gridImage;
		}

		isSelected = false;
		GameObject.Find(selectedName).GetComponent<Image>().color = gridColor;
		selectedName = "";
	}


	IEnumerator ThrowWood(GameObject wood)
	{
		wood.GetComponent<Rigidbody> ().AddExplosionForce (1000, this.transform.position + Vector3.down/2, 10);
		yield return new WaitForSeconds (2.0f);
		this.GetComponent<Player_Noise>().GenerateNoiseAtPosWithDistance(wood.transform.position, 15f);
	}

	IEnumerator ThrowWood(GameObject wood, GameObject pla)
	{
		wood.GetComponent<Rigidbody> ().AddExplosionForce (1000, pla.transform.position + Vector3.down/2, 10);
		yield return new WaitForSeconds (2.0f);
		this.GetComponent<Player_Noise>().GenerateNoiseAtPosWithDistance(wood.transform.position, 15f);
	}
	
	public void DropAllItems()
	{
		if (!isLocalPlayer)
			return;

		foreach(string i in grids)
		{
			ThrowAwayItem(i);
		}
	}
}
