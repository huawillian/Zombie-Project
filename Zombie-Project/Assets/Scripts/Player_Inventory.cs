using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Player_Inventory : MonoBehaviour
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
		items ["Grid 1 2"] = foodSprite;
		items ["Grid 1 3"] = batSprite;
		items ["Grid 1 4"] = medkitSprite;
		items ["Grid 2 1"] = pistolSprite;
		items ["Grid 2 2"] = woodSprite;

		selectedName = "";
		InventoryUI.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update ()
	{

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

		if (Input.GetKeyDown (KeyCode.I))
		{
			if(InventoryUI.gameObject.activeInHierarchy)
			{
				InventoryUI.SetActive(false);

				this.gameObject.GetComponent<Player_BasicAttacks>().enabled = true;
				this.gameObject.GetComponent<Player_BasicMovement>().enabled = true;
				this.gameObject.GetComponent<Player_BasicRotation>().enabled = true;
				this.gameObject.GetComponent<Player_Camera_BasicRotation>().enabled = true;
			}
			else
			{
				InventoryUI.SetActive(true);

				this.gameObject.GetComponent<Player_BasicAttacks>().enabled = false;
				this.gameObject.GetComponent<Player_BasicMovement>().enabled = false;
				this.gameObject.GetComponent<Player_BasicRotation>().enabled = false;
				this.gameObject.GetComponent<Player_Camera_BasicRotation>().enabled = false;
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
		if (items [itemGridName].Equals (ammoSprite)) {
			Instantiate(ammoPrefab, this.transform.position + this.transform.forward, Quaternion.identity);
		}

		if (items [itemGridName].Equals (foodSprite)) {
			Instantiate(foodPrefab, this.transform.position + this.transform.forward, Quaternion.identity);
		}

		if (items [itemGridName].Equals (medkitSprite)) {
			Instantiate(medkitPrefab, this.transform.position + this.transform.forward, Quaternion.identity);
		}

		if (items [itemGridName].Equals (batSprite)) {
			Instantiate(batPrefab, this.transform.position + this.transform.forward, Quaternion.identity);
		}

		if (items [itemGridName].Equals (woodSprite)) {
			Instantiate(woodPrefab, this.transform.position + this.transform.forward, Quaternion.identity);
		}

		if (items [itemGridName].Equals (pistolSprite)) {
			Instantiate(pistolPrefab, this.transform.position + this.transform.forward, Quaternion.identity);
		}

		items [itemGridName] = gridImage;
	}

	public void PickUpItem(string itemName)
	{
		Sprite sprite = new Sprite ();

		if (itemName == "Ammo")
			sprite = ammoSprite;
		if (itemName == "Food")
			sprite = foodSprite;
		if (itemName == "Baseball Bat")
			sprite = batSprite;
		if (itemName == "Medkit")
			sprite = medkitSprite;
		if (itemName == "Pistol")
			sprite = pistolSprite;
		if (itemName == "Wood")
			sprite = woodSprite;

		foreach (string i in grids) {
			if(items[i].Equals(gridImage))
			{
				items[i] = sprite;
				return;
			}
		}
	}

	public void ActionGrid()
	{
		if (items [selectedName].Equals (medkitSprite))
		{
			// Heal Player
			this.GetComponent<Player_Health>().healPlayer(20);
			items[selectedName] = gridImage;
		}

		if (items [selectedName].Equals (foodSprite))
		{
			// Decrease Hunger Level
			this.gameObject.GetComponent<Player_Hunger>().AddHunger(20);
			items[selectedName] = gridImage;
		}

		if (items [selectedName].Equals (woodSprite))
		{
			// Throw wood in front of player
			ThrowAwayItem(selectedName);
			items[selectedName] = gridImage;
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

	/*
	void OnGUI () {
		if(GUI.Button(new Rect(20,40,80,20), "Pickup Medkit"))
		{	
			PickUpItem("Medkit");
			
		}
	}*/
}
