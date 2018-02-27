using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DestructableWall : MonoBehaviour
{
	public float health = 3;
	public float fadeSpeed;

	bool yeBabe;
	Renderer rend;

	public GameObject[] m_Players;
	public PlayerControl[] playerControl;

	void Awake()
	{
		rend = GetComponent<Renderer>();
	}

	void Start()
	{
		m_Players = GameObject.FindGameObjectsWithTag("Player");

		for (int i = 0; i < m_Players.Length; i++)
		{
			playerControl[i] = m_Players[i].GetComponent<PlayerControl>();
		}
	}

	void Update()
	{
		//CmdRestoreAlpha();

		if (playerControl[0].inBetweenObjects.Contains(this.gameObject) || playerControl[1].inBetweenObjects.Contains(this.gameObject))
		{
			yeBabe = false;
		}
		else //if (!playerControl[0].inBetweenObjects.Contains(this.gameObject) && !playerControl[1].inBetweenObjects.Contains(this.gameObject))
		{
			yeBabe = true;
		}

		if (yeBabe)
		{
			Color tempColor = rend.material.color;
			if (tempColor.a < 1)
			{
				tempColor.a += fadeSpeed * Time.deltaTime;
				rend.material.color = tempColor;
			}
		}
	}

//	[Command]
//	void CmdRestoreAlpha()
//	{
//		RpcRestoreAlpha();
//	}
//
//	[ClientRpc]
//	void RpcRestoreAlpha()
//	{
//		if (playerControl[0].inBetweenObjects.Contains(this.gameObject) || playerControl[1].inBetweenObjects.Contains(this.gameObject))
//		{
//			yeBabe = false;
//		}
//		else
//		{
//			yeBabe = true;
//		}
//
//		if (yeBabe)
//		{
//			Color tempColor = rend.material.color;
//			if (tempColor.a < 1)
//			{
//				tempColor.a += fadeSpeed * Time.deltaTime;
//				rend.material.color = tempColor;
//			}
//		}
//	}

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