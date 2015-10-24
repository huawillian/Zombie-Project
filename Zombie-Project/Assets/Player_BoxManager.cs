using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_BoxManager : NetworkBehaviour
{
	public AudioClip hitSound;
	public GameObject woodPrefab;

	public void BoxForceLocDmgClient(string boxName, float explosionForce, Vector3 location, int damage)
	{
		if (isServer)
		{
			GameObject box = GameObject.Find(boxName);
			AudioSource.PlayClipAtPoint (hitSound, location);
			box.GetComponent<Rigidbody> ().AddExplosionForce (explosionForce, location, 5);
			box.GetComponent<Box_Controller> ().Health -= damage;

			RpcClientPlaySound(location);
		} else
		{
			CmdServerHitBox(boxName, explosionForce, location, damage);
		}
	}

	[ClientRpc]
	private void RpcClientPlaySound(Vector3 loc)
	{
		AudioSource.PlayClipAtPoint (hitSound, loc);
	}

	[Command]
	private void CmdServerHitBox(string boxName, float explosionForce, Vector3 location, int damage)
	{
		GameObject box = GameObject.Find(boxName);
		AudioSource.PlayClipAtPoint (hitSound, location);
		box.GetComponent<Rigidbody> ().AddExplosionForce (explosionForce, location, 5);
		box.GetComponent<Box_Controller> ().Health -= damage;

		RpcClientPlaySound(location);
	}

	public void DestroyBox(string boxName)
	{
		if (isServer)
		{
			for(int i=0; i<4;i++)
			{
				GameObject wood = (GameObject) Instantiate(woodPrefab, GameObject.Find(boxName).transform.position, Quaternion.identity);
				wood.GetComponent<Rigidbody> ().AddExplosionForce (500, wood.transform.position, 5);
				NetworkServer.Spawn(wood);
			}

			Destroy(GameObject.Find(boxName));
		} else
		{
			CmdDestroyBox(boxName);
		}
	}

	[Command]
	private void CmdDestroyBox(string boxName)
	{
		for(int i=0; i<4;i++)
		{
			NetworkServer.Spawn((GameObject) Instantiate(woodPrefab, GameObject.Find(boxName).transform.position, Quaternion.identity));
		}
			
		Destroy(GameObject.Find(boxName));
	}

}
