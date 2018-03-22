using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Meteorite : NetworkBehaviour
{
	public GameObject burntGround;

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