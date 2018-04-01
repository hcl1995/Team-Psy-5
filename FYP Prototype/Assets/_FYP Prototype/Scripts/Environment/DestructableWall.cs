using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DestructableWall : NetworkBehaviour
{
	//public Mesh destroyMesh;

	protected bool callOnce = false;
	public float health;
	public float breakHP;

	Animator animation;
	protected SoundEffect soundEffect;

	public Collider[] m_Collider;

	public ParticleSystem hitParticle;
	public ParticleSystem breakParticle;

	void Awake()
	{
		animation = GetComponent<Animator>();
		soundEffect = GetComponent<SoundEffect>();
	}

	protected virtual void Update()
	{
		PropChuiDiao();
	}

	void PropChuiDiao()
	{
		if (health <= breakHP && callOnce == false)
		{
			//gameObject.GetComponent<MeshFilter>().mesh = destroyMesh;
			RpcSetAnimation("Break");
			//			foreach (Collider c in m_Collider)
			//			{
			//				c.enabled = !c.enabled;
			//			}
			//m_Collider.enabled = !m_Collider.enabled;
			callOnce = true;
		}
		if (health <= 0)
		{
			AudioSource.PlayClipAtPoint(soundEffect.selfServiceClip[1], transform.position);
			Destroy(gameObject);
		}
	}

	[ClientRpc]
	protected void RpcSetAnimation(string anim){
		animation.SetTrigger(anim);
		foreach (Collider c in m_Collider)
		{
			c.enabled = !c.enabled;
		}
		breakParticle.Play();
	}

	//	[ClientRpc]
	//	void RpcSwapMesh()
	//	{
	//			gameObject.GetComponent<MeshFilter>().mesh = destroyMesh;
	//		}
	//	}

	void OnTriggerEnter(Collider other){
		if (isServer) {
			if (other.gameObject.CompareTag("Attack") && health > 0)
			{
				health -= 1;
				RpcHitParticlePLUSSound();
			}
		}
	}

	[ClientRpc]
	void RpcHitParticlePLUSSound()
	{
		hitParticle.Play();
		soundEffect.PlaySFXClip(soundEffect.selfServiceClip[0]);
	}
}