using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	// float cameraOffset = 9.5f;
	GameObject[] m_Players;

	void Update ()
	{
		m_Players = GameObject.FindGameObjectsWithTag("Player");

		transform.position = new Vector3((m_Players[0].transform.position.x + m_Players[1].transform.position.x) / m_Players.Length, 10.5f, -8.5f);

//		transform.position = new Vector3((m_Players[0].transform.position.x + m_Players[1].transform.position.x) / m_Players.Length,
//			10.5f, (m_Players[0].transform.position.z + m_Players[1].transform.position.z) / m_Players.Length - cameraOffset);
//
//		if (transform.position.z <= -8.5f)
//		{
//			transform.position = new Vector3((m_Players[0].transform.position.x + m_Players[1].transform.position.x) / m_Players.Length, 10.5f, -8.5f);
//		}
	}
}