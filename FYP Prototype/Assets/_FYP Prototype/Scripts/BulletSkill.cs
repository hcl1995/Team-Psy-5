﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletSkill : NetworkBehaviour
{
	Vector3 initialVelocity;

	float minVelocity = 10f;

	Vector3 lastFrameVelocity;
	Rigidbody rb;

	int bounceLimit;
	float travelingDistance;

	Vector3 startPos;
	Vector3 endPos;

	public float projectileSpeed;
	public float travelingThreshold;
	public float distance = 2;
	public float damage = 10;

	bool maxCharge = false;

	public GameObject impact;

	SoundEffect soundEffect;
	

	void OnEnable()
	{
		//playerSkillControl = transform.parent.GetComponent<PlayerSkillControl>();

		soundEffect = GetComponent<SoundEffect>();
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

//	void OnCollisionEnter(Collision other)
//	{
//		bounceLimit++;
//
//		// OnCollision way to look.
//		Vector3 dir = other.contacts[0].point - transform.position;
//		dir = dir.normalized;
//		//dir = -dir.normalized;
//
//		if (other.gameObject.CompareTag("Player"))
//		{
//			other.gameObject.GetComponent<PlayerHealth> ().takeDamageBullet (damage,"DamageDown", impact, other.transform.position);
//			//AudioSource.PlayClipAtPoint(SoundManager.instance.onHitClip, other.transform.position);
//			soundEffect.PlaySFXClip(soundEffect.selfServiceClip[0]);
//
//			if (maxCharge)
//			{
//				other.gameObject.transform.root.position += (gameObject.transform.root.forward * distance);
//			}
//			// For Bounce
////			NetworkServer.Destroy (gameObject);
////			Destroy(gameObject);
//		}
//		NetworkServer.Destroy (gameObject);
//		Destroy(gameObject);
//		//Bounce(other.contacts[0].normal);
//	}

	void OnTriggerEnter(Collider other)
	{
		bounceLimit++;

		// OnCollision way to look.
		//Vector3 dir = other.contacts[0].point - transform.position;
		//Vector3 dir = other.ClosestPointOnBounds(transform.position);
		//dir = dir.normalized;
		//dir = -dir.normalized;

		if (other.gameObject.CompareTag("Player"))
		{
			
			AudioSource.PlayClipAtPoint(soundEffect.selfServiceClip[0], other.transform.position, SoundManager.instance.GetSoundVolume() * SoundManager.instance.GetMasterVolume ());
	
			if (maxCharge)
			{
				other.gameObject.transform.root.gameObject.GetComponent<PlayerControl>().flying = true;
				other.gameObject.transform.root.gameObject.GetComponent<PlayerControl>().getOtherPos = transform.root.forward * distance;
			}
			other.gameObject.GetComponent<PlayerHealth> ().takeDamageBullet (damage,"DamageDown", impact, other.transform.position);
//			NetworkServer.Destroy (gameObject);
			Destroy(gameObject);
		}

		if (other.gameObject.layer == LayerMask.NameToLayer("AboveGround")){
			AudioSource.PlayClipAtPoint(soundEffect.selfServiceClip[0], other.transform.position, SoundManager.instance.GetSoundVolume() * SoundManager.instance.GetMasterVolume ());
			Destroy(gameObject);
		}
		//NetworkServer.Destroy (gameObject);
		//Destroy(gameObject);
		//Bounce(other.contacts[0].normal);
	}

	void Bounce(Vector3 collisionNormal)
	{
		//var speed = lastFrameVelocity.magnitude;
		//var direction = Vector3.Reflect(lastFrameVelocity.normalized, collisionNormal);

		var speed = initialVelocity.magnitude;
		var direction = Vector3.Reflect(initialVelocity.normalized, collisionNormal);

		transform.rotation = Quaternion.LookRotation(direction);
		rb.velocity = direction * Mathf.Max(speed, minVelocity);
	}

	public void setMaxCharge(bool max){
		maxCharge = max;
	}
}