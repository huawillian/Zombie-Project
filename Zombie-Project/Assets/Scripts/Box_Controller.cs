using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Box_Controller : MonoBehaviour
{
	Search_Content searchScript;

	public GameObject baseballBatPrefab;
	public GameObject pistolPrefab;
	public GameObject ammoPrefab;
	public GameObject foodPrefab;
	public GameObject medkitPrefab;
	public GameObject woodPrefab;

	public GameObject player;

	private int health = 100;
	
	public int Health{
		get{
			return health;
		}
		set {
			if(value > 100) health = 100;
			else if(value <= 0)
			{
				health= 0;

				player.GetComponent<Player_Noise> ().GenerateNoiseAtPosWithDistance (this.transform.position, 30f);
				
				LinkedList<string> listOfItems = searchScript.getContent ();
				Vector3 pos = this.transform.position;
				
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

				Destroy(this.gameObject);
			}
			else 
			{
				health = value;
			}
		}
	}

	// Use this for initialization
	void Start ()
	{
		searchScript = this.GetComponent<Search_Content> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider obj)
	{
		if (obj.name == "Player")
			player = obj.gameObject;
	}
}
