using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
	GameObject[] m_Players;

	void Update ()
	{
		m_Players = GameObject.FindGameObjectsWithTag("Player");
		if (m_Players.Length <= 1)
			return;
		transform.position = new Vector3((m_Players[0].transform.position.x + m_Players[1].transform.position.x) / m_Players.Length, 0, 5.75f);

//		transform.position = new Vector3((m_Players[0].transform.position.x + m_Players[1].transform.position.x) / m_Players.Length,
//							 0, (m_Players[0].transform.position.z + m_Players[1].transform.position.z) / m_Players.Length);
	}
}