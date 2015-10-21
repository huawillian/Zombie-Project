using UnityEngine;
using System.Collections;

public class BaseballBat_Trigger : MonoBehaviour
{
	void OnTriggerEnter(Collider collider)
	{
		this.GetComponentInParent<BaseballBat_Weapon> ().hit(collider);
	}
}
