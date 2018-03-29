using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UnityChanSkill01 : NetworkBehaviour
{
	Vector3 initialVelocity;

	Rigidbody rb;

	float travelingDistance;

	Vector3 startPos;
	Vector3 endPos;

	public float projectileSpeed;
	public float travelingThreshold;

	public float damage = 10;

	public GameObject impact;

	SoundEffect soundEffect;

	void OnEnable()
	{
		rb = GetComponent<Rigidbody>();
		soundEffect = GetComponent<SoundEffect>();

		initialVelocity = transform.forward * projectileSpeed;
		rb.velocity = initialVelocity;

		startPos = transform.position;
	}

	void Update()
	{
		endPos = transform.position;

		travelingDistance = Vector3.Distance(startPos, endPos);

		if (travelingDistance >= travelingThreshold)
		{
			Destroy(gameObject);
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
			other.gameObject.GetComponent<PlayerHealth> ().takeDamageBullet (damage, "DamageDown", impact, other.gameObject.GetComponent<Collider> ().ClosestPointOnBounds (transform.position));
			//AudioSource.PlayClipAtPoint(SoundManager.instance.onHitClip, other.transform.position);
			soundEffect.PlaySFXClip(soundEffect.selfServiceClip[0]);
		}
		//NetworkServer.Destroy (transform.root.gameObject);
		//Destroy(transform.root.gameObject);
		Destroy(gameObject);
	}
}