using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DestructableWall : NetworkBehaviour
{
	public Mesh destroyMesh;

	bool callOnce = false;
	public float health = 3;

	void Update()
	{
		CmdSwapMesh();
	}

	[Command]
	void CmdSwapMesh()
	{
		RpcSwapMesh();
		if (health <= 1.5f && callOnce == false)
		{
			gameObject.GetComponent<MeshFilter>().mesh = destroyMesh;
			callOnce = true;
		}
	}

	[ClientRpc]
	void RpcSwapMesh()
	{
		if (health <= 1.5f && callOnce == false)
		{
			gameObject.GetComponent<MeshFilter>().mesh = destroyMesh;
			callOnce = true;
		}
	}

	void OnCollisionEnter(Collision other)
	{
		
		if (other.gameObject.CompareTag("Attack"))
		{
			health -= 1;
			if(health<=0)
				Destroy(gameObject);			
		}
	}

	void OnTriggerEnter(Collider other){
		health -= 1;
		if(health<=0)
			Destroy(gameObject);	
	}
}