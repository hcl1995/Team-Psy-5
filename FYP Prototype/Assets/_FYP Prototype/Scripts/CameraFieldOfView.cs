using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFieldOfView : MonoBehaviour
{
	public float distance;
	public GameObject[] m_Players;

	public CinemachineVirtualCamera vcam;

	void Update ()
	{
		m_Players = GameObject.FindGameObjectsWithTag("Player");

		distance = Vector3.Distance(m_Players[0].transform.position, m_Players[1].transform.position);

		vcam.m_Lens.FieldOfView = distance * 2.9f;

		if (vcam.m_Lens.FieldOfView >= 63)
		{
			vcam.m_Lens.FieldOfView = 63;
		}
	}
}