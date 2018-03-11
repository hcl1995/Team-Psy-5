using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UnityChanSkill01 : NetworkBehaviour
{
	Rigidbody rb;

	float travelingDistance;

	Vector3 startPos;
	Vector3 endPos;

	public float projectileSpeed;
	public float travelingThreshold;

	public float damage = 10;

	void OnEnable()
	{
		rb = GetComponent<Rigidbody>();
		rb.velocity = transform.forward * projectileSpeed;

		startPos = transform.position;
	}

	void Update()
	{
		endPos = transform.position;

		travelingDistance = Vector3.Distance(startPos, endPos);

		if (travelingDistance >= travelingThreshold)
		{
			Destroy(transform.root.gameObject);
		}
	}

//	void OnCollisionEnter(Collision other)
//	{
//		if (other.gameObject.CompareTag("Player"))
//		{
//			//other.gameObject.GetComponent<Animator>().SetTrigger("Death");
//			other.gameObject.GetComponent<PlayerHealth> ().takeDamageBullet (damage, "DamageDown");
//		}
//		NetworkServer.Destroy (gameObject);
//		Destroy(gameObject);
//	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			//other.gameObject.GetComponent<Animator>().SetTrigger("Death");
			other.gameObject.GetComponent<PlayerHealth> ().takeDamageBullet (damage, "DamageDown");
			AudioSource.PlayClipAtPoint(SoundManager.instance.onHitClip, other.transform.position);
		}
		NetworkServer.Destroy (transform.root.gameObject);
		Destroy(transform.root.gameObject);
	}
}