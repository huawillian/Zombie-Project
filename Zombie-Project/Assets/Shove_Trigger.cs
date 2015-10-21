using UnityEngine;
using System.Collections;

public class Shove_Trigger : MonoBehaviour {

	void OnTriggerEnter(Collider collider)
	{
		this.GetComponentInParent<Shove_Weapon> ().hit(collider);
	}
}
