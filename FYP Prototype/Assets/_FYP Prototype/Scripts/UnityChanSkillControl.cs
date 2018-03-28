using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UnityChanSkillControl : PlayerControl
{
	[Header("Skill01")]
	public GameObject projectile;
	public Transform skill01SpawnPoint;
	public ParticleSystem skill01ParticleEffect;


	[Header("Skill02")]
	public GameObject skill02Object;
	public Transform skill02SpawnPoint;


	[Header("Ultimate")]
	//Vector3 endPos;
	//Vector3 startPos;

	//bool isUltimating;
	//float animationTime;
	//public float ultiLerpSpeed;
	//public float fixedDistance;

	public GameObject ultimateObject;
	public GameObject drillKickParticle;


	void Update()
	{
		if (!isLocalPlayer)
			return;
		CheckInput();
		InputSkills();
	}

	public void InputSkills()
	{
		if (state == playerState.Normal)
		{
			if (KeyBindingManager.GetKeyDown(KeyAction.Skill01))
			{
				if (skill01Cooldown <= 0)
				{
					CmdAnimation("Bullet");
					RotateTowardMouseDuringAction();
					CmdSkill01 (skill01SpawnPoint.position, skill01SpawnPoint.rotation);

					skill01CD.fillAmount = 1;
					skill01Cooldown = skill01CooldownDuration;

					//soundEffect.PlaySFX(SFXAudioClipID.SFX_U_SKILL01);
					soundEffect.PlaySFXClip(soundEffect.selfServiceClip[7]);
				}
			}
			else if (KeyBindingManager.GetKeyDown(KeyAction.Skill02))
			{
				if (skill02Cooldown <= 0)
				{
					CmdAnimation("Wall");

					skill02CD.fillAmount = 1;
					skill02Cooldown = skill02CooldownDuration;

					//soundEffect.PlaySFX(SFXAudioClipID.SFX_U_SKILL02);
					soundEffect.PlaySFXClip(soundEffect.selfServiceClip[9]);
				}
			}
			else if (KeyBindingManager.GetKeyDown(KeyAction.Ultimate))
			{
				if (ultimateCooldown <= 0)
				{
					RotateTowardMouseDuringAction();
					CmdAnimation("Ultimate");

					//startPos = transform.position;
					//endPos = transform.position += transform.forward * fixedDistance;

					//isUltimating = true;
					//CmdUltimateActive();

					ultimateCD.fillAmount = 1;
					ultimateCooldown = ultimateCooldownDuration;
					soundEffect.PlaySFXClip(soundEffect.selfServiceClip[10]);
				}
			}
		}

//		if (isUltimating)
//		{
//			animationTime += (Time.deltaTime * ultiLerpSpeed);
//			transform.position = Vector3.Lerp (startPos, endPos, animationTime);
//
//			CmdUltimateActive();
//		}
//
//		if (animationTime >= 1)
//		{
//			isUltimating = false;
//			animationTime = 0;
//		}
	}
		
	void Skill01Casting()
	{
		CmdSkill01PlayParticle();
	}

	[Command]
	void CmdSkill01PlayParticle()
	{
		RpcSkill01PlayParticle();
	}

	[ClientRpc]
	void RpcSkill01PlayParticle()
	{
		skill01ParticleEffect.Play();
	}

	void Skill02Active()
	{
		skill02Object.SetActive(true);
	}

	void UltimateActive()
	{
		ultimateObject.SetActive(true);
		drillKickParticle.SetActive(true);
		//soundEffect.PlaySFX(SFXAudioClipID.SFX_U_ULTIMATE);
		soundEffect.PlaySFXClip(soundEffect.selfServiceClip[10]);
	}

	void UltimateNotActive()
	{
		ultimateObject.SetActive(false);
		drillKickParticle.SetActive(false);
	}

	void UltimateMoving()
	{
		transform.position += transform.forward * 3;
	}

	[Command]
	public void CmdSkill01(Vector3 position, Quaternion rotation){
		var skill01 = Instantiate(projectile, position, rotation);
		NetworkServer.Spawn (skill01);
	}
}