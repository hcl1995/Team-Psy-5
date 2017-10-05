﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack02 : MonoBehaviour
{
	public Transform player;

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Enemy"))
		{
			//other.transform.LookAt(player);
			other.gameObject.GetComponent<Animator>().SetTrigger("OnHit02");
		}
	}
}