using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TransparentRecover : NetworkBehaviour
{
	public bool yeBabe;
	Renderer rend;

	public float fadeSpeed;

	public GameObject[] m_Players;
	public PlayerControl[] playerControl;

	void Awake()
	{
		rend = GetComponent<Renderer>();
	}

	void Start()
	{
		m_Players = GameObject.FindGameObjectsWithTag("Player");

		// set the size beforehand WTF..
		for (int i = 0; i < m_Players.Length; i++)
		{
			playerControl[i] = m_Players[i].GetComponent<PlayerControl>();
		}
	}

	void Update()
	{
		if (playerControl[0].inBetweenObjects.Contains(this.gameObject) || playerControl[1].inBetweenObjects.Contains(this.gameObject))
		{
			yeBabe = false;
		}
		else //if (!playerControl[0].inBetweenObjects.Contains(this.gameObject) && !playerControl[1].inBetweenObjects.Contains(this.gameObject))
		{
			yeBabe = true;
		}

		CmdRestoreAlpha();

		//		if (yeBabe)
		//		{
		//			Color tempColor = rend.material.color;
		//			if (tempColor.a < 1)
		//			{
		//				tempColor.a += fadeSpeed * Time.deltaTime;
		//				rend.material.color = tempColor;
		//			}
		//		}
	}

	[Command]
	void CmdRestoreAlpha()
	{
		RpcRestoreAlpha();
	}

	[ClientRpc]
	void RpcRestoreAlpha()
	{
		//		if (playerControl[0].inBetweenObjects.Contains(this.gameObject) || playerControl[1].inBetweenObjects.Contains(this.gameObject))
		//		{
		//			yeBabe = false;
		//		}
		//		else
		//		{
		//			yeBabe = true;
		//		}

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
}