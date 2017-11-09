﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateSkill : MonoBehaviour
{
	public float distance;

	void Start()
	{
		Destroy(gameObject, 0.5f);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Enemy"))
		{
			//other.gameObject.GetComponent<Animator>().SetTrigger("DamageDown");
			other.gameObject.GetComponentInParent<Animator>().SetTrigger("DamageDown");
			other.gameObject.transform.root.position += (transform.forward * distance);

//			other.transform.LookAt(transform.position);
//
//			// OnTrigger way to look. Works for OnCollision as well.
//			Vector3 eulerFucker = other.transform.rotation.eulerAngles;
//			eulerFucker = new Vector3(0, eulerFucker.y, 0);
//			other.transform.rotation = Quaternion.Euler(eulerFucker);

			other.transform.root.LookAt(transform.position);

			Vector3 eulerFucker = other.transform.rotation.eulerAngles;
			eulerFucker = new Vector3(0, eulerFucker.y - 180f, 0);
			other.transform.root.rotation = Quaternion.Euler(eulerFucker);
		}
	}
}