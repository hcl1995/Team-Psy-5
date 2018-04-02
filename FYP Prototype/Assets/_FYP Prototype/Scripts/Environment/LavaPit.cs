using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LavaPit : DestructableWall
{
	public GameObject meteorRock;
	public GameObject wtfDoubleMeshRock;

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
		if (health <= breakHP && callOnce == false)
		{
			AudioSource.PlayClipAtPoint(soundEffect.selfServiceClip[1], transform.position);

			int repeat = Random.Range(5, 20);
			for (int i = 0; i < repeat; i++)
			{
				int randomX = Random.Range(1, 23);
				int randomZ = Random.Range(1, 23);
				CmdSpawnMe (meteorRock, GetMeteorPosition(randomX, randomZ), Quaternion.identity);
			}

			CmdSpawnMe (wtfDoubleMeshRock, transform.position, transform.rotation);
			Destroy(gameObject);

			callOnce = true;
		}
	}

	[Command]
	void CmdSpawnMe(GameObject meteorNrock, Vector3 position, Quaternion rotation)
	{
		var meteorOrock = Instantiate(meteorNrock, position, rotation);
		NetworkServer.Spawn (meteorOrock);
	}
}