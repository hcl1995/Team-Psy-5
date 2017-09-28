using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
	Rigidbody rb;

	int bounceLimit;

	Vector3 startPos;
	Vector3 endPos;
	float travelingDistance;

	public float projectileSpeed;
	public float travelingThreshold;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	void Start()
	{
		startPos = transform.position;
	}

	void Update()
	{
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

	void FixedUpdate()
	{
		rb.AddForce(transform.forward * projectileSpeed);
	}

	void OnCollisionEnter(Collision other)
	{
		bounceLimit++;

		Vector3 dir = other.contacts[0].point - transform.position;
		dir = -dir.normalized;

		if (other.gameObject.CompareTag("Enemy"))
		{
			other.gameObject.GetComponent<Animator>().SetTrigger("DamageDown");

			if (PlayerControl02.Instance.maxCharge || PlayerControl03.Instance.maxCharge)
			{
				other.gameObject.transform.Translate (dir);
			}
		}
	}
}