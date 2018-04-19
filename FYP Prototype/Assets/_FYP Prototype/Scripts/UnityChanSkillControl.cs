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
	Vector3 endPos;
	Vector3 startPos;

	bool isUltimating;
	float animationTime;
	public float ultiLerpSpeed;
	public float fixedDistance;

	public GameObject ultimateObject;


	void Update()
	{
		if (!isLocalPlayer)
			return;
		CheckInput();
		InputSkills();
	}

	public void InputSkills()
	{
		if(lastResort == true)
			return;
		
		if (state == playerState.Normal)
		{
			if (KeyBindingManager.GetKeyDown(KeyAction.Skill01))
			{
				if (skill01Cooldown <= 0)
				{
					CmdAnimation("Bullet");
					RotateTowardMouseDuringAction();
					//CmdAnimation("Bullet");
					CmdSkill01 (skill01SpawnPoint.position, skill01SpawnPoint.rotation);

					skill01CD.fillAmount = 1;
					skill01Cooldown = skill01CooldownDuration;
					CmdPlaySFXClip(3);
				}
			}
			else if (KeyBindingManager.GetKeyDown(KeyAction.Skill02))
			{
				if (skill02Cooldown <= 0)
				{
					CmdAnimation("Wall");

					skill02CD.fillAmount = 1;
					skill02Cooldown = skill02CooldownDuration;
					CmdPlaySFXClip(4);
				}
			}
			else if (KeyBindingManager.GetKeyDown(KeyAction.Ultimate))
			{
				if (ultimateCooldown <= 0)
				{
					startPos = transform.position;
					endPos = transform.position += (transform.forward * fixedDistance);

					isUltimating = true;

					//RotateTowardMouseDuringAction();
					CmdAnimation("Ultimate");

					ultimateCD.fillAmount = 1;
					ultimateCooldown = ultimateCooldownDuration;
					CmdPlaySFXClip(5);
				}
			}
		}

		if (isUltimating)
		{
			animationTime += ultiLerpSpeed * Time.deltaTime;
			transform.position = Vector3.Lerp (startPos, endPos, animationTime);
		}

		if (animationTime >= 1)
		{
			isUltimating = false;
			animationTime = 0;
		}
	}

	[Command]
	void CmdSkill01(Vector3 position, Quaternion rotation){
		RpcSkill01PlayParticle();

		var bullet = Instantiate(projectile, position, rotation);
		NetworkServer.Spawn (bullet);
	}
		
	[ClientRpc]
	void RpcSkill01PlayParticle(){
		skill01ParticleEffect.Play();
	}

	void Skill02Active(){
		skill02Object.SetActive(true);
	}

	void UltimateActive(){
		ultimateObject.SetActive(true);
	}

	void UltimateNotActive(){
		ultimateObject.SetActive(false);
	}
}