using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Meteorite : NetworkBehaviour
{
	public GameObject burntGround;
	public float damage = 20.0f;

	void Start()
	{
		Destroy(gameObject, 3.0f);
	}

	void Update()
	{
		transform.position += (Vector3.down * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other)
	{
		if (isServer)
		{
			if (other.gameObject.CompareTag("Ground"))
			{
				CmdBurntLand(other.transform.position, other.transform.rotation);
				Destroy(other.gameObject);
			}

			if (other.gameObject.CompareTag("Player"))
			{
				other.gameObject.GetComponent<PlayerHealth> ().takeSkill02 (damage, "DamageDown02");
				// instantiate particle
				Destroy(gameObject);
			}

			if (other.gameObject.layer == LayerMask.NameToLayer("AboveGround") || other.gameObject.CompareTag("CameraShaker")){
				// instantiate particle
				Destroy(gameObject);
			}
		}
	}

	[Command]
	void CmdBurntLand(Vector3 position, Quaternion rotation)
	{
		var burntLand = Instantiate(burntGround, position, rotation);
		NetworkServer.Spawn (burntLand);
	}

	[ClientRpc]
	void RpcBurntLand(Vector3 position, Quaternion rotation)
	{
		var burntLand = Instantiate(burntGround, position, rotation);
		NetworkServer.Spawn (burntLand);
	}
}