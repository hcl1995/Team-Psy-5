using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFieldOfView : MonoBehaviour
{
	float distance;
	public float maxFov;

	GameObject[] m_Players;
	CinemachineVirtualCamera vcam;

	void Awake()
	{
		vcam = GetComponent<CinemachineVirtualCamera>();
	}

	void Update ()
	{
		m_Players = GameObject.FindGameObjectsWithTag("Player");
		if (m_Players.Length <= 1)
			return;
		distance = Vector3.Distance(m_Players[0].transform.position, m_Players[1].transform.position);

		vcam.m_Lens.FieldOfView = distance * 2.9f;
		if (vcam.m_Lens.FieldOfView >= maxFov)
		{
			vcam.m_Lens.FieldOfView = maxFov;
		}
	}
}