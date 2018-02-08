using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraBlending : MonoBehaviour
{
	public CinemachineVirtualCamera vcam;
	//float minFOV;

	void Start()
	{
		//var composer = vcam.AddCinemachineComponent<CinemachineComposer>();
		//var composer = vcam.AddCinemachineComponent<CinemachineGroupComposer>();
		//composer.m_MaximumFOV = minFOV;
	}

	void Update()
	{
		//transform.eulerAngles = new Vector3 (transform.rotation.x, 0, transform.rotation.z);

		if (Camera.main.fieldOfView < 30)
		{
			vcam.enabled = true;
		}
		else
		{
			vcam.enabled = false;
		}
	}
}