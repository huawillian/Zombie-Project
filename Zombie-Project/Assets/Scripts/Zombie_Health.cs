using UnityEngine;
using System.Collections;

public class Zombie_Health : MonoBehaviour
{
	public GameObject corpsePrefab;

	[SerializeField]
	private int health = 100;

	public AudioClip damageSound;
	public AudioClip deathSound;

	public int Health{
		get{
			return health;
		}
		set {
			if(value > 100) health = 100;
			else if(value <= 0 && health != 0)
			{
				health= 0;
				AudioSource.PlayClipAtPoint(deathSound, this.transform.position);

				this.GetComponentInChildren<Zombie_AnimatorController>().StartCoroutine("setDeath");
				this.GetComponent<NavMeshAgent>().enabled = false;
				this.GetComponent<Zombie_BasicMovement>().StopAllCoroutines();

				CapsuleCollider[] cols = this.GetComponentsInChildren<CapsuleCollider>();

				foreach(CapsuleCollider col in cols)
				{
					if(col.gameObject.activeInHierarchy)
						col.gameObject.SetActive(false);
				}

				this.GetComponent<Rigidbody>().isKinematic = true;
				this.transform.localPosition += Vector3.down;

				this.gameObject.name = "Corpse";
				gameObject.AddComponent<Search_Content>();
				gameObject.AddComponent<BoxCollider>().isTrigger = true;
				this.gameObject.tag = "Search";

				//Destroy(this.gameObject);
			}
			else 
			{
				AudioSource.PlayClipAtPoint(damageSound, this.transform.position);
				this.GetComponentInChildren<Zombie_AnimatorController>().StartCoroutine("setHurt");
				health = value;
			}
		}
	}

	public void damageZombie(int damage)
	{
		Health -= damage;
		this.GetComponent<Zombie_BasicMovement> ().isDamaged = true;
		this.GetComponent<Zombie_BasicMovement> ().StopCoroutine ("ResetDamaged");
		this.GetComponent<Zombie_BasicMovement> ().StartCoroutine ("ResetDamaged");
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
