using UnityEngine;
using System.Collections;

public class Instruction_Disable : MonoBehaviour
{
	public GameObject instruction;
	public GameObject inventoryInstruction;
	public GameObject instruction1;
	public GameObject inventoryInstruction1;

	bool iFirstTime = false;

	// Use this for initialization
	void Start ()
	{
		instruction.SetActive (false);
		inventoryInstruction.SetActive (false);
		instruction1.SetActive (false);
		inventoryInstruction1.SetActive (false);
		StartCoroutine ("ShowInstruction", 20);
	}

	public void ShowInstructions()
	{
		instruction.SetActive (false);
		inventoryInstruction.SetActive (false);
		instruction1.SetActive (false);
		inventoryInstruction1.SetActive (false);
		StartCoroutine ("ShowInstruction", 20);

		iFirstTime = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.I) && !iFirstTime) {
			iFirstTime = true;
			StartCoroutine ("ShowInventoryInstruction", 20);
			StopCoroutine("ShowInstruction");
			instruction.SetActive(false);
			instruction1.SetActive(false);
		}
	}

	IEnumerator ShowInstruction(float duration)
	{
		instruction.SetActive (true);
		yield return new WaitForSeconds (duration/2);
		instruction.SetActive (false);
		instruction1.SetActive (true);
		yield return new WaitForSeconds (duration/2);
		instruction1.SetActive (false);
	}

	IEnumerator ShowInventoryInstruction(float duration)
	{
		inventoryInstruction.SetActive (true);
		yield return new WaitForSeconds (duration/2);
		inventoryInstruction.SetActive (false);
		inventoryInstruction1.SetActive (true);
		yield return new WaitForSeconds (duration/2);
		inventoryInstruction1.SetActive (false);
	}
}
