using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraBlending : MonoBehaviour
{
	float distance;
	float centerOfZ;

	GameObject[] m_Players;

	public CinemachineVirtualCamera vcam;
	public CinemachineVirtualCamera vcam02;
	public CinemachineVirtualCamera vcam03;


	void Update()
	{
		m_Players = GameObject.FindGameObjectsWithTag("Player");
		if (m_Players.Length <= 1)
			return;
		distance = Vector3.Distance(m_Players[0].transform.position, m_Players[1].transform.position);
		centerOfZ = (m_Players[0].transform.position.z + m_Players[1].transform.position.z) / m_Players.Length;

		if (centerOfZ <= 11 && distance > 12.5f)
		{
			vcam.enabled = false;
			vcam02.enabled = false;
			vcam03.enabled = true;
		}
		else if (centerOfZ >= 11 && distance > 12.5f)
		{
			vcam.enabled = true;
			vcam02.enabled = false;
			vcam03.enabled = false;
		}
		else if (distance < 12.5f)
		{
			vcam.enabled = false;
			vcam02.enabled = true;
			vcam03.enabled = false;
		}
	}
}