using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Box_Controller : NetworkBehaviour
{
	Search_Content searchScript;

	Player_BoxManager boxManagerWithAuth;

	[SyncVar, SerializeField]
	private string uniqueName;

	[SyncVar, SerializeField]
	private int health = 100;

	[SyncVar]
	private Vector3 pos;
	[SyncVar]
	private Vector3 rot;

	[SyncVar]
	private float qx;
	[SyncVar]
	private float qy;
	[SyncVar]
	private float qz;
	[SyncVar]
	private float qw;

	private bool isDead = false;

	public int Health
	{
		get{
			return health;
		}
		set {
			if(value > 100) health = 100;
			else if(value <= 0)
			{
				health= 0;
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
		NetworkProximityChecker prox = this.GetComponent<NetworkProximityChecker> ();
		prox.visRange = 25;
		prox.visUpdateInterval = Random.Range (5f,10f);


		searchScript = this.GetComponent<Search_Content> ();

		foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
		{
			if(player.GetComponent<NetworkIdentity>().isLocalPlayer)
			{
				boxManagerWithAuth = player.GetComponent<Player_BoxManager>();
			}
		}
	}

	void Update()
	{
		if (isServer) {
			pos = this.transform.position;
			rot = this.transform.localEulerAngles;
			qx = this.transform.localRotation.x;
			qy = this.transform.localRotation.y;
			qz = this.transform.localRotation.z;
			qw = this.transform.localRotation.w;

			if(!isDead && Health == 0)
			{
				if (boxManagerWithAuth == null)
					return;

				isDead = true;
				boxManagerWithAuth.DestroyBox(uniqueName);
			}

		} else
		{
			this.transform.position = Vector3.MoveTowards(this.transform.position, pos, Time.deltaTime * 15f);
			this.transform.localRotation =  Quaternion.Lerp(this.transform.localRotation, new Quaternion(qx,qy,qz,qw), Time.deltaTime * 15f);
		}
	}

	public override void OnStartServer()
	{
		uniqueName = "Box_" + this.gameObject.GetInstanceID ().ToString ();
		this.name = uniqueName;
	}

	public override void OnStartClient()
	{
		this.name = uniqueName;

		if (!isServer)
		{
			this.GetComponent<Rigidbody> ().useGravity = false;
			this.GetComponent<Rigidbody> ().isKinematic = true;
		}
	}

	void OnTriggerEnter(Collider obj)
	{
		if (boxManagerWithAuth == null)
			return;

		if (obj.name.StartsWith ("Shove Weapon") && obj.GetComponentInParent<Shove_Weapon>().isShoving)
		{
			boxManagerWithAuth.BoxForceLocDmgClient(uniqueName, 1000f, obj.gameObject.transform.position, 10);
		}

		if (obj.name.StartsWith ("Baseball Bat Weapon") && obj.GetComponentInParent<BaseballBat_Weapon>().isAttacking)
		{
			boxManagerWithAuth.BoxForceLocDmgClient(uniqueName, 500f, obj.gameObject.transform.position, 20);
		}
	}
}
