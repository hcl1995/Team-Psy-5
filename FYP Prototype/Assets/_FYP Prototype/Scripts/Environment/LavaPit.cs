using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LavaPit : DestructableWall
{
	public GameObject meteorRock;

	protected override void Update()
	{
		LavaPitChuiDiao();
	}

	Vector3 GetMeteorPosition(int xpos, int zpos)
	{
		return new Vector3(xpos, 15, zpos);
	}

	void LavaPitChuiDiao()
	{
		if (health <= 0 && callOnce == false)
		{
			//gameObject.GetComponent<MeshFilter>().mesh = destroyMesh;
			RpcSetAnimation("Break");
			int repeat = Random.Range(5, 20);
			for (int i = 0; i < repeat; i++)
			{
				int randomX = Random.Range(1, 23);
				int randomZ = Random.Range(1, 23);
				RpcSpawnMeteor (GetMeteorPosition(randomX, randomZ), Quaternion.identity);
			}
			//			foreach (Collider c in m_Collider)
			//			{
			//				c.enabled = !c.enabled;
			//			}
			//m_Collider.enabled = !m_Collider.enabled;
			callOnce = true;
		}
	}

	[ClientRpc]
	void RpcSpawnMeteor(Vector3 position, Quaternion rotation)
	{
		var lavaRock = Instantiate(meteorRock, position, rotation);
		NetworkServer.Spawn (lavaRock);
	}
}