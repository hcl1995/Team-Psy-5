using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
	Rigidbody rb;

	int bounceLimit;
	Vector3 initialVelocity;
	Vector3 startPos;
	Vector3 endPos;
	float travelingDistance;
	float minVelocity = 10f;
	public float projectileSpeed;
	public float travelingThreshold;

	public int damage = 10;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	void Start()
	{
		initialVelocity = transform.forward * projectileSpeed;
		rb.velocity = initialVelocity;
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

			if (PlayerControl03.Instance.maxCharge)
			{
				other.gameObject.transform.Translate (dir);
			}
		}
		var hit = other.gameObject;
		var health = hit.GetComponent<Health>();
		if (health != null)
		{
			health.TakeDamage(damage);
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