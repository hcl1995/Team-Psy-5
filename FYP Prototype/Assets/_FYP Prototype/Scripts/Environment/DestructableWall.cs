using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DestructableWall : NetworkBehaviour
{
	//public Mesh destroyMesh;

	bool callOnce = false;
	public float health = 3;

	Animator animation;
	SoundEffect soundEffect;

	public Collider[] m_Collider;

	void Awake()
	{
		animation = GetComponent<Animator>();
		soundEffect = GetComponent<SoundEffect>();
	}

	void Update()
	{
		if (health <= 0 && callOnce == false)
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

		//CmdSwapMesh();
	}

//	[Command]
//	void CmdSwapMesh()
//	{
//		RpcSwapMesh();
//	}
//
//	[ClientRpc]
//	void RpcSwapMesh()
//	{
//		if (health <= 0 && callOnce == false)
//		{
//			gameObject.GetComponent<MeshFilter>().mesh = destroyMesh;
//			CmdAnimation("Break");
//			callOnce = true;
//		}
//	}

//	[Command]
//	void CmdAnimation(string anim){
//		RpcSetAnimation (anim);
//	}
//
	[ClientRpc]
	void RpcSetAnimation(string anim){
		animation.SetTrigger(anim);
		foreach (Collider c in m_Collider)
		{
			c.enabled = !c.enabled;
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if (isServer) {
			if (other.gameObject.CompareTag("Attack"))
			{
				health -= 1;
				soundEffect.PlaySFX(SFXAudioClipID.SFX_ATTACK);
				//if(health<=0)
				//Destroy(gameObject);			
			}
		}

	}

	void OnTriggerEnter(Collider other){
		if (isServer) {
			if (other.gameObject.CompareTag("Attack"))
			{
				health -= 1;
				soundEffect.PlaySFX(SFXAudioClipID.SFX_ATTACK);
				//if(health<=0)
				//Destroy(gameObject);
			}
		}

	}
}