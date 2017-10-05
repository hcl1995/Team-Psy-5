using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterCamera : MonoBehaviour
{
	public Transform[] playerTransforms;

	void Start()
	{
		GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
		playerTransforms = new Transform[allPlayers.Length];

		for (int i = 0; i < allPlayers.Length; i++)
		{
			playerTransforms[i] = allPlayers[i].transform;
		}
	}

	public float xOffset = 0;
	public float zOffset = -2;
	public float minDistance = 3.5f;

	float xMin, xMax, zMin, zMax;

	void LateUpdate()
	{
		if (playerTransforms.Length == 0)
		{
			Debug.Log("have no found a player, make sure the player tag is on");
			return;
		}

		xMin = xMax = playerTransforms[0].position.x;
		zMin = zMax = playerTransforms[0].position.z;

		for (int i = 1; i < playerTransforms.Length; i++)
		{
			if (playerTransforms[i].position.x < xMin)
				xMin = playerTransforms[i].position.x;
			if (playerTransforms[i].position.x > xMax)
				xMax = playerTransforms[i].position.x;
			if (playerTransforms[i].position.z < zMin)
				zMin = playerTransforms[i].position.z;
			if (playerTransforms[i].position.z > zMax)
				zMax = playerTransforms[i].position.z;
		}

		float xMiddle = (xMin + xMax) / 2;
		float zMiddle = (zMin + zMax) / 2;
		float distance = (xMax - xMin) + (zMax - zMin);

		if (distance < minDistance)
			distance = minDistance;
		
		transform.position = new Vector3(xMiddle + xOffset, distance, zMiddle + zOffset);
	}﻿
}