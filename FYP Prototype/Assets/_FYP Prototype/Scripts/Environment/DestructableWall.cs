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
	public ParticleSystem FallParticle;
	public ParticleSystem breakParticle;

	public GameObject m_HitParticle;
	public GameObject m_FallParticle;
	public GameObject m_BreakParticle;

	//public GameObject spawnPoint;

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
			AudioSource.PlayClipAtPoint(soundEffect.selfServiceClip[1], transform.position, SoundManager.instance.GetSoundVolume() * SoundManager.instance.GetMasterVolume ());
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
		CmdHitParticles(3, new Vector3(transform.position.x, transform.position.y +1.5f, transform.position.z), transform.rotation);
		//breakParticle.Play();
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
		//hitParticle.Play();
		CmdHitParticles(1, new Vector3(transform.position.x, transform.position.y +1.5f, transform.position.z), transform.rotation);
		soundEffect.PlaySFXClip(soundEffect.selfServiceClip[0]);
	}

	void AnimEventPlayParticle()
	{
		CmdHitParticles(2, new Vector3(transform.position.x, transform.position.y +0.5f, transform.position.z), transform.rotation);
		//FallParticle.Play();
	}

	[Command]
	void CmdHitParticles(int destroyType, Vector3 position, Quaternion rotation){
		GameObject particleType = m_HitParticle;
		switch (destroyType) {
		case 1:
			particleType = m_HitParticle;
			break;
		case 2:
			particleType = m_FallParticle;
			break;
		case 3:
			particleType = m_BreakParticle;
			break;
		}

		var particleHit = Instantiate(particleType, position, rotation);
		NetworkServer.Spawn (particleHit);
		Destroy(particleHit, 1.5f);
	}

	// use back old aren't that bad also
}