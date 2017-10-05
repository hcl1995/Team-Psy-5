﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSkill : MonoBehaviour
{
	Vector3 initialVelocity;

	[SerializeField]
	float minVelocity = 10f;

	Vector3 lastFrameVelocity;
	Rigidbody rb;

	int bounceLimit;
	float travelingDistance;

	Vector3 startPos;
	Vector3 endPos;

	public float projectileSpeed;
	public float travelingThreshold;

	void OnEnable()
	{
		rb = GetComponent<Rigidbody>();
		initialVelocity = transform.forward * projectileSpeed;
		rb.velocity = initialVelocity;

		startPos = transform.position;
	}

	void Update()
	{
		//lastFrameVelocity = rb.velocity;

		endPos = transform.position;

		if (bounceLimit >= 2)
		{
			Destroy(gameObject);
		}

		travelingDistance = Vector3.Distance(startPos, endPos);

		if (travelingDistance >= travelingThreshold)
		{
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter(Collision other)
	{
		bounceLimit++;

		Vector3 dir = other.contacts[0].point - transform.position;
		dir = -dir.normalized;

		if (other.gameObject.CompareTag("Enemy"))
		{
			Quaternion rotation = Quaternion.LookRotation(dir);
			other.transform.rotation = rotation;
			other.gameObject.GetComponent<Animator>().SetTrigger("DamageDown");

			if (PlayerControl02.Instance.maxCharge || PlayerControl03.Instance.maxCharge)
			{
				other.gameObject.transform.Translate (dir);
			}
		}

		Bounce(other.contacts[0].normal);
	}

	void Bounce(Vector3 collisionNormal)
	{
		//var speed = lastFrameVelocity.magnitude;
		//var direction = Vector3.Reflect(lastFrameVelocity.normalized, collisionNormal);

		var speed = initialVelocity.magnitude;
		var direction = Vector3.Reflect(initialVelocity.normalized, collisionNormal);

		rb.velocity = direction * Mathf.Max(speed, minVelocity);
	}
}